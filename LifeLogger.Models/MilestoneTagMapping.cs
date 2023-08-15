using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;

namespace LifeLogger.Models
{
    public class MilestoneTagMapping
    {
        public int MilestoneID { get; set; }
        [ForeignKey("MilestoneID")]
        [ValidateNever]
        public virtual LifeMilestone LifeMilestone { get; set; }
        
        public int TagID { get; set; }
        [ForeignKey("TagID")]
        [ValidateNever]
        public virtual Tag Tag { get; set; }
    }
}