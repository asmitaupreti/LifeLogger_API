using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace LifeLogger.Models
{
    public class ApplicationUser:IdentityUser {

		[Required]
        public string Name { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
        public string ProfilePicturePath { get; set; }
        public DateTime DateJoined { get; set; } = DateTime.Now;
        public string Role { get; set; }
    }
}