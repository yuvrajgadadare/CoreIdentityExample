
using ERP_Models;

namespace ERP_Services.Interfaces
{
    public interface IBatchService
    {
        Task AddBatch(BatchModel batch);
        Task AddBatchStudent(BatchStudentModel student);
        Task AddBatchSchedule(BatchScheduleModel schedule);
        Task DeleteBatch(int batch_id);
        Task RestoreBatch(int batch_id);
        Task DeleteBatchStudent(int student_id);
        Task< List<BatchModel>> GetAllBatches(int branch_id);
        Task< List<BatchModel>> GetAllDeletedBatches(int branch_id);
        Task<List<EmployeeModel>> GetAllTrainers(int branch_id);

        Task<List<BatchStudentModel>> GetAllBatchStudents(int branch_id);
        Task<List<BatchStudentModel>> GetStudentWiseBatches(int student_id);
        Task<BatchModel> GetRegistrationAndTopicWiseBatch(int registration_id, int topic_id);
        Task<List<BatchScheduleModel>> GetAllBatchSchedules(int branch_id);

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
