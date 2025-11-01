using ERP_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Interfaces
{
    public interface IContentService
    {
        Task AddTopicContent(TopicModel topic);

        Task UpdateTopicContent(ContentModel topic);
        Task AddContentVideo(VideoModel topic);
        Task<List<VideoModel>> GetAllContentVideos();
        Task<List<ContentModel>> GetTopicWiseContents(int topic_id);
        Task<List<ContentModel>> GetAllTopicContents();
        Task<List<ContentModel>> GetAllTopicWiseContentQuestionAndInterviewQuestionsCounts(int topic_id);
        Task<ContentModel> GetTopicContent(int content_id);
        Task<List<ContentQuestionModel>> GetContentWiseQuestion(int content_id);
        Task AddContentQuestion(int content_id, List<ContentQuestionModel> questions);
        Task DeleteTopicContent(int content_id);
        Task UpdateContentQuestion(ContentQuestionModel question);
        Task DeleteContentQuestion(int question_id);
        Task RestoreContentQuestion(int question_id);
        Task<TopicModel> GetTopicWiseContentVideos(int topic_id);
        Task<List<ContentQuestionModel>> GetContentWiseQuestions(int content_id);
        
        Task<List<ContentQuestionModel>> GetAllContentQuestions();
       Task AddBulkContents(DataTable dt, string topic_name, int topic_id);
        Task <List<TopicModel>> GetCourseWiseTopicAndContents(int course_id);
        //Task <CourseModel> GetStudentWiseCourseSchedule(int registration_id,int course_id);
        Task<CourseModel> GetTrainingCourse(int course_id);
    }
}
