using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LifeLogger.Models
{
    public class LifeProject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        
        public string UserID { get; set; }
        // Navigation Property
         [ForeignKey("UserID")]
         [ValidateNever]
        public ApplicationUser User { get; set; }

        [Required]
        [MaxLength(255)] // You can adjust the length as needed.
        public string Title { get; set; }

        [MaxLength(2000)] // Adjust length as needed. This gives a long description.
        public string Description { get; set; }

        [MaxLength(500)] // Adjust length for location details.
        public string Location { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Required]
        public bool IsPublic { get; set; }


        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        
    }

}