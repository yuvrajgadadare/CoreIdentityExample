namespace ERP_Models
{
    public class TopicModel
    {
        public int topic_id {  get; set; }
        public string topic_name {  get; set; }
        public string folder_id {  get; set; }
        public bool is_selected { get; set; }
        public bool video_status { get; set; }
        public int total_program_count{get; set; }
        public int total_interview_question_count{get; set; }
        public int total_content_question_count{get; set; }
        public List<ContentModel> contents { get; set; }
        public List<VideoModel> videos { get; set; }

    }
}
