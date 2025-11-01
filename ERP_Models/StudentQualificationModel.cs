using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class StudentQualificationModel
    {
       public int qualification_id { get; set; }
        public int student_id { get; set; }
        public string student_name { get; set; }
        public string qualification { get; set; }
        public int passing_year { get; set; }
        public string university { get; set; }
        public string medium { get; set; }
        public float percentage { get; set; }
    }
}
