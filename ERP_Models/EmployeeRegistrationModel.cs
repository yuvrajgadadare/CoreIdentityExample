using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class EmployeeRegistrationModel
    {

        [Required(ErrorMessage = "First Name is Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailAvailable", controller: "Account")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]

        public string gender { get; set; }
 
 
        [Required]
        public string mobile_number { get; set; }
 
        [Required]
        public DateTime birth_date { get; set; }
        [Required]
        public DateTime joining_date { get; set; }
     
     
        public string qualification { get; set; }
        // public string password { get; set; }
        public float salary { get; set; }
    }
}
