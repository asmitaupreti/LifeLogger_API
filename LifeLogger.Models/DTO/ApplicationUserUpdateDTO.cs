
using System.ComponentModel.DataAnnotations;

namespace LifeLogger.Models.DTO
{
    public class ApplicationUserUpdateDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
        public string PhoneNumber{ get; set; }
        public string ProfilePicturePath { get; set; }
        public string Role { get; set; }
    }
}