using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class GuestRegistrationFormModel
    {
       
        [Required(ErrorMessage = "*")]
        public int? branch_id { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("[a-zA-Z]{1,14}", ErrorMessage = "only characters are required")]
        public string student_name { get; set; }


        [Required(ErrorMessage = "*")]
        public string local_address { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("[0-9]{10}", ErrorMessage = "invalid mobile number")]

        public string mobile_number { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("[0-9]{10}", ErrorMessage = "invalid mobile number")]
        public string whatsapp_number { get; set; }
        [Required(ErrorMessage = "*")]
        public string gender { get; set; }

        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "invalid email address")]
        // [RegularExpression("[a-zA-Z0-9]+[a-zA-Z]*[@][a-zA-Z]+[.][a-zA-Z]+",ErrorMessage ="Invalid email address")]
        public string email_address { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression("[a-zA-Z]{1,14}", ErrorMessage = "only characters are required")]
        public string last_name { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime? birth_date { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime? registration_date { get; set; }
       
        [Required(ErrorMessage = "*")]
        public int? fee_id { get; set; }
    }
}
