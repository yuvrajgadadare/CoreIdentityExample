using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class ForgotPasswordModel
    {
        public int student_id {  get; set; }
        [Required(ErrorMessage ="please enter password")]
        public string password { get; set; }
        [Required(ErrorMessage = "please enter confirm password")]
        [Compare("password",ErrorMessage ="password and confirm password does not matched")]
        public string confirm_spassword { get; set; }
    }
}
