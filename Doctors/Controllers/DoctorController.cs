using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doctors.Data;
using Doctors.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Doctors.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorRepository _doctorRepository;
        public DoctorController(DoctorRepository doctorRepo)
        {
            _doctorRepository = doctorRepo;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAllDoctors([FromQuery] bool isActive, [FromQuery] bool isPaying)
        {
            List<Models.Doctor> doctors = await _doctorRepository.GetAll(isActive, isPaying);
            return Ok(doctors);
        }
        [HttpPost]
        public IActionResult ContactDoctor([FromBody] PostContactDoctorDTO bodyObj)
        {
             _doctorRepository.SaveToFile(bodyObj);
            return Ok( new { success = true });
        }
    }
}
