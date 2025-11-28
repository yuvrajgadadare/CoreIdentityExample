
using ERP_Models;

namespace ERP_Services.Interfaces
{
    public interface IBatchService
    {
        Task AddBatch(BatchModel batch);
        Task AddBatchStudent(BatchStudentModel student);
        Task AddBatchSchedule(BatchScheduleModel schedule);
        Task DeleteBatch(int batch_id);
        Task DeleteBatchStudent(int student_id);
        Task< List<BatchModel>> GetAllBatches();
        Task<List<EmployeeModel>> GetAllTrainers();

        Task<List<BatchStudentModel>> GetAllBatchStudents();
        Task<List<BatchStudentModel>> GetStudentWiseBatches(int student_id);
        Task<BatchModel> GetRegistrationAndTopicWiseBatch(int registration_id, int topic_id);
        Task<List<BatchScheduleModel>> GetAllBatchSchedules();

        Task<BatchModel> GetBatch(int id);
        Task<List<BatchStudentModel>> GetBatchWiseStudents(int batch_id);
        Task<List<TopicStudentModel>> GetTopicWiseStudents(int batch_id);
        Task<List<BatchScheduleModel>> GetBatchWiseSchedule(int batch_id);
        Task< List<BatchModel>> GetTrainerWiseBatches(int trainer_id);
        Task< BatchScheduleModel> GetScheduleWiseSchedule(int batch_schedule_id);
        Task<List<BatchScheduleExamModel>> GetBatchWiseScheduledExams(int batch_id);

        Task MarkStudentScheduleAttendance(ScheduleAttendanceModel s);
        Task<List<StudentMarkAttendance>> GetBatchWiseStudentAttendance(int batch_id, int registration_id);
    }
}
