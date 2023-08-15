using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LifeLogger.Models
{
    public class LifeIncident
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IncidentID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public DateTime IncidentDate { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        // For severity, consider using an Enum with defined severity levels.
        public int Severity { get; set; }

        public virtual ICollection<Media> Media { get; set; }

        
        public int MilestoneID { get; set; }
        [ForeignKey("MilestoneID")]
        public virtual LifeMilestone LifeMilestone { get; set; }

        [MaxLength(255)]
        public string Location { get; set; }
    }
}