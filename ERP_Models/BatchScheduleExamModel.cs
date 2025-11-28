using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class BatchScheduleExamModel
    {
        public int batch_id {  get; set; }
        public string batch_name { get; set; }
        public DateTime exam_date { get; set; }
        public DateTime start_time { get; set; }
        public int topic_id {  get; set; }
        public string topic_name { get; set; }
    }
}
