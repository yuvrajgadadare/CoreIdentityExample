using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class StudentCertificationModel
    {
        public string permanent_identification_number { get; set; }
        public int student_id { get; set; }
        public int registration_id { get; set; }
        public string status { get; set; }
 
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public string student_name { get; set; }
        public string course_name { get; set; }
        public int total_exams { get; set; }
        public float average_percentage { get; set; }
        public string grade { get; set; }
        public string CertificationCode { get; set; }
    }
}
