using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Implementations
{
    public class TopicService:ITopicService
    {
      //  IContentService contentService;
        ICourseService courseService;
        private readonly ICacheService _cacheService;
        public TopicService(ICourseService courseService, ICacheService cacheService )
        {
            this.courseService = courseService;
            //    this.contentService = contentService;
          _cacheService = cacheService;
        }
        public async Task<List<TopicModel>> GetAllYoutubeTopicVideos(List<TopicModel> topics)
        {
            List<TopicModel> lst = new List<TopicModel>();

            foreach (TopicModel t in topics)
            {
                if (t.folder_id != "" && t.folder_id != null)
                {

                    List<VideoModel> videos = await GetVideos(t.folder_id, t.topic_id);
                    t.videos = videos;
                    lst.Add(t);


                }

            }

            return lst;
        }
        public async Task<List<VideoModel>> GetVideos(string publicFolderId, int topic_id)
        {
            List<VideoModel> lst = new List<VideoModel>();
            var httpClient = new HttpClient();
            //  var publicFolderId = "19uvwt8_anE869Zbzu2gFDWQvNyUGhtE3";
            var googleDriveApiKey = "AIzaSyDIaGnTInCqBeUCJXEqO5ignIyVk9YLNzE";
            var nextPageToken = "";
            do
            {
                var folderContentsUri = $"https://www.googleapis.com/drive/v3/files?q='{publicFolderId}'+in+parents&key={googleDriveApiKey}";

                if (!String.IsNullOrEmpty(nextPageToken))
                {
                    folderContentsUri += $"&pageToken={nextPageToken}";
                }
                // ViewBag.folder = "https://drive.google.com/file/d";
                var contentsJson = await httpClient.GetStringAsync(folderContentsUri);
                var contents = (JObject)JsonConvert.DeserializeObject(contentsJson);
                nextPageToken = (string)contents["nextPageToken"];
                foreach (var file in (JArray)contents["files"])
                {
                    var id = (string)file["id"];
                    var name = (string)file["name"];
                    Console.WriteLine($"{id}:{name}");
                    lst.Add(new VideoModel { video_file_id = id, video_title = name.Split('.')[0], topic_id = topic_id });
                }
            } while (!String.IsNullOrEmpty(nextPageToken));

            return lst;
        }
        public async Task<List<TopicVideoModel>> GetAllTopicVideos()
        {
            List<TopicVideoModel> lst = new List<TopicVideoModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltopic_videos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@video_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int video_id = Convert.ToInt32(dr["video_id"].ToString());
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string video_title = dr["video_title"].ToString();
                    string video_url = dr["video_url"].ToString();
                    string video_description = dr["video_description"].ToString();
                    string topic_name = dr["topic_name"].ToString();

                    TopicVideoModel e = new TopicVideoModel()
                    {
                        topic_id = topic_id,
                        video_description = video_description,
                        video_url = video_url,
                        video_title = video_title,
                        video_id = video_id,
                        topic_name = topic_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<ContentQuestionModel>> GetTopicWiseQuestions(int topic_id, int count)
        {
            List<ContentQuestionModel> lst = new List<ContentQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topicwise_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                cmd.Parameters.AddWithValue("@size", count);
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
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
                        submitted_option_number = 0,
                        serial_number = i
                    };
                    i++;
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<TopicVideoModel> GetTopicVideo(int video_id)
        {
            TopicVideoModel st = new TopicVideoModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltopic_videos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@video_id", video_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string video_title = dr["video_title"].ToString();
                    string video_url = dr["video_url"].ToString();
                    string video_description = dr["video_description"].ToString();
                    string topic_name = dr["topic_name"].ToString();

                    st = new TopicVideoModel()
                    {
                        topic_id = topic_id,
                        video_description = video_description,
                        video_url = video_url,
                        video_title = video_title,
                        video_id = video_id,
                        topic_name = topic_name
                    };

                }
                con.Close();
            }
            return st;
        }
        public async Task<List<ContentQuestionModel>> GetTopicWiseQuestions(int topic_id)
        {
            List<ContentQuestionModel> lst = new List<ContentQuestionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topic_wise_content_questions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    int content_id = Convert.ToInt32(dr["content_id"].ToString());
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
        public async Task<List<TopicVideoModel>> GetTopicWiseVideos(int topic_id)
        {
            List<TopicVideoModel> lst = new List<TopicVideoModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_topic_wise_videos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@video_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int video_id = Convert.ToInt32(dr["video_id"].ToString());
                    string video_title = dr["video_title"].ToString();
                    string video_url = dr["video_url"].ToString();
                    string video_description = dr["video_description"].ToString();
                    string topic_name = dr["topic_name"].ToString();
                    TopicVideoModel e = new TopicVideoModel()
                    {
                        topic_id = topic_id,
                        video_description = video_description,
                        video_url = video_url,
                        video_title = video_title,
                        video_id = video_id,
                        topic_name = topic_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<TopicModel>> GetTrainingTopics()
        {
            var cacheData = await _cacheService.GetData<List<TopicModel>>("TopicModels");
            if (cacheData != null)
            {
                return cacheData;
            }
            List<TopicModel> lst = new List<TopicModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltraining_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("topic_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string publicfolderid = dr["publicfolderid"].ToString();
                    int id = Convert.ToInt32(dr["topic_id"].ToString());
                    string name = dr["topic_name"].ToString();
                    TopicModel e = new TopicModel() { topic_id = id, topic_name = name, folder_id = publicfolderid, is_selected = false };

                    if (publicfolderid != "")
                    {
                        e.video_status = true;
                    }
                    else
                    {
                        e.video_status = false;
                    }
                    lst.Add(e);

                }
                con.Close();
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            cacheData = lst;
            _cacheService.SetData<IEnumerable<TopicModel>>("TopicModels", cacheData, expirationTime);
            return lst;
        }
        public async Task AddTopic(TopicModel topic)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltraining_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@topic_id", topic.topic_id);
                cmd.Parameters.AddWithValue("@topic_name", topic.topic_name);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task UpdateTopic(TopicModel topic)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltraining_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@topic_id", topic.topic_id);
                cmd.Parameters.AddWithValue("@topic_name", topic.topic_name);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task DeleteTopic(int topic_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltraining_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@topic_id", topic_id);
                cmd.Parameters.AddWithValue("@topic_name", "");
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task AddCourseTopics(CourseModel course)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_course_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@course_id", course.course_id);
                DataTable dt = new DataTable();
                dt.Columns.Add("topic_id", typeof(int));
                foreach (TopicModel t in course.topics)
                {
                    dt.Rows.Add(t.topic_id);
                }
                cmd.Parameters.AddWithValue("@topics", dt);

                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task<TopicModel> GetTrainingTopic(int topic_id)
        {
            TopicModel e = new TopicModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltraining_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("topic_id", topic_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //int id = Convert.ToInt32(dr["topic_id"].ToString());
                    //string name = dr["topic_name"].ToString();
                    // st = new TopicModel() { topic_id = id, topic_name = name, is_selected = false };
                    string publicfolderid = dr["publicfolderid"].ToString();
                    int id = Convert.ToInt32(dr["topic_id"].ToString());
                    string name = dr["topic_name"].ToString();
                    e = new TopicModel() { topic_id = id, topic_name = name, folder_id = publicfolderid, is_selected = false };

                    if (publicfolderid != "")
                    {
                        e.video_status = true;
                    }
                    else
                    {
                        e.video_status = false;
                    }

                }
                con.Close();
            }
            return e;
        }
        public async Task AddTopicFolderId(TopicModel topic)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_topic_folder_id", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@topic_id", topic.topic_id);
                cmd.Parameters.AddWithValue("@publicfolderid", topic.folder_id);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task AddTopicVideo(TopicVideoModel vm)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltopic_videos", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@video_id", vm.video_id);
                cmd.Parameters.AddWithValue("@topic_id", vm.topic_id);
                cmd.Parameters.AddWithValue("@video_title", vm.video_title);
                cmd.Parameters.AddWithValue("@video_url", vm.video_url);
                cmd.Parameters.AddWithValue("@video_description", vm.video_description);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task<List<TopicModel>> GetTopicByCourseIds(int course_id)
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
                    TopicModel tm = await GetTrainingTopic(t_id);

                    lst.Add(tm);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<TopicModel>> GetCourseWiseTopics(int course_id)
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
                    int ct_id = Convert.ToInt32(dr["course_topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();
                    // List<ContentModel> contents = await contentService.GetAllTopicWiseContentQuestionAndInterviewQuestionsCounts(t_id);
                    //    int total_interview_questions = contents.Sum(e => e.total_interview_questions);
                    //   int total_program_questions = contents.Sum(e => e.total_program_questions);

                    List<ContentQuestionModel> questions = await GetTopicWiseQuestions(t_id);
                    TopicModel e = new TopicModel()
                    {
                        topic_id = t_id,
                        topic_name = topic_name,
                        course_topic_id = ct_id,
                         total_content_question_count = questions.Count()
                      //    total_interview_question_count= total_interview_questions,
                      //     total_program_count=total_program_questions
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task AddBulkTopics(DataTable dt)
        {
            List<string> names = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string topic_name = dr[1].ToString();
                TopicModel t = new TopicModel() { topic_name=topic_name };
             await   AddTopic(t);
            }
        }
        public async Task<List<CourseModel>> GetTrainingCoursesWithTopics()
        {
            var cacheData = await _cacheService.GetData<List<CourseModel>>("TrainingCourses");
            if (cacheData != null)
            {
                return cacheData;
            }
            List<CourseModel> lst = new List<CourseModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltraining_courses", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@course_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    List<TopicModel> topics = await GetCourseWiseTopics(course_id);
                    List<CourseFeeModel> fees = await courseService.GetCourseWiseFees(course_id);
                    CourseModel e = new CourseModel()
                    {
                        course_id = course_id,
                        course_name = course_name,
                        courseFees = fees,
                        topics = topics
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            cacheData = lst;
            _cacheService.SetData<IEnumerable<CourseModel>>("TrainingCourses", cacheData, expirationTime);
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
                    List<TopicModel> topics = await GetCourseWiseTopics(course_id);
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
