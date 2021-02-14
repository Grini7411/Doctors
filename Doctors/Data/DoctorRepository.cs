using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Doctors.Models;
using System.Linq;
using Newtonsoft.Json.Linq;
using Doctors.DTOs;
using Newtonsoft.Json;

namespace Doctors.Data
{
    public class DoctorRepository : IDoctorRepository
    {
        public async Task<List<Doctor>> GetAll(bool isActive, bool isPaying)
        {
            try
            {
                string articlePath = "./assets/articles.json";
                using FileStream articlesRead = File.OpenRead(articlePath);
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                List<Article> allArticles = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Article>>(articlesRead, options);

                string langPath = "./assets/language.json";
                var readLang = File.ReadAllText(langPath);
                JObject allLangsIds = JObject.Parse(readLang);
                allLangsIds = (JObject)allLangsIds["language"];

                string doctorsPath = "./assets/doctors.json";
                using FileStream openStream = File.OpenRead(doctorsPath);
                List<Doctor> allDoctors = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Doctor>>(openStream, options);
                IQueryable<Doctor> allDoctorsQueryable = allDoctors.AsQueryable();
                if (isActive)
                {
                    allDoctors = allDoctorsQueryable.Where(x => x.IsActive == true).ToList();
                }
                if (isPaying)
                {
                    allDoctors = allDoctorsQueryable.Where(x => x.PromotionLevel <= 5).ToList();
                }

                // Order List:
                allDoctors = allDoctors.OrderByDescending(o => o.Reviews.AverageRating).OrderByDescending(x => x.Reviews.TotalRatings).OrderByDescending(x => x.Reviews.ProfessionalismRate).ToList();

                // Change LanguageId to language string:
                foreach (var doctor in allDoctors)
                {
                    // Validate PhoneNum
                    for (int j = 0; j < doctor.Phones.Count; j++)
                    {
                        doctor.Phones[j] = CheckNumber(doctor.Phones[j]);
                    }
                    for (int i = 0; i < doctor.LanguageIds.Length; i++)
                    {
                        doctor.LanguageIds[i] = allLangsIds[doctor.LanguageIds[i]].ToString();
                    }
                    doctor.HasArticle = allArticles.AsQueryable().Where(x => x.AuthorName == doctor.FullName).Any();
                }
                return allDoctors;
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
        }

        public void SaveToFile(PostContactDoctorDTO dto)
        {
            string path = "./assets/contact.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            string json = JsonConvert.SerializeObject(dto);
            File.AppendAllText(path, json);
        }

        private Phone CheckNumber(Phone phone)
        {
            if (!phone.Number.Contains('-'))
            {
                if (phone.PhoneType != 0 || phone.Number.StartsWith("07"))
                {
                    phone.Number = phone.Number.Insert(3, "-");
                }
                else
                {
                    phone.Number = phone.Number.Insert(2, "-");
                }
            }
            return phone;
        }
    }
}
