using System.ComponentModel.DataAnnotations;

namespace LifeLogger.Models.DTO
{
    public class LifeMilestoneUpdateDTO
    {
        [Required(ErrorMessage ="MilestoneID field shouldnot be empty")]
         public int MilestoneID { get; set; }

        [Required(ErrorMessage ="ProjectID field shouldnot be empty")]
        public int ProjectID { get; set; }

        [MaxLength(255)]
        public string MilestoneName { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(255)]
        public string Location { get; set; }

        // For PeopleInvolved, a JSON string might be useful if there are multiple fields for each person.
        public string PeopleInvolved { get; set; }

        [MaxLength(50)]
        public string Sentiment { get; set; }

        public int? AchievementLevel { get; set; }

        public string RelatedLinks { get; set; }  // Can be a JSON object or an array of URLs

        [MaxLength(1000)]
        public string PrivateNotes { get; set; }

        [MaxLength(50)]
        public string Weather { get; set; }

        [MaxLength(255)]
        public string FinancialImpact { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }
    }
}