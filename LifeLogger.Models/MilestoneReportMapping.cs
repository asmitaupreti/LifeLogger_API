using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeLogger.Models
{
    public class MilestoneReportMapping
    {
        public int MilestoneId { get; set; } 
        [ForeignKey("MilestoneId")]// FK to LifeMilestone
        public virtual LifeMilestone LifeMilestone { get; set; }

        public int? ReportId { get; set; }   
        [ForeignKey("ReportId")] // FK to LifeReport
        public virtual LifeReport LifeReport { get; set; }
    }
}