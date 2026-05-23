using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly ICacheService _cacheService;
        public QuestionService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        public async Task AddInterviewQuestions(int content_id, List<InterviewQuestionModel> interviewQuestions)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_interview_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", content_id);
                DataTable dt = new DataTable();
                dt.Columns.Add("question",typeof(string));
                dt.Columns.Add("answer", typeof(string));
                foreach(var item in interviewQuestions)
                {
                    dt.Rows.Add(item.question,item.answer);
                }
                cmd.Parameters.AddWithValue("@questions", dt);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task AddProgramAnswer(ProgramAnswerModel answer)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_content_program_answers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@program_question_id", answer.program_question_id);
                cmd.Parameters.AddWithValue("@program_answer_id", answer.program_answer_id);
                cmd.Parameters.AddWithValue("@program_answer", answer.program_answer);
                cmd.Parameters.AddWithValue("@program_description", answer.program_description);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task AddProgramQuestion(ProgramQuestionModel program)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_content_program_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@program_question_id", program.program_question_id);
                cmd.Parameters.AddWithValue("@content_id", program.content_id);
                cmd.Parameters.AddWithValue("@question_title", program.question_title);
                cmd.Parameters.AddWithValue("@question_description", program.question_description);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task AddProgramQuestionWithAnswer(ProgramAnswerModel program)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_content_program_And_answers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                DataTable dtquestion = new DataTable();
                dtquestion.Columns.Add("content_id", typeof(int));
                dtquestion.Columns.Add("question_title", typeof(string));
                dtquestion.Columns.Add("question_description", typeof(string));
                dtquestion.Rows.Add(program.content_id, program.question_title,program.question_description);

                DataTable dtanswer = new DataTable();
                dtanswer.Columns.Add("program_answer", typeof(string));
                dtanswer.Columns.Add("program_description", typeof(string));
                dtanswer.Rows.Add(program.program_answer, program.program_description);

                cmd.Parameters.AddWithValue("@question", dtquestion);
                cmd.Parameters.AddWithValue("@answer", dtanswer);
                cmd.Parameters.AddWithValue("@program_question_id",0);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task DeleteProgramAnswer(int program_answer_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_content_program_answers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@program_question_id", 0);
                cmd.Parameters.AddWithValue("@program_answer_id", program_answer_id);
                cmd.Parameters.AddWithValue("@program_answer", "");
                cmd.Parameters.AddWithValue("@program_description", "");
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task DeleteProgramQuestion(int program_question_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_content_program_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@program_question_id", 0);
                cmd.Parameters.AddWithValue("@content_id", 0);
                cmd.Parameters.AddWithValue("@question_title", "");
                cmd.Parameters.AddWithValue("@question_description", "");
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task<List<InterviewQuestionModel>> GetAllInterviewQuestions()
        {
            var cacheData = await _cacheService.GetData<List<InterviewQuestionModel>>("InterviewQuestionModel");
            if (cacheData != null)
            {
                return cacheData;
            }
            List<InterviewQuestionModel> lst = new List<InterviewQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_interview_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
              
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    int  question_id = Convert.ToInt32(dr["question_id"].ToString());
                    string question = dr["question"].ToString();
                    string answer = dr["answer"].ToString();
                    InterviewQuestionModel e = new InterviewQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        question_id = question_id,
                        question = question,
                        answer = answer
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            cacheData = lst;
            _cacheService.SetData<IEnumerable<InterviewQuestionModel>>("InterviewQuestionModel", cacheData, expirationTime);
            return lst;
           
        }
        public async Task<List<ProgramAnswerModel>> GetAllProgramAnswers()
        {
            var cacheData = await _cacheService.GetData<List<ProgramAnswerModel>>("ProgramAnswerModel");
            if (cacheData != null)
            {
                return cacheData;
            }
            List<ProgramAnswerModel> lst = new List<ProgramAnswerModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_all_program_answers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@topic_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    int program_question_id = Convert.ToInt32(dr["program_question_id"].ToString());
                    string question_title = dr["question_title"].ToString();
                    string question_description = dr["question_description"].ToString();
                    int program_answer_id = Convert.ToInt32(dr["program_answer_id"].ToString());
                    string program_answer = dr["program_answer"].ToString();
                    string program_description = dr["program_description"].ToString();
                    ProgramAnswerModel e = new ProgramAnswerModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        program_question_id = program_question_id,
                        question_title = question_title,
                        question_description = question_description,
                        program_description = program_description,
                        program_answer = program_answer,
                        program_answer_id = program_answer_id
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            cacheData = lst;
            _cacheService.SetData<IEnumerable<ProgramAnswerModel>>("ProgramAnswerModel", cacheData, expirationTime);
            return lst;
        }
        public async Task<List<ProgramQuestionModel>> GetAllProgramQuestions()
        {
            var cacheData = await _cacheService.GetData<List<ProgramQuestionModel>>("ProgramQuestionModel");
            if (cacheData != null)
            {
                return cacheData;
            }
            List<ProgramQuestionModel> lst = new List<ProgramQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_all_program_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@topic_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    int program_question_id = Convert.ToInt32(dr["program_question_id"].ToString());
                    string question_title = dr["question_title"].ToString();
                    string question_description = dr["question_description"].ToString();
                    List<ProgramAnswerModel> answers =await GetQuestionWiseProgramAnswers(program_question_id);
                    ProgramQuestionModel e = new ProgramQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        program_question_id = program_question_id,
                        question_title = question_title,
                        question_description =  question_description,
                         answers = answers
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            cacheData = lst;
            _cacheService.SetData<IEnumerable<ProgramQuestionModel>>("ProgramQuestionModel", cacheData, expirationTime);
            return lst;
        }
        public async Task<List<InterviewQuestionModel>> GetContentWiseInterviewQuestions(int content_id)
        {

            List<InterviewQuestionModel> lst = new List<InterviewQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_content_wise_interview_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", content_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    string question = dr["question"].ToString();
                    string answer = dr["answer"].ToString();
                    InterviewQuestionModel e = new InterviewQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        question_id = question_id,
                        question = question,
                        answer = answer
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<ProgramAnswerModel>> GetContentWiseProgramAnswers(int content_id)
        {
            List<ProgramAnswerModel> lst = new List<ProgramAnswerModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_content_wise_program_answers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                 cmd.Parameters.AddWithValue("@content_id", content_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                   // int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    int program_question_id = Convert.ToInt32(dr["program_question_id"].ToString());
                    string question_title = dr["question_title"].ToString();
                    string question_description = dr["question_description"].ToString();
                    int program_answer_id = Convert.ToInt32(dr["program_answer_id"].ToString());
                    string program_answer = dr["program_answer"].ToString();
                    string program_description = dr["program_description"].ToString();
                    ProgramAnswerModel e = new ProgramAnswerModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        program_question_id = program_question_id,
                        question_title = question_title,
                        question_description = question_description,
                        program_description = program_description,
                        program_answer = program_answer,
                        program_answer_id = program_answer_id
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<ProgramQuestionModel>> GetContentWiseProgramQuestions(int content_id)
        {
            List<ProgramQuestionModel> lst = new List<ProgramQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_content_wise_program_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", content_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                   // int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    int program_question_id = Convert.ToInt32(dr["program_question_id"].ToString());
                    string question_title = dr["question_title"].ToString();
                    string question_description = dr["question_description"].ToString();
                    List<ProgramAnswerModel> answers = await GetQuestionWiseProgramAnswers(program_question_id);

                    ProgramQuestionModel e = new ProgramQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        program_question_id = program_question_id,
                        question_title = question_title,
                        question_description = question_description,
                        answers = answers
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<ProgramAnswerModel>> GetQuestionWiseProgramAnswers(int program_question_id)
        {
            List<ProgramAnswerModel> lst = new List<ProgramAnswerModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_program_wise_answers  ", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
              cmd.Parameters.AddWithValue("@program_question_id", program_question_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    string question_title = dr["question_title"].ToString();
                    string question_description = dr["question_description"].ToString();
                    int program_answer_id = Convert.ToInt32(dr["program_answer_id"].ToString());
                    string program_answer = dr["program_answer"].ToString();
                    string program_description = dr["program_description"].ToString();
                    ProgramAnswerModel e = new ProgramAnswerModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        program_question_id = program_question_id,
                        question_title = question_title,
                        question_description = question_description,
                        program_description = program_description,
                        program_answer = program_answer,
                        program_answer_id = program_answer_id
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<InterviewQuestionModel>> GetTopicWiseInterviewQuestions(int topic_id)
        {
            List<InterviewQuestionModel> lst = new List<InterviewQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topic_wise_interview_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    string question = dr["question"].ToString();
                    string answer = dr["answer"].ToString();
                    InterviewQuestionModel e = new InterviewQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        question_id = question_id,
                        question = question,
                        answer = answer
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<ProgramAnswerModel>> GetTopicWiseProgramAnswers(int topic_id)
        {
            List<ProgramAnswerModel> lst = new List<ProgramAnswerModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topic_wise_program_answers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                   // int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    int program_question_id = Convert.ToInt32(dr["program_question_id"].ToString());
                    string question_title = dr["question_title"].ToString();
                    string question_description = dr["question_description"].ToString();
                    int program_answer_id = Convert.ToInt32(dr["program_answer_id"].ToString());
                    string program_answer = dr["program_answer"].ToString();
                    string program_description = dr["program_description"].ToString();
                    ProgramAnswerModel e = new ProgramAnswerModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        program_question_id = program_question_id,
                        question_title = question_title,
                        question_description = question_description,
                        program_description = program_description,
                        program_answer = program_answer,
                        program_answer_id = program_answer_id
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<ProgramQuestionModel>> GetTopicWiseProgramQuestions(int topic_id)
        {
            List<ProgramQuestionModel> lst = new List<ProgramQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topic_wise_program_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                   // int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    int program_question_id = Convert.ToInt32(dr["program_question_id"].ToString());
                    string question_title = dr["question_title"].ToString();
                    string question_description = dr["question_description"].ToString();
                    List<ProgramAnswerModel> answers = await GetQuestionWiseProgramAnswers(program_question_id);

                    ProgramQuestionModel e = new ProgramQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        program_question_id = program_question_id,
                        question_title = question_title,
                        question_description = question_description,
                         answers = answers
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task RestoreProgramAnswer(int program_answer_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_content_program_answers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@program_question_id", 0);
                cmd.Parameters.AddWithValue("@program_answer_id", program_answer_id);
                cmd.Parameters.AddWithValue("@program_answer", "");
                cmd.Parameters.AddWithValue("@program_description", "");
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task RestoreProgramQuestion(int program_question_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_content_program_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Restore");
                cmd.Parameters.AddWithValue("@program_question_id", 0);
                cmd.Parameters.AddWithValue("@content_id", 0);
                cmd.Parameters.AddWithValue("@question_title", "");
                cmd.Parameters.AddWithValue("@question_description", "");
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task UpdateProgramAnswer(ProgramAnswerModel answer)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_content_program_answers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@program_question_id", answer.program_question_id);
                cmd.Parameters.AddWithValue("@program_answer_id", answer.program_answer_id);
                cmd.Parameters.AddWithValue("@program_answer", answer.program_answer);
                cmd.Parameters.AddWithValue("@program_description", answer.program_description);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task UpdateProgramQuestion(ProgramQuestionModel program)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_content_program_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@program_question_id", program.program_question_id);
                cmd.Parameters.AddWithValue("@content_id", program.content_id);
                cmd.Parameters.AddWithValue("@question_title", program.question_title);
                cmd.Parameters.AddWithValue("@question_description", program.question_description);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}