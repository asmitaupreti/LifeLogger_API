using System.ComponentModel.DataAnnotations;

namespace LifeLogger.Models.DTO
{
    public class LifeProjectUpdateDTO
    {
        [Required(ErrorMessage = "ProjectId shouldnot be empty")]
        public int ProjectId { get; set; }

        public string Title { get; set; }

        [MaxLength(2000)] // Adjust length as needed. This gives a long description.
        public string Description { get; set; }

        [MaxLength(500)] // Adjust length for location details.
        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsPublic { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}