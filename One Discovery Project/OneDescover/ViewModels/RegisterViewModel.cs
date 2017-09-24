using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneDiscovery.ViewModels
{
    public class RegisterViewModel
    {
       
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name="User Name")]
        [Required(ErrorMessage = "Please Enter Email Address")]    
        [MaxLength(150)]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 8, ErrorMessage = "Password Must be Longer than eight digit")]
        [Display(Name="Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        [Display(Name="Confirm Password")]
        public string ConfirmPassword { get; set; }

  

        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [MaxLength(150)]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Last Name must be Alphabets")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }



        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "First Name must be Alphabets")]
        [Display(Name = "First Name")]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile No")]
        [Display(Name = "Mobile")]
        //[StringLength(10, ErrorMessage = "The Mobile must contains 10 characters", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string Phone { get; set; } 

        [Required]
        [Display(Name = "Age")]
        [Range(1, 120)]
        public int Age { get; set; }
    }
}