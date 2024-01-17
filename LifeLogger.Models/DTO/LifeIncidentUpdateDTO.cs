using System.ComponentModel.DataAnnotations;

namespace LifeLogger.Models.DTO
{
    public class LifeIncidentUpdateDTO
    {
         [Required(ErrorMessage ="IncidentID field shouldnot be empty")]
         public int IncidentID { get; set; }

        [Required(ErrorMessage ="MilestoneID field shouldnot be empty")]
        public int MilestoneID { get; set; }

       
        [MaxLength(255)]
        public string Title { get; set; }
        
        
        public DateTime IncidentDate { get; set; }

        [MaxLength(255)]
        public string Location { get; set; }

        
        public int Severity { get; set; }

       
        public string Description { get; set; }        
    }
}