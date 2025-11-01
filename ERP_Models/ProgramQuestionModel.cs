using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class ProgramQuestionModel
    {
        public int topic_id { get; set; }
        public string topic_name { get; set; }
        public int content_id { get; set; }
        public string content_name { get; set; }
        public int program_question_id { get; set; }
        public string question_title { get; set; }
        public string question_description { get; set; }
        public List<ProgramAnswerModel> answers { get; set; }
    }
}
