using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class InterviewQuestionFormModel
    {
        public int content_id {  get; set; }
        public List<InterviewQuestionModel> questions { get; set; }
    }
}
