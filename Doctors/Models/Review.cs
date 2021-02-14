using System;
namespace Doctors.Models
{
    public class Review
    {
        public int ProfessionalismRate { get; set; }
        public int AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public int WaitingTimeRate { get; set; }
        public int ServiceRate { get; set; }
    }
}
