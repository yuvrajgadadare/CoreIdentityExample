using ERP_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Interfaces
{
    public interface IQuestionService
    {
        Task AddInterviewQuestions(int content_id, List<InterviewQuestionModel> interviewQuestions);
        Task<List<InterviewQuestionModel>> GetAllInterviewQuestions();
        Task<List<InterviewQuestionModel>> GetTopicWiseInterviewQuestions(int topic_id);
        Task<List<InterviewQuestionModel>> GetContentWiseInterviewQuestions(int content_id);
        Task AddProgramQuestion(ProgramQuestionModel program);
        Task UpdateProgramQuestion(ProgramQuestionModel program);
        Task DeleteProgramQuestion(int program_question_id);
        Task RestoreProgramQuestion(int program_question_id);
        Task<List<ProgramQuestionModel>> GetAllProgramQuestions();
        Task< List<ProgramQuestionModel>> GetTopicWiseProgramQuestions(int topic_id);
        Task<List<ProgramQuestionModel>> GetContentWiseProgramQuestions(int content_id);
        Task AddProgramAnswer(ProgramAnswerModel answer);
        Task AddProgramQuestionWithAnswer(ProgramAnswerModel program);
        Task UpdateProgramAnswer(ProgramAnswerModel answer);
        Task DeleteProgramAnswer(int program_answer_id);
        Task RestoreProgramAnswer(int program_id);
        Task<List<ProgramAnswerModel>> GetAllProgramAnswers();
        Task<List<ProgramAnswerModel>> GetTopicWiseProgramAnswers(int topic_id);
        Task<List<ProgramAnswerModel>> GetQuestionWiseProgramAnswers(int program_question_id);
        Task<List<ProgramAnswerModel>> GetContentWiseProgramAnswers(int content_id);

    }
}
