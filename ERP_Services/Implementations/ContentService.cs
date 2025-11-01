using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Implementations
{
    public class ContentService:IContentService
    {
        ITopicService topicService;
        ICourseService courseService;
        public ContentService(ITopicService topicService, ICourseService courseService)
        {
            this.topicService = topicService;
            this.courseService = courseService;
        }

        public async Task<List<ContentModel>> GetAllTopicWiseContentQuestionAndInterviewQuestionsCounts(int topic_id)
        {
            List<ContentModel> lst = new List<ContentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topic_wise_content_wise_program_and_interview_question_count", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int t_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string content_name = dr["content_name"].ToString();
                    int total_questions = Convert.ToInt32(dr["total_program_questions"].ToString());
                    int total_interview_questions = Convert.ToInt32(dr["total_interview_questions"].ToString());

                    ContentModel e = new ContentModel() { topic_id = t_id, topic_name = topic_name, content_id = content_id, content_name = content_name, total_program_questions = total_questions, total_interview_questions = total_interview_questions };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task AddContentVideo(VideoModel topic)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_content_video", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@topic_id", topic.topic_id);
                cmd.Parameters.AddWithValue("@video_title", topic.video_title);
                cmd.Parameters.AddWithValue("@video_file_id", topic.video_file_id);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<VideoModel>> GetAllContentVideos()
        {
            List<VideoModel> lst = new List<VideoModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblcontentvideos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_video_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int t_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int content_video_id = Convert.ToInt32(dr["content_video_id"].ToString());
                    string publicfolderid = dr["publicfolderid"].ToString();
                    string video_title = dr["video_title"].ToString();
                    string video_file_id = dr["video_file_id"].ToString();


                    VideoModel e = new VideoModel()
                    {
                        topic_id = t_id,
                        topic_name = topic_name,
                        video_file_id = video_file_id,
                        video_title = video_title,
                        folder_id = publicfolderid,
                        content_video_id = content_video_id
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        //public async Task<List<VideoModel>> GetTopicWiseContentVideos(int topic_id)
        //{
        //    List<VideoModel> lst = new List<VideoModel>();
        //    using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("sp_fetch_topicwise_tblcontentvideos", con);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@topic_id", topic_id);
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        int cnt = 0;
        //        while (dr.Read())
        //        {
        //            int t_id = Convert.ToInt32(dr["topic_id"].ToString());
        //            string topic_name = dr["topic_name"].ToString();
        //            int content_video_id = Convert.ToInt32(dr["content_video_id"].ToString());
        //            string publicfolderid = dr["publicfolderid"].ToString();
        //            string video_title = dr["video_title"].ToString();
        //            string video_file_id = dr["video_file_id"].ToString();
        //            cnt++;

        //            VideoModel e = new VideoModel()
        //            {
        //                topic_id = t_id,
        //                topic_name = topic_name,
        //                video_file_id = video_file_id,
        //                video_title = video_title,
        //                folder_id = publicfolderid,
        //                content_video_id = content_video_id
        //            };
        //            lst.Add(e);
        //        }
        //        con.Close();
        //    }

        //    return    lst;
        //}

        public async Task<TopicModel> GetTopicWiseContentVideos(int topic_id)
        {
            TopicModel t = await topicService.GetTrainingTopic(topic_id);
            if (t.folder_id != "" && t.folder_id != null)
            {

                List<VideoModel> videos = await topicService.GetVideos(t.folder_id, t.topic_id);
                t.videos = videos;



            }
            return t;
        }

        public async Task<ContentModel> GetTopicContent(int content_id)
        {
            ContentModel st = new ContentModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltopic_contents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", content_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int t_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();

                    string content_name = dr["content_name"].ToString();
                    st = new ContentModel() { topic_id = t_id, topic_name = topic_name, content_id = content_id, content_name = content_name };

                }
                con.Close();
            }
            return st;
        }
        public async Task<List<ContentModel>> GetTopicWiseContents(int topic_id)
        {
            List<ContentModel> lst = new List<ContentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltopic_wise_contents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int t_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string content_name = dr["content_name"].ToString();
                    ContentModel e = new ContentModel() { topic_id = t_id, topic_name = topic_name, content_id = content_id, content_name = content_name };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<ContentModel>> GetAllTopicContents()
        {
            List<ContentModel> lst = new List<ContentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltopic_contents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int t_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    string content_name = dr["content_name"].ToString();
                    ContentModel e = new ContentModel() { topic_id = t_id, topic_name = topic_name, content_id = content_id, content_name = content_name };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<ContentQuestionModel>> GetContentWiseQuestions(int content_id)
        {
            List<ContentQuestionModel> lst = new List<ContentQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_content_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", content_id);

                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    //    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    string question = dr["question"].ToString();
                    string option1 = dr["option1"].ToString();
                    string option2 = dr["option2"].ToString();
                    string option3 = dr["option3"].ToString();
                    string option4 = dr["option4"].ToString();
                    int correct_option_number = Convert.ToInt32(dr["correct_option_number"].ToString());


                    ContentQuestionModel e = new ContentQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        question_id = question_id,
                        question = question,
                        option1 = option1,
                        option2 = option2,
                        option3 = option3,
                        option4 = option4,
                        correct_option_number = correct_option_number,
                    };
                    i++;
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<ContentQuestionModel>> GetAllContentQuestions()
        {
            List<ContentQuestionModel> lst = new List<ContentQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_content_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", 0);

                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    int question_id = Convert.ToInt32(dr["question_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    string content_name = dr["content_name"].ToString();
                    string question = dr["question"].ToString();
                    string option1 = dr["option1"].ToString();
                    string option2 = dr["option2"].ToString();
                    string option3 = dr["option3"].ToString();
                    string option4 = dr["option4"].ToString();
                    int correct_option_number = Convert.ToInt32(dr["correct_option_number"].ToString());


                    ContentQuestionModel e = new ContentQuestionModel()
                    {
                        topic_id = topic_id,
                        topic_name = topic_name,
                        content_id = content_id,
                        content_name = content_name,
                        question_id = question_id,
                        question = question,
                        option1 = option1,
                        option2 = option2,
                        option3 = option3,
                        option4 = option4,
                        correct_option_number = correct_option_number,
                    };
                    i++;
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task AddTopicContent(TopicModel topic)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_addtopic_wise_contents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic.topic_id);
                DataTable dt = new DataTable();
                dt.Columns.Add("content_name", typeof(string));
                foreach (ContentModel c in topic.contents)
                {
                    dt.Rows.Add(c.content_name);
                }
                cmd.Parameters.AddWithValue("@contents", dt);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }


        public async Task UpdateTopicContent(ContentModel content)
        {

            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltopic_contents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@content_id", content.content_id);
                cmd.Parameters.AddWithValue("@content_name", content.content_name);
                cmd.Parameters.AddWithValue("@topic_id", content.topic_id);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task AddContentQuestion(int content_id, List<ContentQuestionModel> questions)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_tblcontent_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@content_id", content_id);
                DataTable dt = new DataTable();
                dt.Columns.Add("question", typeof(string));
                dt.Columns.Add("option1", typeof(string));
                dt.Columns.Add("option2", typeof(string));
                dt.Columns.Add("option3", typeof(string));
                dt.Columns.Add("option4", typeof(string));
                dt.Columns.Add("correct_option_number", typeof(int));
                foreach (ContentQuestionModel c in questions)
                {
                    dt.Rows.Add(c.question, c.option1, c.option2, c.option3, c.option4, c.correct_option_number);
                }
                cmd.Parameters.AddWithValue("@questions", dt);

                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task DeleteTopicContent(int content_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltopic_contents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@content_id", content_id);
                cmd.Parameters.AddWithValue("@content_name", "");
                cmd.Parameters.AddWithValue("@topic_id", 0);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<ContentQuestionModel>> GetContentWiseQuestion(int content_id)
        {
            throw new NotImplementedException();
        }

        //public void AddContentQuestion(ContentQuestionModel question)
        //{
        //    using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("sp_tbltopic_contents", con);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@type", "Delete");
        //        cmd.Parameters.AddWithValue("@content_id", content_id);
        //        cmd.Parameters.AddWithValue("@content_name", "");
        //        cmd.Parameters.AddWithValue("@topic_id", 0);
        //        int cnt = cmd.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        public async Task UpdateContentQuestion(ContentQuestionModel question)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_modify_tblcontent_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@question_id", question.question_id);
                cmd.Parameters.AddWithValue("@content_id", question.content_id);
                cmd.Parameters.AddWithValue("@question", question.question);
                cmd.Parameters.AddWithValue("@option1", question.option1);
                cmd.Parameters.AddWithValue("@option2", question.option2);
                cmd.Parameters.AddWithValue("@option3", question.option3);
                cmd.Parameters.AddWithValue("@option4", question.option4);
                cmd.Parameters.AddWithValue("@correct_option_number", question.correct_option_number);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task DeleteContentQuestion(int question_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_modify_tblcontent_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@question_id", question_id);
                cmd.Parameters.AddWithValue("@content_id", 0);
                cmd.Parameters.AddWithValue("@question", "");
                cmd.Parameters.AddWithValue("@option1", "");
                cmd.Parameters.AddWithValue("@option2", "");
                cmd.Parameters.AddWithValue("@option3", "");
                cmd.Parameters.AddWithValue("@option4", "");
                cmd.Parameters.AddWithValue("@correct_option_number", 0);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task RestoreContentQuestion(int question_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_modify_tblcontent_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Restore");
                cmd.Parameters.AddWithValue("@question_id", question_id);
                cmd.Parameters.AddWithValue("@content_id", 0);
                cmd.Parameters.AddWithValue("@question", "");
                cmd.Parameters.AddWithValue("@option1", "");
                cmd.Parameters.AddWithValue("@option2", "");
                cmd.Parameters.AddWithValue("@option3", "");
                cmd.Parameters.AddWithValue("@option4", "");
                cmd.Parameters.AddWithValue("@correct_option_number", 0);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }


        public async Task AddBulkContents(DataTable dt, string topic_name, int topic_id)
        {
            List<string> names = new List<string>();
            List<ContentModel> contents = new List<ContentModel>();
            foreach (DataRow dr in dt.Rows)
            {
                string tname = dr[3].ToString();
                if (tname == topic_name)
                {
                    string content_name = dr[1].ToString();
                    contents.Add(new ContentModel() { content_name=content_name });
                }
            }
            TopicModel t = new TopicModel() { topic_id=topic_id, topic_name=topic_name, contents=contents };
         await   AddTopicContent(t);
        }




        //public async Task<List<TopicModel>> GetCourseWiseTopicAndContents(int course_id)
        //{
        //    List<TopicModel> lst = new List<TopicModel>();
        //    using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("sp_course_wise_topics", con);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@course_id", course_id);
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            int t_id = Convert.ToInt32(dr["topic_id"].ToString());
        //            string topic_name = dr["topic_name"].ToString();
        //            List<ContentModel> contents = await GetAllTopicWiseContentQuestionAndInterviewQuestionsCounts(t_id);
        //            int total_interview_questions = contents.Sum(e => e.total_interview_questions);
        //            int total_program_questions = contents.Sum(e => e.total_program_questions);
        //            TopicModel e = new TopicModel() { topic_id = t_id, topic_name = topic_name, contents = contents, total_interview_question_count = total_interview_questions, total_program_count = total_program_questions };
        //            lst.Add(e);
        //        }
        //        con.Close();
        //    }
        //    return lst;
        //}
        public async Task<List<TopicModel>> GetCourseWiseTopicAndContents(int course_id)
        {
            List<TopicModel> lst = new List<TopicModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_course_wise_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@course_id", course_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int t_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    List<ContentModel> contents = await GetTopicWiseContents(t_id);
                    int total_interview_questions = 0;// contents.Sum(e => e.total_interview_questions);
                    int total_program_questions = 0;// contents.Sum(e => e.total_program_questions);
                    TopicModel e = new TopicModel() { topic_id = t_id, topic_name = topic_name, contents = contents, total_interview_question_count = total_interview_questions, total_program_count = total_program_questions };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<CourseModel> GetTrainingCourse(int course_id)
        {
            CourseModel st = new CourseModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltraining_courses", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@course_id", course_id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    //int cid = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    List<CourseFeeModel> coursefees = await courseService.GetCourseWiseFees(course_id);
                    List<TopicModel> topics = await GetCourseWiseTopicAndContents(course_id);
                    st = new CourseModel()
                    {
                        course_id = course_id,
                        course_name = course_name,
                        courseFees = coursefees,
                        topics = topics
                    };

                }
                con.Close();
            }
            return st;
        }

    

    }
}
