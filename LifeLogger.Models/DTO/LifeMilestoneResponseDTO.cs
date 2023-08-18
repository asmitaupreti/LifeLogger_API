
namespace LifeLogger.Models.DTO
{
    public class LifeMilestoneResponseDTO
    {
       
        public int MilestoneID { get; set; }

        public int ProjectID { get; set; }

        public string MilestoneName { get; set; }

        public DateTime Date { get; set; }
 
        public string Location { get; set; }

        // For PeopleInvolved, a JSON string might be useful if there are multiple fields for each person.
        public string PeopleInvolved { get; set; }

        public string Sentiment { get; set; }

        public int? AchievementLevel { get; set; }

        public string RelatedLinks { get; set; }  // Can be a JSON object or an array of URLs

        public string PrivateNotes { get; set; }

        public string Weather { get; set; }

        public string FinancialImpact { get; set; }

        public string Description { get; set; }
    }
}