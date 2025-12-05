namespace ERP_Models
{
    public class ExamQuestionModel
    {
        public int exam_id { get; set; }
        public int question_id { get; set; }
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public int exam_question_id { get; set; }
        public string question { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public string option4 { get; set; }
        public int submitted_option_number { get; set; }
        public int correct_option_number { get; set; }

    }
}
