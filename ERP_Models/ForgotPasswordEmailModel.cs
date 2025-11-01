using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class ForgotPasswordEmailModel
    {
        
        [Required(ErrorMessage = "please enter email address")]
        [EmailAddress(ErrorMessage ="invalid email address")]
        public string email_address { get; set; }
    }
}
