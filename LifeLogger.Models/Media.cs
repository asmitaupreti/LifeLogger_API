using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LifeLogger.Models
{
    public class Media
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MediaID { get; set; }

        [Required]
        public string MediaUrl { get; set; }

        public string Caption { get; set; }

        public virtual ICollection<LifeIncident> LifeIncidents { get; set; }

    }

}