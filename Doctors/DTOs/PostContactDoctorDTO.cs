using System;
namespace Doctors.DTOs
{
    public class PostContactDoctorDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int DoctorId { get; set; }
    }
}
