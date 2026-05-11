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
    public class ExamService : IExamService
    {
        IBatchService batchService;
        IContentService contentService;
        IStudentService studentService;
        public ExamService( IBatchService batchService, IContentService contentService,IStudentService studentService)
        {
            this.batchService = batchService;
            this.contentService = contentService;
            this.studentService = studentService;
        }
        public async Task<ExamModel> GetExam(int exam_id)
        {
            ExamModel st = null;
            List<ExamQuestionModel> questions = await GetExamWiseQuestionResult(exam_id);
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam_id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["student_name"].ToString();
                    string mobile_number = dr["student_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();

                    int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = (total_correct_questions * 100 / total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";

                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        end_time = end_time ,
                        end_time_string = end_time.ToLongTimeString(),
                        exam_date = exam_date,
                        exam_id = eid,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time ,
                        start_time_string = start_time.ToLongTimeString(),
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                         branch_id = branch_id,
                          branch_name=branch_name
                         
                    };

                }
                con.Close();
            }
            return st;
        }

        public async Task<List<ExamQuestionModel>> GetExamWiseQuestionResult(int exam_id)
        {
            List<ExamQuestionModel> lst = new List<ExamQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_exam_wise_result", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    int exam_question_id = Convert.ToInt32(dr["exam_question_id"].ToString());
                    string question = dr["question"].ToString();
                    string option1 = dr["option1"].ToString();
                    string option2 = dr["option2"].ToString();
                    string option3 = dr["option3"].ToString();
                    string option4 = dr["option4"].ToString();
                    int correct_option_number = Convert.ToInt32(dr["correct_option_number"].ToString());
                    int submitted_option_number = Convert.ToInt32(dr["submitted_option_number"].ToString());
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    ExamQuestionModel e = new ExamQuestionModel()
                    {
                        correct_option_number = correct_option_number,
                        exam_id = eid,
                        exam_question_id = exam_question_id,
                        option1 = option1,
                        option2 = option2,
                        option3 = option3,
                        option4 = option4,
                        question = question,
                        question_id = question_id,
                        submitted_option_number = submitted_option_number,
                          branch_name=branch_name,
                           branch_id=branch_id

                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task SubmitExam(ExamModel exam)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                //DataTable examtable = new DataTable();
                //examtable.Columns.Add("student_id", typeof(int));
                //examtable.Columns.Add("topic_id", typeof(int));
                //examtable.Columns.Add("exam_date", typeof(DateTime));
                //examtable.Columns.Add("start_time", typeof(DateTime));
                //examtable.Columns.Add("end_time", typeof(DateTime));
                //examtable.Rows.Add(exam.student_id, exam.topic_id, exam.exam_date, exam.start_time, exam.end_time);
                DataTable questionstable = new DataTable();
                questionstable.Columns.Add("question_id", typeof(int));
                questionstable.Columns.Add("submitted_option_number", typeof(int));
                foreach (ExamQuestionModel e in exam.examQuestions)
                {
                    questionstable.Rows.Add(e.question_id, e.submitted_option_number);
                }
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_student_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@exam", examtable);
                cmd.Parameters.AddWithValue("@exam_id", exam.exam_id);
                cmd.Parameters.AddWithValue("@end_time", exam.end_time);
                cmd.Parameters.AddWithValue("@question", questionstable);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }



        public async Task SubmitBatchScheduledExam(BatchExamModel exam)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                //DataTable examtable = new DataTable();
                //examtable.Columns.Add("student_id", typeof(int));
                //examtable.Columns.Add("topic_id", typeof(int));
                //examtable.Columns.Add("exam_date", typeof(DateTime));
                //examtable.Columns.Add("start_time", typeof(DateTime));
                //examtable.Columns.Add("end_time", typeof(DateTime));
                //examtable.Rows.Add(exam.student_id, exam.topic_id, exam.exam_date, exam.start_time, exam.end_time);
                DataTable questionstable = new DataTable();
                questionstable.Columns.Add("question_id", typeof(int));
                questionstable.Columns.Add("submitted_option_number", typeof(int));
                foreach (ExamQuestionModel e in exam.examQuestions)
                {
                    questionstable.Rows.Add(e.question_id, e.submitted_option_number);
                }
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_submit_student_batch_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@exam", examtable);
                cmd.Parameters.AddWithValue("@exam_id", exam.exam_id);
                cmd.Parameters.AddWithValue("@start_time", exam.start_time);
                cmd.Parameters.AddWithValue("@end_time", exam.end_time);
                cmd.Parameters.AddWithValue("@question", questionstable);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }


        public async Task<int> ScheduleExamForStudent(ExamModel exam)
        {
            int exam_id = 0;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_schedule_student_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.Add("@exam_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@student_id", exam.student_id);
                cmd.Parameters.AddWithValue("@topic_id", exam.topic_id);
                cmd.Parameters.AddWithValue("@total_questions", exam.total_questions);
                cmd.Parameters.AddWithValue("@exam_date", exam.exam_date);
                cmd.Parameters.AddWithValue("@start_time", exam.start_time);
                cmd.Parameters.AddWithValue("@end_time", exam.end_time);
                //cmd.Parameters.AddWithValue("@exam_id", exam_id);
                object st = cmd.ExecuteScalar();
                exam_id = Convert.ToInt32(st);
                con.Close();
            }
            return exam_id;
        }

        public async Task SubmitScheduledExam(ExamModel exam)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_submit_scheduled_student_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam.exam_id);
                cmd.Parameters.AddWithValue("@end_time", exam.end_time);
                cmd.Parameters.AddWithValue("@exam_date", exam.exam_date);


                DataTable questionstable = new DataTable();
                questionstable.Columns.Add("question_id", typeof(int));
                questionstable.Columns.Add("submitted_option_number", typeof(int));
                foreach (ExamQuestionModel e in exam.examQuestions)
                {
                    questionstable.Rows.Add(e.question_id, e.submitted_option_number);
                }
                cmd.Parameters.AddWithValue("@question", questionstable);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task RejectScheduledExam(int exam_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_reject_scheduled_student_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam_id);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<ExamModel>> ViewAllScheduleExams(int branch_id)
        {
            List<ExamModel> lst = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_scheduled_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    //DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string status = dr["status"].ToString();
                  //  int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    ExamModel e = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        exam_date = exam_date,
                        exam_id = eid,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        start_time_string = start_time.ToLongTimeString(),
                        end_time = end_time ,
                        end_time_string = end_time.ToLongTimeString(),
                        student_name = student_name,
                        topic_name = topic_name,
                        status = status,
                        total_questions = total_questions,
                         branch_id = branch_id,
                          branch_name=branch_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<ExamModel>> ViewAllSubmittedExams(int branch_id)
        {
            List<ExamModel> lst = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_Submitted_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    string topic_name = dr["topic_name"].ToString();
                    string status = dr["status"].ToString();
                    ExamModel e = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        exam_date = exam_date,
                        exam_id = eid,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        start_time_string = start_time.ToLongTimeString(),
                     
                        student_name = student_name,
                        topic_name = topic_name,
                        status = status,
                        total_questions = total_questions,
                         branch_name=branch_name,
                          branch_id = branch_id
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<ExamModel>> ViewAllRejectedExams(int branch_id)
        {
            List<ExamModel> lst = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_Rejected_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    //DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    string topic_name = dr["topic_name"].ToString();
                    string status = dr["status"].ToString();
                    ExamModel e = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        exam_date = exam_date,
                        exam_id = eid,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        start_time_string = start_time.ToLongTimeString(),
                        
                        student_name = student_name,
                        topic_name = topic_name,
                        status = status,
                        total_questions = total_questions
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task SubmitPracticeExam(ExamModel exam)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                DataTable examtable = new DataTable();
                examtable.Columns.Add("student_id", typeof(int));
                examtable.Columns.Add("topic_id", typeof(int));
                examtable.Columns.Add("exam_date", typeof(DateTime));
                examtable.Columns.Add("start_time", typeof(DateTime));
                examtable.Columns.Add("end_time", typeof(DateTime));
                examtable.Rows.Add(exam.student_id, exam.topic_id, exam.exam_date, exam.start_time, exam.end_time);
                DataTable questionstable = new DataTable();
                questionstable.Columns.Add("question_id", typeof(int));
                questionstable.Columns.Add("submitted_option_number", typeof(int));
                foreach (ExamQuestionModel e in exam.examQuestions)
                {
                    questionstable.Rows.Add(e.question_id, e.submitted_option_number);
                }
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_student_practice_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam", examtable);
                cmd.Parameters.AddWithValue("@exam_id", exam.exam_id);
                cmd.Parameters.AddWithValue("@question", questionstable);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<ExamModel>> GetAllPracticeExams(int branch_id)
        {
            List<ExamModel> lst = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_practice_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    int student_id = Convert.ToInt32(dr["student_id"].ToString());

                    int exam_id = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string status = dr["status"].ToString();

                    string topic_name = dr["topic_name"].ToString();
                    string branch_name = dr["branch_name"].ToString();

                    ExamModel st = null;
                    List<ExamQuestionModel> questions = await GetPracticeExamWiseQuestionResult(exam_id);

                    //  int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = (total_correct_questions * 100 / total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";

                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                      
                        exam_date = exam_date,
                        exam_id = exam_id,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        start_time_string = start_time.ToLongTimeString(),
                        end_time = end_time,
                        end_time_string = end_time.ToLongTimeString(),
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                        status = status,
                         branch_id = branch_id,
                          branch_name= branch_name

                    };
                    lst.Add(st);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<ExamModel>> GetStudentWisePracticeExams(int student_id)
        {
            List<ExamModel> lst = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_student_practice_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int exam_id = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["student_name"].ToString();
                    string mobile_number = dr["student_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string status = dr["status"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    string topic_name = dr["topic_name"].ToString();

                    ExamModel st = null;
                    List<ExamQuestionModel> questions = await GetPracticeExamWiseQuestionResult(exam_id);

                    //  int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = (total_correct_questions * 100 / total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";

                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                         
                        exam_date = exam_date,
                        exam_id = exam_id,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        start_time_string = start_time.ToLongTimeString(),
                        end_time = end_time,
                        end_time_string = end_time.ToLongTimeString(),
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                        status = status,
                         branch_name=branch_name,
                          branch_id = branch_id

                    };
                    lst.Add(st);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<ExamQuestionModel>> GetPracticeExamWiseQuestionResult(int exam_id)
        {
            List<ExamQuestionModel> lst = new List<ExamQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_practice_exams_wise_result", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    int exam_question_id = Convert.ToInt32(dr["exam_question_id"].ToString());
                    string question = dr["question"].ToString();
                    string option1 = dr["option1"].ToString();
                    string option2 = dr["option2"].ToString();
                    string option3 = dr["option3"].ToString();
                    string option4 = dr["option4"].ToString();
                    int correct_option_number = Convert.ToInt32(dr["correct_option_number"].ToString());
                    int submitted_option_number = Convert.ToInt32(dr["submitted_option_number"].ToString());

                    ExamQuestionModel e = new ExamQuestionModel()
                    {
                        correct_option_number = correct_option_number,
                        exam_id = eid,
                        exam_question_id = exam_question_id,
                        option1 = option1,
                        option2 = option2,
                        option3 = option3,
                        option4 = option4,
                        question = question,
                        question_id = question_id,
                        submitted_option_number = submitted_option_number,

                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<ExamModel> GetPracticeExam(int exam_id)
        {
            ExamModel st = null;

            List<ExamQuestionModel> questions = await GetPracticeExamWiseQuestionResult(exam_id);
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_practice_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam_id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["student_name"].ToString();
                    string mobile_number = dr["student_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();


                    int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = (total_correct_questions * 100 / total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";

                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                         
                        exam_date = exam_date,
                        exam_id = eid,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        start_time_string = start_time.ToLongTimeString(),
                        end_time = end_time,
                        end_time_string = end_time.ToLongTimeString(),
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                         branch_id = branch_id,
                          branch_name=branch_name
                    };

                }
                con.Close();
            }
            return st;
        }


        public async Task<List<ExamModel>> ViewExamWiseScheduleExams(int exam_id)
        {
           ExamModel e=await GetExam(exam_id);
            List<ExamModel> lst = await ViewAllScheduleExams(e.branch_id);

            return lst.Where(e => e.exam_id.Equals(exam_id)).ToList();
        }
        public async Task<List<ExamModel>> GetStudentWiseExams(int student_id)
        {
            List<ExamModel> exams = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_student_wise_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int exam_id = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["student_name"].ToString();
                    string mobile_number = dr["student_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string status = dr["status"].ToString();

                    string topic_name = dr["topic_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
            
                    ExamModel st = null;
                    
                    List<ExamQuestionModel> questions = await GetExamWiseQuestionResult(exam_id);

                    //  int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = ((float)total_correct_questions * 100 / (float)total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";
                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        exam_date = exam_date,
                        exam_id = exam_id,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        start_time_string = start_time.ToLongTimeString(),
                        end_time = end_time,
                        end_time_string = end_time.ToLongTimeString(),
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                        status = status,
                        branch_id = branch_id,
                        branch_name=branch_name
                    };
                    exams.Add(st);
                }
                con.Close();
            }

            List<ExamModel> lst = new List<ExamModel>();
            foreach (ExamModel e in exams)
            {
                if (lst.Count > 0)
                {
                    ExamModel s = lst.FirstOrDefault(p => p.topic_name.Equals(e.topic_name));
                    if (s != null)
                    {
                        if (s.total_correct_questions < e.total_correct_questions)
                        {
                            int p = lst.IndexOf(s);
                            lst[p] = e;

                        }
                    }
                    else
                    {
                        lst.Add(e);

                    }
                }
                else
                {
                    lst.Add(e);
                }
            }
                return lst;

        }
        public async Task<List<ExamModel>> GetStudentRegistrationWiseSubmittedExams(int registration_id)
        {
            List<ExamModel> exams = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_student_registration_wise_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int exam_id = Convert.ToInt32(dr["exam_id"].ToString());
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["student_name"].ToString();
                    string mobile_number = dr["student_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string status = dr["status"].ToString();

                    string topic_name = dr["topic_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();

                    ExamModel st = null;

                    List<ExamQuestionModel> questions = await GetExamWiseQuestionResult(exam_id);

                    //  int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = ((float)total_correct_questions * 100 / (float)total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";
                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        exam_date = exam_date,
                        exam_id = exam_id,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        start_time_string = start_time.ToLongTimeString(),
                        end_time = end_time,
                        end_time_string = end_time.ToLongTimeString(),
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                        status = status,
                        branch_id = branch_id,
                        branch_name = branch_name
                    };
                    exams.Add(st);
                }
                con.Close();
            }

            List<ExamModel> lst = new List<ExamModel>();
            foreach (ExamModel e in exams)
            {
                if (lst.Count > 0)
                {
                    ExamModel s = lst.FirstOrDefault(p => p.topic_name.Equals(e.topic_name));
                    if (s != null)
                    {
                        if (s.total_correct_questions < e.total_correct_questions)
                        {
                            int p = lst.IndexOf(s);
                            lst[p] = e;

                        }
                    }
                    else
                    {
                        lst.Add(e);

                    }
                }
                else
                {
                    lst.Add(e);
                }
            }
            return lst;

        }
        public async Task<List<ExamModel>> GetAllExams(int branch_id)
        {
            List<ExamModel> lst = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_Submitted_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    //int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    //string student_name = dr["student_name"].ToString();
                    //string email_address = dr["email_address"].ToString();
                    //string mobile_number = dr["mobile_number"].ToString();
                    //DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    //DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    //DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    //int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    //int total_Questions = Convert.ToInt32(dr["total_Questions"].ToString());
                    //string topic_name = dr["topic_name"].ToString();
                    //string status = dr["status"].ToString();

                    //ExamModel e = new ExamModel()
                    //{
                    //    student_id = student_id,
                    //    email_address = email_address,
                    //    end_time = end_time,
                    //    exam_date = exam_date,
                    //    exam_id = eid,
                    //    mobile_number = mobile_number,
                    //    topic_id = topic_id,
                    //    start_time = start_time,
                    //    student_name = student_name,
                    //    topic_name = topic_name,
                    //     total_questions=total_Questions,
                    //    status= status
                    //};
                    //lst.Add(e);
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());

                    int exam_id = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string status = dr["status"].ToString();
                   // int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    string topic_name = dr["topic_name"].ToString();

                    ExamModel st = null;
                    List<ExamQuestionModel> questions = await GetExamWiseQuestionResult(exam_id);

                    //  int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = (total_correct_questions * 100 / total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";

                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        
                        exam_date = exam_date,
                        exam_id = exam_id,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        start_time_string = start_time.ToLongTimeString(),
                        end_time = end_time,
                        end_time_string = end_time.ToLongTimeString(),
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                        status = status,
                         branch_id = branch_id,
                         branch_name=branch_name
                    };
                    lst.Add(st);
                }
                con.Close();
            }
            return lst;
        }
        //public async Task  GetRegistrationAndBatchWiseScheduledExams(int batch_id, int registration_id)
        //{
        //    RegistrationModel r=await studentService.GetRegistration(registration_id);
        //        BatchModel b=await batchService.GetBatch(batch_id);
        //        if (b != null)
        //        {
        //            DateTime next_date = b.start_date;
        //            int total_leactures = b.total_leactures;
        //            int total_exams = total_leactures / 5;
        //            for (int i = 1; i <= total_exams; i++)
        //            {
        //                next_date = next_date.AddDays(5);
        //                if (next_date.DayOfWeek.ToString().ToLower().Equals("saturday"))
        //                {
        //                    next_date=next_date.AddDays(2);
        //                }
        //                else if (next_date.DayOfWeek.ToString().ToLower().Equals("sunday"))
        //                {
        //                    next_date = next_date.AddDays(1);
        //                }
        //            List<ContentModel> contents =await contentService.GetTopicWiseContents(b.topic_id);

        //            ExamModel exam = new ExamModel()
        //            {
        //                registration_id = r.registration_id,
        //                exam_date = next_date,
        //                topic_id = b.topic_id,
        //                topic_name = b.topic_name,
        //                start_time = Convert.ToDateTime(next_date.ToShortDateString() + " " + "2:00 PM"),
        //                status = "Not Attended",
        //                is_attended=0
        //            };
        //           await GenerateBatchExams(exam, batch_id);

        //            }
        //        }

        //}
        public async Task<List<ExamQuestionModel>> GetContentWiseQuestions(List<ContentModel> contents)
        {
            List<ExamQuestionModel> lst = new List<ExamQuestionModel>();
            foreach (ContentModel c in contents)
            {
                List<ContentQuestionModel> questions = await contentService.GetContentWiseQuestions(c.content_id);
                if (questions.Count > 0)
                {
                    Random r = new Random();
                    for (int i = 1; i <= 5; i++)
                    {
                        ContentQuestionModel q = questions[r.Next(0, questions.Count - 1)];
                        int p = r.Next(0, questions.Count() - 1);
                        lst.Add(new ExamQuestionModel()
                        {
                            question_id = q.question_id,
                            question = q.question,
                            option1 = q.option1,
                            option2 = q.option2,
                            option3 = q.option3,
                            option4 = q.option4,
                            correct_option_number = q.correct_option_number

                        });
                    }
                }
            }
            return lst;
        }
        public async Task GenerateBatchExams(int batch_id, int total_questions)
        {

            BatchModel b = await batchService.GetBatch(batch_id);
            List<ExamModel> exams = new List<ExamModel>();
            DateTime next_date = b.start_date;
            int total_leactures = b.total_leactures;
            int total_exams = total_leactures / 5;
            List<DateTime> dates = new List<DateTime>();
            for (int i = 1; i <= total_exams; i++)
            {
                next_date = next_date.AddDays(5);
                if (next_date.DayOfWeek.ToString().ToLower().Equals("saturday"))
                {
                    next_date = next_date.AddDays(2);
                }
                else if (next_date.DayOfWeek.ToString().ToLower().Equals("sunday"))
                {
                    next_date = next_date.AddDays(1);
                }
                dates.Add(next_date);

            }
            foreach (BatchStudentModel bs in await batchService.GetBatchWiseStudents(batch_id))
            {
               
                List<ContentModel> contents = await contentService.GetTopicWiseContents(b.topic_id);
                foreach (DateTime dt in dates)
                {
                    ExamModel exam = new ExamModel()
                    {
                        registration_id = bs.registration_id,
                        exam_date = dt,
                        topic_id = b.topic_id,

                        //topic_name = b.topic_name,
                        start_time = Convert.ToDateTime(dt.ToShortDateString() + " " + "9:00 AM"),
                        end_time = Convert.ToDateTime(dt.ToShortDateString() + " " + "6:00 PM"),
                        status = "Not Attended",
                        is_attended = 0,
                        total_questions = total_questions
                    };
                    exams.Add(exam);
                }
            }
            try
            {
                foreach (ExamModel em in exams)
                {
                    using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
                    {
                        //DataTable examtable = new DataTable();
                        //examtable.Columns.Add("registration_id", typeof(int));
                        //examtable.Columns.Add("topic_id", typeof(int));
                        //examtable.Columns.Add("batch_id", typeof(int));
                        //examtable.Columns.Add("exam_date", typeof(DateTime));
                        //examtable.Columns.Add("start_time", typeof(DateTime));
                        //examtable.Columns.Add("end_time", typeof(DateTime));
                        //examtable.Columns.Add("total_questions", typeof(int));

                        //examtable.Columns.Add("is_attended", typeof(int));

                        //foreach (ExamModel exam in exams)
                        //{
                        //    examtable.Rows.Add(exam.registration_id, exam.topic_id, batch_id, exam.total_questions, exam.exam_date, exam.start_time, exam.end_time, exam.is_attended);
                        //}
                        //DataTable questionstable = new DataTable();
                        //questionstable.Columns.Add("question_id", typeof(int));
                        //questionstable.Columns.Add("submitted_option_number", typeof(int));
                        //foreach (ExamQuestionModel e in exam.examQuestions)
                        //{
                        //questionstable.Rows.Add(e.question_id, e.submitted_option_number);
                        //}

                        con.Open();
                        SqlCommand cmd = new SqlCommand("sp_student_batch_exam", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@exam", examtable);

                        cmd.Parameters.AddWithValue("@registration_id", em.registration_id);
                        cmd.Parameters.AddWithValue("@topic_id", em.topic_id);
                        cmd.Parameters.AddWithValue("@batch_id", batch_id);
                        cmd.Parameters.AddWithValue("@total_questions", em.total_questions);
                        cmd.Parameters.AddWithValue("@is_attended", em.is_attended);
                        cmd.Parameters.AddWithValue("@exam_date", em.exam_date);
                        cmd.Parameters.AddWithValue("@start_time", em.start_time);
                        cmd.Parameters.AddWithValue("@end_time", em.end_time);
                        //cmd.Parameters.AddWithValue("@exam_id", exam.exam_id);
                        //cmd.Parameters.AddWithValue("@question", questionstable);
                        int cnt = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        public async Task<List<BatchExamModel>> GetStudentWiseScheduledBatchExams(int registration_id )
        {
            List<BatchExamModel> lst = new List<BatchExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_student_wise_batch_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int batch_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int total_Questions = Convert.ToInt32(dr["total_Questions"].ToString());
                    int is_attended = Convert.ToInt32(dr["is_attended"].ToString());


                    BatchExamModel e = new BatchExamModel()
                    {
                        student_id = student_id,
                        end_time = end_time,
                        exam_date = exam_date,
                        exam_id = eid,
                        topic_id = topic_id,
                        start_time = start_time,
                        student_name = student_name,
                        topic_name = topic_name,
                        total_questions = total_Questions,
                         is_attended = is_attended,
                          registration_id = registration_id,
                           batch_id = batch_id
                    };
                    lst.Add(e);
                }  
                 
            }
            return lst;
        }

        public async Task<List<BatchExamModel>> GetBatchWiseScheduledStudentExams(int batch_id)
        {
            List<BatchExamModel> lst = new List<BatchExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_batch_wise_student_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int total_Questions = Convert.ToInt32(dr["total_Questions"].ToString());
                    int is_attended = Convert.ToInt32(dr["is_attended"].ToString());
                    BatchExamModel e = new BatchExamModel()
                    {
                        student_id = student_id,
                        end_time = end_time,
                        exam_date = exam_date,
                        exam_id = eid,
                        topic_id = topic_id,
                        start_time = start_time,
                        student_name = student_name,
                        topic_name = topic_name,
                        total_questions = total_Questions,
                        is_attended = is_attended,
                        registration_id = registration_id,
                        batch_id = batch_id
                    };
                    lst.Add(e);
                }

            }
            return lst;
        }

        public async Task<BatchExamModel>  GetBatchExamByExamId(int exam_id)
        {
           BatchExamModel t = new  BatchExamModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_batch_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam_id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int batch_id = Convert.ToInt32(dr["batch_id"].ToString());
                    string batch_name = dr["batch_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int total_Questions = Convert.ToInt32(dr["total_Questions"].ToString());
                    int is_attended = Convert.ToInt32(dr["is_attended"].ToString());


                    t = new BatchExamModel()
                    {
                        student_id = student_id,
                        end_time = end_time,
                        exam_date = exam_date,
                        exam_id = eid,
                        topic_id = topic_id,
                        start_time = start_time,
                        student_name = student_name,
                        topic_name = topic_name,
                        total_questions = total_Questions,
                        is_attended = is_attended,
                        registration_id = registration_id,
                        batch_id = batch_id
                    };
                     
                }

            }
            return t;
        }

        public async Task<BatchExamModel> GetBatchExam(int exam_id)
        {
            BatchExamModel st = null;

            List<ExamQuestionModel> questions = await GetBatchExamWiseQuestionResult(exam_id);
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_batch_exam", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam_id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int batch_id = Convert.ToInt32(dr["batch_id"].ToString());
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string batch_name = dr["batch_name"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();

                    int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = (total_correct_questions * 100 / total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";

                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new BatchExamModel()
                    {
                        student_id = student_id,
                        end_time = end_time,
                        exam_date = exam_date,
                        exam_id = eid,
                        topic_id = topic_id,
                        start_time = start_time,
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                         branch_id = branch_id,
                          branch_name=branch_name
                         
                    };

                }
                con.Close();
            }
            return st;
        }
        public async Task<List<ExamQuestionModel>> GetBatchExamWiseQuestionResult(int exam_id)
        {
            List<ExamQuestionModel> lst = new List<ExamQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_batch_exam_wise_result", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", exam_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int eid = Convert.ToInt32(dr["exam_id"].ToString());
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    int exam_question_id = Convert.ToInt32(dr["exam_question_id"].ToString());
                    string question = dr["question"].ToString();
                    string option1 = dr["option1"].ToString();
                    string option2 = dr["option2"].ToString();
                    string option3 = dr["option3"].ToString();
                    string option4 = dr["option4"].ToString();
                    int correct_option_number = Convert.ToInt32(dr["correct_option_number"].ToString());
                    int submitted_option_number = Convert.ToInt32(dr["submitted_option_number"].ToString());

                    ExamQuestionModel e = new ExamQuestionModel()
                    {
                        correct_option_number = correct_option_number,
                        exam_id = eid,
                        exam_question_id = exam_question_id,
                        option1 = option1,
                        option2 = option2,
                        option3 = option3,
                        option4 = option4,
                        question = question,
                        question_id = question_id,
                        submitted_option_number = submitted_option_number,

                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async  Task<StudentCertificationModel> GetStudentCertificate(int registration_id)
        {
            List<ExamModel> exams = new List<ExamModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_student_registration_wise_exams", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int exam_id = Convert.ToInt32(dr["exam_id"].ToString());
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    DateTime exam_date = Convert.ToDateTime(dr["exam_date"].ToString());
                    DateTime start_time = Convert.ToDateTime(dr["start_time"].ToString());
                    DateTime end_time = Convert.ToDateTime(dr["end_time"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int total_questions = Convert.ToInt32(dr["total_questions"].ToString());
                    string status = dr["status"].ToString();

                    string topic_name = dr["topic_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();

                    ExamModel st = null;

                    List<ExamQuestionModel> questions = await GetExamWiseQuestionResult(exam_id);

                    //  int total_questions = questions.Count;
                    int total_correct_questions = 0;
                    int total_wrong_questions = 0;
                    foreach (var q in questions)
                    {
                        if (q.submitted_option_number == q.correct_option_number)
                        {
                            total_correct_questions++;
                        }
                        else
                        {
                            total_wrong_questions++;
                        }
                    }
                    float percentage = ((float)total_correct_questions * 100 / (float)total_questions);
                    string grade = "";
                    if (percentage < 40)
                    {
                        grade = "Poor";
                    }
                    else if (percentage >= 40 && percentage < 60)
                    {
                        grade = "Average";
                    }
                    else if (percentage >= 60 && percentage < 80)
                    {
                        grade = "Good";
                    }
                    else
                    {
                        grade = "Excellent";
                    }
                    st = new ExamModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        exam_date = exam_date,
                        exam_id = exam_id,
                        mobile_number = mobile_number,
                        topic_id = topic_id,
                        start_time = start_time,
                        start_time_string = start_time.ToLongTimeString(),
                        end_time = end_time,
                        end_time_string = end_time.ToLongTimeString(),
                        student_name = student_name,
                        topic_name = topic_name,
                        examQuestions = questions,
                        total_questions = total_questions,
                        total_correct_questions = total_correct_questions,
                        total_wrong_questions = total_wrong_questions,
                        percentage = percentage,
                        grade = grade,
                        status = status,
                        branch_id = branch_id,
                        branch_name = branch_name
                    };
                    exams.Add(st);
                }
                con.Close();
            }

            List<ExamModel> lst = new List<ExamModel>();
            foreach (ExamModel e in exams)
            {
                if (lst.Count > 0)
                {
                    ExamModel s = lst.FirstOrDefault(p => p.topic_name.Equals(e.topic_name));
                    if (s != null)
                    {
                        if (s.total_correct_questions < e.total_correct_questions)
                        {
                            int p = lst.IndexOf(s);
                            lst[p] = e;

                        }
                    }
                    else
                    {
                        lst.Add(e);

                    }
                }
                else
                {
                    lst.Add(e);
                }
            }
            float average_percentage = lst.Average(e => e.percentage);
            string grade2 = "";
            if (average_percentage < 40)
            {
                grade2 = "Poor";
            }
            else if (average_percentage >= 40 && average_percentage < 60)
            {
                grade2 = "Average";
            }
            else if (average_percentage >= 60 && average_percentage < 80)
            {
                grade2 = "Good";
            }
            else
            {
                grade2 = "Excellent";
            }
            RegistrationModel r =await studentService.GetRegistration(registration_id);

            StudentCertificationModel sc = new StudentCertificationModel()
            {
                registration_id = r.registration_id,
                permanent_identification_number = r.permanent_identification_number,
                average_percentage = average_percentage,
                branch_id = r.branch_id,
                branch_name = r.branch_name,
                CertificationCode = "",
                course_name = r.course_name,
                grade = grade2,
                status = "",
                student_id = r.student_id,
                student_name = r.student_name,
                total_exams = lst.Count()
            };
            return sc;
        }
    }
}
