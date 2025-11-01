using ERP_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Interfaces
{
    public interface ITopicService
    {
        Task AddTopic(TopicModel topic);
        Task UpdateTopic(TopicModel topic);
        Task AddTopicFolderId(TopicModel topic);
        Task<List<TopicModel>> GetAllYoutubeTopicVideos(List<TopicModel> topis);

        //Task< List<VideoModel>> GetTopicWiseContentVideos(int topic_id);
        Task<List<TopicModel>> GetTopicByCourseIds(int course_id);
        Task<List<TopicModel>> GetCourseWiseTopics(int course_id);

        Task DeleteTopic(int topic_id);

        Task AddCourseTopics(CourseModel course);


        Task<List<TopicModel>> GetTrainingTopics();
        Task<TopicModel> GetTrainingTopic(int topic_id);
        Task AddTopicVideo(TopicVideoModel vm);
        Task<List<TopicVideoModel>> GetAllTopicVideos();
        Task<List<TopicVideoModel>> GetTopicWiseVideos(int topic_id);
        Task<TopicVideoModel> GetTopicVideo(int video_id);
        Task<List<ContentQuestionModel>> GetTopicWiseQuestions(int topic_id, int count);
        //List<ContentQuestionModel> GetTopicWiseQuestions(int topic_id);
        Task<List<ContentQuestionModel>> GetTopicWiseQuestions(int topic_id);
        Task<List<VideoModel>> GetVideos(string publicFolderId, int topic_id);
        Task AddBulkTopics(DataTable dt);

        Task<List<CourseModel>> GetTrainingCoursesWithTopics();
        Task<CourseModel> GetTrainingCourse(int course_id);


    }
}
