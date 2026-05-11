namespace ERP_Models
{
    public class ExamModel
    {
        public int exam_id { get; set; }
        public int student_id { get; set; }
        public int registration_id { get; set; }
        public string status { get; set; }
        public int is_attended { get; set; }
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public string student_name { get; set; }
        public string email_address { get; set; }
        public string mobile_number { get; set; }
        public DateTime exam_date { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public string start_time_string { get; set; }
        public string end_time_string { get; set; }
        public int topic_id { get; set; }
        public string topic_name { get; set; }
        public int total_questions {  get; set; }
        public int total_correct_questions {  get; set; }
        public int total_wrong_questions {  get; set; }
        public float percentage {  get; set; }
        public string grade {  get; set; }
        public List<ExamQuestionModel> examQuestions { get; set; }
        //public List<ExamResultModel> resultquestions { get; set; }
    }
    public class ExamFinalModel
    {
        public int student_id { get; set; }
        public int topic_id { get; set; }
        //    public string topic_name { get; set; }
        public DateTime exam_date { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
    }

    }
