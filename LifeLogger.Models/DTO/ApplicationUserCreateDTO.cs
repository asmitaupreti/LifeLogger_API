using System.ComponentModel.DataAnnotations;
namespace LifeLogger.Models.DTO
{
    public class ApplicationUserCreateDTO
    {
        [Required(ErrorMessage = "Please enter your first and last name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your Street name")]
		public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Please enter your City name")]
		public string City { get; set; }

        [Required(ErrorMessage = "Please enter your State name")]
		public string State { get; set; }

        [Required(ErrorMessage = "Please enter your Post code")]
		public string PostalCode { get; set; }

        [Required(ErrorMessage = "Please enter your Phone number")]
        [Phone(ErrorMessage = "Please enter a valid Phone number.")]
        public string PhoneNumber{ get; set; }

        public string ProfilePicturePath { get; set; }

        [Required]
        public DateTime DateJoined { get; set; }

        [Required(ErrorMessage = "Please enter your Password")]
        public string Password { get; set; } 

        [Required] 
        public string ConfirmPassword { get; set; } 

        public string Role { get; set; }
    }
}