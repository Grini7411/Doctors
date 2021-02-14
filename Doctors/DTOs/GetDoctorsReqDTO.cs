using System;
namespace Doctors.DTOs
{
    public class GetDoctorsReqDTO
    {
        public bool IsActive { get; set; }
        public bool IsPaying { get; set; }
        public string DoctorId { get; set; }
    }
}
