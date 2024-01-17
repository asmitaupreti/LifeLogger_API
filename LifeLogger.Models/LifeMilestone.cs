using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LifeLogger.Models
{
    public class LifeMilestone:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MilestoneID { get; set; }

        public int ProjectID { get; set; }
        [ForeignKey("ProjectID")]
        [ValidateNever]
        public LifeProject LifeProject { get; set; } // Navigation property
       
        [Required]
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


        public virtual ICollection<LifeIncident> LifeIncidents { get; set; }

        public virtual ICollection<MilestoneTagMapping> MilestoneTagMappings { get; set; } // Linking property for many-to-many relation

        public virtual ICollection<MilestoneReportMapping> MilestoneReportMappings { get; set; }
    }
}