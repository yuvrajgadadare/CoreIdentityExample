using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class InterviewQuestionModel
    {
        public int topic_id { get; set; }
        public string topic_name { get; set; }
        public int content_id { get; set; }
        public string content_name { get; set; }
        public int question_id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
    }
}
