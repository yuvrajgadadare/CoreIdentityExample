using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class EmployeeLoginModel
    {
        [Required(ErrorMessage ="please enter employee code")]
        public string employee_code {  get; set; }

        [Required(ErrorMessage ="please enter password")]
        public string password {  get; set; }
    }
}
