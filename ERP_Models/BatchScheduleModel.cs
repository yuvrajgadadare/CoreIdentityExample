namespace ERP_Models
{
    public class BatchScheduleModel
    {
        public int batch_schedule_id { get; set; }
        public int batch_id { get; set; }
        public string batch_name { get; set; }
        public string batch_time { get; set; }
        public int employee_id { get; set; }
        public string employee_name { get; set; }
        public int content_id { get; set; }
        public string content_name { get; set; }
        public int topic_id { get; set; }
        public string topic_name { get; set; }
        public DateTime expected_date { get; set; }
        public DateTime? actual_date { get; set; }
        public string status {  get; set; }
      
    }
}
