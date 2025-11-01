namespace ERP_Models
{
    public class BatchStudentModel
    {
        public int batch_student_id { get; set; }
        public int batch_id { get; set; }
        public string batch_name { get; set; }
        public int topic_id { get; set; }
        public int employee_id { get; set; }
        public string employee_name { get; set; }
        public string topic_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime registration_date { get; set; }
        public string batch_time { get; set; }
        public int student_id { get; set; }
        public int registration_id { get; set; }
        public string student_name{ get; set; }
        public string course_name{ get; set; }
        public string status { get; set; }
        public int total_leactures { get; set; }
        public int attended_leactures { get; set; }
        public int remaining_leactures { get; set; }
        public List<BatchScheduleModel> schedule { get; set; }
        public List<StudentMarkAttendance> attendance { get; set; }
    }
}
