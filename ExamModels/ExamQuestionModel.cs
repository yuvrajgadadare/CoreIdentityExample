using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamModels
{
    public class ExamQuestionModel
    {
        public int QuestionId {  get; set; }
        public int ContentId {  get; set; }
        public int TopicId {  get; set; }
        public string Question {  get; set; }
        public string ContentName {  get; set; }
        public string TopicName {  get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public int SubmittedOptionNumber {  get; set; }
        public int CorrectOptionNumber {  get; set; }
        public string Status { get; set; }

    }
}
