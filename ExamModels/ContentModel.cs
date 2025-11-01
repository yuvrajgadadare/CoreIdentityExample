namespace ExamModels
{
    public class ContentModel
    {
        public int topic_id {  get; set; }
        public string topic_name {  get; set; }
        public int content_id {  get; set; }
        public string content_name{  get; set; }
        public int total_program_questions {  get; set; }
        public int total_interview_questions {  get; set; }
        public List<ContentQuestionModel> contentQuestions { get; set; }
     


    }
}
