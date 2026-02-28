using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your Email address.")]
        [EmailAddress(ErrorMessage = "The Email address is not valid.")]
        public string EmailAddress { get; set; } = null!;
    }
}
