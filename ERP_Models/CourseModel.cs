namespace ERP_Models
{
    public class CourseModel
    {
        public int course_id {  get; set; }
        public string course_name {  get; set; }
        public List<CourseFeeModel> courseFees { get; set; }
        public List<TopicModel> topics { get; set; }

    }
    public class CourseFeeModel
    {
        public int course_id { get; set; }
        public string course_name { get; set; }
        public int fee_id { get; set; }
        public string fee_mode { get; set; }
        public float fees_amount { get; set; }
        public float gst { get; set; }
        public DateTime fees_change_date  { get; set; }

    }


    //public class  TopicModel
    //{
      
    //    public int topic_id { get; set; }
    //    public string topic_name { get; set; }
    //    public List<ContentModel> contents { get; set; }
    //}
    //public class  ContentModel
    //{
         
    //    public int content_id { get; set; }
    //    public string content_name { get; set; }
    //}
}
