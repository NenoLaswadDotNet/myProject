using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDiscovery.Model
{
    public class User
    {

        public int UserID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Display(Name = "First Name")]
        [MaxLength(150)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        [MaxLength(150)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }      

        [Display(Name = "Age")]
        [Range(1, 120)]
        public int Age { get; set; }

       
        //Use Email Address to log in
        [Required(ErrorMessage = "Please Enter Email Address")]
        [Display(Name = "Email")]
        [MaxLength(150)]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        [Index(IsUnique = true)]

        public string Email { get; set; }


        [Required(ErrorMessage = "Please Enter Mobile No")]
        [Display(Name = "Mobile")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string Phone { get; set; }

        [MaxLength(150)]
        public string AddressLine { get; set; }     

        [MaxLength(50)]
        [Display(Name="Unit")]
        public string UnitOrApartmentNumber { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(50)]
        public string State { get; set; }

        [Display(Name="Zip Code")]
        [MaxLength(20)]
        public string ZipCode { get; set; }

        [Display(Name="Password")]
        [MaxLength(64)]
        [MinLength(8)]
        [Required]
        [StringLength(150, MinimumLength = 8, ErrorMessage = "Password Must be Longer than eight digit")]
        public string Password { get; set; }


        [ScaffoldColumn(false)]
        public string Salt { get; set; }

        [ScaffoldColumn(false)]
        public bool IsLocked { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? LastLockedDateTime { get; set; }

        [ScaffoldColumn(false)]
        public int? FailedAttempts { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
