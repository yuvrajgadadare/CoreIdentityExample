using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class BatchExamModel
    {
        public int exam_id { get; set; }
        public int student_id { get; set; }
        public int registration_id { get; set; }
        public string status { get; set; }
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public int batch_id { get; set; }
        public string batch { get; set; }
        public int is_attended { get; set; }

        public string student_name { get; set; }
     
        public DateTime exam_date { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public int topic_id { get; set; }
        public string topic_name { get; set; }
        public int total_questions { get; set; }
        public int total_correct_questions { get; set; }
        public int total_wrong_questions { get; set; }
        public float percentage { get; set; }
        public string grade { get; set; }
        public List<ExamQuestionModel> examQuestions { get; set; }
    }
}
