namespace ERP_Models
{
    public class ScheduleAttendanceModel
    {
        public int schedule_attendance_id {  get; set; }
        public int batch_schedule_id {  get; set; }
        public  DateTime attendance_date {  get; set; }
        public List<StudentAttendanceModel> students { get; set; }
    }

    public class StudentAttendanceModel
    {
        public int  registration_id { get; set; }
        public int is_present { get; set; }
    }
}