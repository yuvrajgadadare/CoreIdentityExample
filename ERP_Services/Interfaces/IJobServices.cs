using ERP_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Interfaces
{
    public interface IJobServices
    {
       Task AddJobOpening(JobOpeningModel job);
       Task< List<JobOpeningModel>> GetAllJobsOpening();
        Task <JobOpeningModel> GetJobsOpeningById(int OpeningId);
       Task< List<JobApplicationModel>> GetJobsOpeningWiseApplications(int OpeningId);
        Task AddJobApplication(JobApplicationModel job);
        Task<List<JobApplicationModel>> GetAllJobApplications();
        Task<List<JobApplicationModel>> GetStudentWiseJobApplications(int StudentId);
        Task<List<JobApplicationModel>> GetJobOpeningWiseApplications(int OpeningId);
        Task<List<JobApplicationModel>> GetJobOpeningWiseAttendedStudents(int OpeningId);
        Task<List<JobApplicationModel>> GetJobAOpeningWiseNotAttendedStudents(int OpeningId);
        Task<List<JobApplicationModel>> GetJobApplicationWiseRoundResults(int ApplicationId);
        Task<List<JobApplicationModel>> GetStudentWiseRoundResults(int ApplicationId);
    }
}
