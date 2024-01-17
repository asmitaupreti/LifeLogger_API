using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LifeLogger.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id{ get; set; }
        public string UserId{ get; set; }
        public string JwtTokenId{ get; set; }
        public string Refresh_Token{ get; set; }
        public bool IsValid{ get; set; } // makes sure the refresh token is only valid for one use
        public DateTime ExpiresAt{ get; set; }
    }
}