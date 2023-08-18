using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeLogger.Models.DTO
{
    public class TagUpdateDTO
    {
        [Required(ErrorMessage ="Tag Id cannot be empty")]
        public int TagID { get; set; }
        
        [MaxLength(100)]
        public string TagName { get; set; }
    }
}