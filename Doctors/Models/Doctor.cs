using System;
using System.Collections.Generic;

namespace Doctors.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public int PromotionLevel { get; set; }
        public string FullName { get; set; }
        public string[] LanguageIds { get; set; }
        public List<Phone> Phones { get; set; }
        public Review Reviews { get; set; }
        public bool HasArticle { get; set; }
        public bool IsActive { get; set; }

    }
}
