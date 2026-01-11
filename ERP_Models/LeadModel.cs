using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class LeadModel
    {
        public int lead_id { get; set; }
        public string candidate_name { get; set; }
        public string email_address { get; set; }
        public string mobile_number { get; set; }
        public string training_type { get; set; }
        public string description { get; set; }
        public DateTime lead_date { get; set; }
    }
}
