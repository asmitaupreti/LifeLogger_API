using System.ComponentModel.DataAnnotations;
namespace LifeLogger.Models.DTO
{
    public class LifeProjectCreateDTO
    {
        [Required(ErrorMessage = "UserId shouldnot be empty")]
        public string UserID { get; set; }
        
        [Required(ErrorMessage = "Title shouldnot be empty")]
        public string Title { get; set; }

        [MaxLength(2000)] // Adjust length as needed. This gives a long description.
        public string Description { get; set; }

        [MaxLength(500)] // Adjust length for location details.
        public string Location { get; set; }

        [Required(ErrorMessage = "Start Date  shouldnot be empty")]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Required(ErrorMessage = "IsPublic field shouldnot be empty")]
        public bool IsPublic { get; set; }

      
    }
}