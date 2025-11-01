using System.ComponentModel.DataAnnotations;

namespace ERP_Models
{
    public class StudentLoginModel
    {
        [Required(ErrorMessage ="please enter email address")]
        [EmailAddress(ErrorMessage ="invalid email address")]
        public string email_address {  get; set; }
        [Required(ErrorMessage ="please enter password")]
        public string password {  get; set; }
    }
}
