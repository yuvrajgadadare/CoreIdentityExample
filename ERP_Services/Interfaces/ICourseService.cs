using ERP_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Interfaces
{
    public interface ICourseService
    {
        Task AddTrainingCourse(CourseModel course);
        Task DeleteCourse(int course_id);

        Task AddCourseFees(CourseFeeModel fee);
        Task DeleteCourseFees(int fee_id);
        Task DeleteCourseTopic(int course_topic_id);
        Task<List<CourseModel>> GetTrainingCourses();
        Task<List<CourseFeeModel>> GetCoursesWithMinimumFees();
        Task<List<CourseFeeModel>> GetCourseFees();
        Task<CourseFeeModel> GetCourseFee(int fee_id);
        Task<CourseModel> GetTrainingCourse(int course_id);
        Task<List<CourseFeeModel>> GetCourseWiseFees(int course_id);
    }
}
