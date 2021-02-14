using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doctors.DTOs;
using Doctors.Models;

namespace Doctors.Data
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAll(bool isActive, bool isPaying);
        void SaveToFile(PostContactDoctorDTO dto);
    }
}
