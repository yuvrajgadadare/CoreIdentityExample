using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class GuestRegistrationModel
    {
        public int registration_id { get; set; }
        public int student_id { get; set; }
        public string student_code { get; set; }
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public string student_name { get; set; }
        public string local_address { get; set; }
        public string password { get; set; }
        public string mobile_number { get; set; }
        public string whatsapp_number { get; set; }
        public string gender { get; set; }
        public string email_address { get; set; }
        public string last_name { get; set; }
        public DateTime birth_date { get; set; }
        public DateTime registration_date { get; set; }
        public string current_status { get; set; }
        public float discount { get; set; }
        public int fee_id { get; set; }
        public string permanent_identification_number { get; set; }
    }
}
