using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginDemo.Models
{
    [MetadataType(typeof(userMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }

    }
    public class userMetadata
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings=false, ErrorMessage="First name is mandetory") ]
        public string FirstName { get; set; }
        
        [Display(Name ="Last Name")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Last Name is Required")]
        public string LastName { get; set; }

        [Display(Name ="Email ID")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Email is required...!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name ="Date Of Birth")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Date of birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:DD/MM/YYY}")]
        public string DOB { get; set; }

        [Required(AllowEmptyStrings =false,ErrorMessage ="Password is required")]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Minimun length should be 6")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password not match..!")]
        public string ConfirmPassword { get; set; }
    }
}