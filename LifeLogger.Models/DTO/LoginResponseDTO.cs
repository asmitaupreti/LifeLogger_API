using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LifeLogger.Models.DTO
{
    public class LoginResponseDTO
    {
        public ApplicationUserResponseDTO User{ get; set; }
        public string Token{ get; set; }
    }
}