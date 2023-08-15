using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeLogger.Models
{
    public class IncidentMediaMapping
    {     
        public int IncidentId { get; set; }
        [ForeignKey("IncidentId")]
        public virtual LifeIncident LifeIncident { get; set; }  // Navigation property
  
        public int MediaId { get; set; }
        [ForeignKey("MediaId")]
        public virtual Media Media { get; set; }  // Navigation property
    }
}