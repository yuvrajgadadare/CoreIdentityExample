namespace ERP_Models
{
    public class TopicStudentModel
    {
        public int student_id {  get; set; }
        public int registration_id {  get; set; }
        public int course_id {  get; set; }
        public int topic_id { get; set; }
        public string topic_name { get; set;}
        public string student_name { get; set;}
        public string course_name { get; set; }
        public int branch_id { get; set; }
        public string branch_name { get; set; }
    }
}
