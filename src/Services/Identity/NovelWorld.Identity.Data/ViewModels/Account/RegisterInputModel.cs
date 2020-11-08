using System;
using System.ComponentModel.DataAnnotations;

namespace NovelWorld.Identity.Data.ViewModels.Account
{
    public class RegisterInputModel
    {
        [Required]
        public string Account { get; set; }
        
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        
        public string Address { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DoB { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        [Display(Name = "Re-input Password")]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string PasswordCheck { get; set; }
    }
}