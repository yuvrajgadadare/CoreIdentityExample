using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class JobOpeningModel
    {
        public int opening_id { get; set; }
        public DateTime opening_date { get; set; }
        public string company_name { get; set; }
        public string location { get; set; }
        public string technnology { get; set; }
        public string experience_required { get; set; }
        public string passing_year { get; set; }
        public string job_description { get; set; }
        public string offering_package { get; set; }
    }
}
