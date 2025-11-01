namespace ERP_Models
{
    public class BatchModel
    {
        public int batch_id {  get; set; }
        public string ?batch_name {  get; set; }
        public int topic_id {  get; set; }
        public string? topic_name {  get; set; }
        public int employee_id {  get; set; }
        public string? employee_name {  get; set; }
        public DateTime start_date {  get; set; }
        public DateTime end_date {  get; set; }
        public string? batch_time {  get; set; }
        public bool is_schedule_generated{  get; set; }
        public int total_students { get; set; }
        public int total_leactures { get; set; }
        public int attended_leatures { get; set; }
        public int remaining_leatures { get; set; }
        public string? status { get; set; }
        public string? batch_status { get; set; }
        public float completed_percentage { get; set; }
        public bool is_schedule_exams_generated {  get; set; }
        public List<ExamModel> exams { get; set; }

    }
}
