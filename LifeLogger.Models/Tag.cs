using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace LifeLogger.Models
{
    public class Tag:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagID { get; set; }

        [Required]
        [MaxLength(100)]
        public string TagName { get; set; }
        
        public virtual ICollection<MilestoneTagMapping> MilestoneTagMappings { get; set; } // Linking property for many-to-many relation
    }
}