

using System.ComponentModel.DataAnnotations;

namespace LifeLogger.Models.DTO
{
    public class TagCreateDTO
    {
        [Required(ErrorMessage ="Tag name cannot be empty")]
        [MaxLength(100)]
        public string TagName { get; set; }
    }
}