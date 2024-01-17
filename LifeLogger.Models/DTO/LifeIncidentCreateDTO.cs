using System.ComponentModel.DataAnnotations;

namespace LifeLogger.Models.DTO
{
    public class LifeIncidentCreateDTO
    {
        [Required(ErrorMessage ="MilestoneID field shouldnot be empty")]
        public int MilestoneID { get; set; }

        [Required(ErrorMessage ="Title field shouldnot be empty")]
        [MaxLength(255)]
        public string Title { get; set; }
        
        [Required(ErrorMessage ="IncidentDate field shouldnot be empty")]
        public DateTime IncidentDate { get; set; }

        [MaxLength(255)]
        public string Location { get; set; }

        [Required(ErrorMessage ="Severity field shouldnot be empty")]
        public int Severity { get; set; }

        [Required(ErrorMessage ="Title field shouldnot be empty")]
        public string Description { get; set; }        
       
       
    }
}