using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeLogger.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email{ get; set; }

        [Required]
        public string Password{ get; set; }
    }
}