using ERP_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Interfaces
{
    public interface IExamService
    {
        Task SubmitExam(ExamModel exam);
        Task SubmitPracticeExam(ExamModel exam);
        Task<List<ExamModel>> GetAllExams();
        Task<List<ExamModel>> GetAllPracticeExams();
        Task<List<ExamModel>> GetStudentWiseExams(int student_id);
        Task<List<ExamModel>> GetStudentWisePracticeExams(int student_id);

        Task<List<ExamQuestionModel>> GetExamWiseQuestionResult(int exam_id);
        Task<List<ExamQuestionModel>> GetPracticeExamWiseQuestionResult(int exam_id);
        Task<ExamModel> GetExam(int exam_id);
        Task<ExamModel> GetPracticeExam(int exam_id);


        Task<int> ScheduleExamForStudent(ExamModel exam);
        Task SubmitScheduledExam(ExamModel exam);
        Task RejectScheduledExam(int exam_id);
        Task<List<ExamModel>> ViewAllScheduleExams();
        Task<List<ExamModel>> ViewExamWiseScheduleExams(int exam_id);
        Task<List<ExamModel>> ViewAllSubmittedExams();
        Task<List<ExamModel>> ViewAllRejectedExams();
        //Task  GetRegistrationAndBatchWiseScheduledExams(int batch_id);
        Task GenerateBatchExams(int batch_id,int total_questions);
        Task<List<BatchExamModel>> GetBatchWiseScheduledStudentExams(int batch_id);
        Task<List<BatchExamModel>> GetStudentWiseScheduledBatchExams(int registration_id);
        Task<BatchExamModel> GetBatchExamByExamId(int exam_id);
        Task SubmitBatchScheduledExam(BatchExamModel exam);
        Task<BatchExamModel> GetBatchExam(int exam_id);
        Task<List<ExamQuestionModel>> GetBatchExamWiseQuestionResult(int exam_id);
    }
}
