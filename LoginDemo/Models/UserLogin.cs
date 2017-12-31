using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginDemo.Models
{
    public class UserLogin
    {
        [Display(Name ="Enter Email ID")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Email is required")]
        public string EmailID{get;set;}

        [DataType(DataType.Password)]
        [Display(Name ="Enter Password")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Password is required")]
        public string password { get; set; }
        
        [Display(Name ="Remember Me..!")]
        public bool RememberMe { get; set; }
    }
}