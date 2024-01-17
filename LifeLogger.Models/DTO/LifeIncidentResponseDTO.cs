using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LifeLogger.Models.DTO
{
    public class LifeIncidentResponseDTO
    {
        public int IncidentID { get; set; }

        public string Title { get; set; }

        public DateTime IncidentDate { get; set; }

        public string Description { get; set; }

        // For severity, consider using an Enum with defined severity levels.
        public int Severity { get; set; }

        public int  MediaCount { get; set; }

        
        public int MilestoneID { get; set; }
       
        public string Location { get; set; }
    }
}