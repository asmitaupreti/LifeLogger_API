using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeLogger.Models
{
    public class LifeReport:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }
        public string ReportType { get; set; } // e.g., "Performance", "Productivity"
        public string ReportLink { get; set; }
        public virtual ICollection<MilestoneReportMapping> MilestoneReportMappings { get; set; }

    }
}