namespace ERP_Models
{
    public class StudentMarkAttendance
    {
        public int student_id {  get; set; }
        public string student_name {  get; set; }
        public int registration_id {  get; set; }
        public int batch_id {  get; set; }
        public string batch_name{  get; set; }
        public int topic_id {  get; set; }
        public string topic_name {  get; set; }
        public int content_id { get; set; }

        public string content_name {  get; set; }
        public DateTime expected_date {  get; set; }
        public DateTime actual_date {  get; set; }
        public DateTime attendance_date {  get; set; }
        public int is_present { get; set; }
        public string attendance { get; set; }


    }
}
