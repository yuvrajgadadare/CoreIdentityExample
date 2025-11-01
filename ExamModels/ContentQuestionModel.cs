
namespace ExamModels
{
    public class ContentQuestionModel
    {
        public int  serial_number { get; set; }
        public int  topic_id { get; set; }
        public string  topic_name { get; set; }
        public int  content_id { get; set; }
        public string  content_name { get; set; }
        public int  question_id { get; set; }
        public string  question { get; set; }
        public string option1 { get; set; }
        public string  option2 { get; set; }
        public string option3 { get; set; }
        public string  option4 { get; set; }
        public int  correct_option_number { get; set; }
        public int  submitted_option_number { get; set; }


    }
}
