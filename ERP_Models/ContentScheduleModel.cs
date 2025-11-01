using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class ContentScheduleModel
    {
        public int content_id {  get; set; }
        public string content_name { get; set; }
        public DateTime expected_date { get; set; }
        public DateTime? actual_date { get; set; }
        public string status { get; set; }
        public int is_present { get; set; }
        public string attendance { get; set; }
    }
}
