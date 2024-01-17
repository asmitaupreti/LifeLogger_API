using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeLogger.Models.DTO
{
    public class TokenDTO
    {
        [Required(ErrorMessage ="Access Token is required")]
        public string AccessToken{ get; set; }
        
        [Required(ErrorMessage ="Previous Refresh Token is required")]
        public string RefreshToken { get; set; }
    }
}