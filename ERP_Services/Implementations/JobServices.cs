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
    public class JobServices : IJobServices
    {
        public async Task AddJobApplication(JobApplicationModel job)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblStudent_Job_applications", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", job.student_id);
                cmd.Parameters.AddWithValue("@opening_id", job.opening_id);
                cmd.Parameters.AddWithValue("@application_date", job.application_date);
                cmd.Parameters.AddWithValue("@is_interested", job.is_interested);
                cmd.Parameters.AddWithValue("@is_attended", job.is_attended);
                cmd.Parameters.AddWithValue("@status", job.status);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task AddJobOpening(JobOpeningModel job)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_tbljob_openings", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@opening_date", job.opening_date);
                cmd.Parameters.AddWithValue("@company_name", job.company_name);
                cmd.Parameters.AddWithValue("@location", job.location);
                cmd.Parameters.AddWithValue("@technnology", job.technnology);
                cmd.Parameters.AddWithValue("@experience_required", job.experience_required);
                cmd.Parameters.AddWithValue("@passing_year", job.passing_year);
                cmd.Parameters.AddWithValue("@job_description", job.job_description);
                cmd.Parameters.AddWithValue("@offering_package", job.offering_package);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<JobApplicationModel>> GetAllJobApplications()
        {

            List<JobApplicationModel> lst = new List<JobApplicationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_all_Job_opening_applications", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int opening_id = Convert.ToInt32(dr["opening_id"].ToString());
                    DateTime opening_date = Convert.ToDateTime(dr["opening_date"].ToString());
                    string company_name = dr["company_name"].ToString();
                    string location = dr["location"].ToString();
                    string technnology = dr["technnology"].ToString();
                    string experience_required = dr["experience_required"].ToString();
                    string passing_year = dr["passing_year"].ToString();
                    string job_description = dr["job_description"].ToString();
                    string offering_package = dr["offering_package"].ToString();
                    int student_id = Convert.ToInt32(dr["student_id"].ToString()); 
                    DateTime application_date = Convert.ToDateTime(dr["application_date"].ToString());
                    int is_interested = Convert.ToInt32(dr["is_interested"].ToString());
                    int is_attended = Convert.ToInt32(dr["is_attended"].ToString());
                    string status = dr["status"].ToString();
                    string student_name = dr["student_name"].ToString();
                    JobApplicationModel e = new JobApplicationModel()
                    {
                        opening_id = opening_id,
                        offering_package = offering_package,
                        job_description = job_description,
                        passing_year = passing_year,
                        experience_required = experience_required,
                        technnology = technnology,
                        location = location,
                        company_name = company_name,
                        opening_date = opening_date,
                        application_date = application_date,
                        is_interested = is_interested,
                        is_attended = is_attended,
                        student_id = student_id,
                        student_name = student_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<JobOpeningModel>> GetAllJobsOpening()
        {
            List<JobOpeningModel> lst = new List<JobOpeningModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_all_Job_openings", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int opening_id = Convert.ToInt32(dr["opening_id"].ToString());
                    DateTime  opening_date = Convert.ToDateTime(dr["opening_date"].ToString());
                    string company_name = dr["company_name"].ToString();
                    string location = dr["location"].ToString();
                    string technnology = dr["technnology"].ToString();
                    string experience_required = dr["experience_required"].ToString();
                    string passing_year = dr["passing_year"].ToString();
                    string job_description = dr["job_description"].ToString();
                    string offering_package = dr["offering_package"].ToString();

                    JobOpeningModel e = new JobOpeningModel()
                    {
                        opening_id = opening_id,
                        offering_package = offering_package,
                        job_description = job_description,
                        passing_year = passing_year,
                        experience_required = experience_required,
                        technnology = technnology,
                        location = location,
                        company_name = company_name,
                        opening_date = opening_date
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<JobApplicationModel>> GetJobAOpeningWiseNotAttendedStudents(int OpeningId)
        {
            List<JobApplicationModel> lst = new List<JobApplicationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_jobapplication_wise_notattendedStudents", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@opening_id", OpeningId);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int opening_id = Convert.ToInt32(dr["opening_id"].ToString());
                    DateTime opening_date = Convert.ToDateTime(dr["opening_date"].ToString());
                    string company_name = dr["company_name"].ToString();
                    string location = dr["location"].ToString();
                    string technnology = dr["technnology"].ToString();
                    string experience_required = dr["experience_required"].ToString();
                    string passing_year = dr["passing_year"].ToString();
                    string job_description = dr["job_description"].ToString();
                    string offering_package = dr["offering_package"].ToString();
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    DateTime application_date = Convert.ToDateTime(dr["application_date"].ToString());
                    int is_interested = Convert.ToInt32(dr["is_interested"].ToString());
                    int is_attended = Convert.ToInt32(dr["is_attended"].ToString());
                    string status = dr["status"].ToString();
                    string student_name = dr["student_name"].ToString();
                    int application_id = Convert.ToInt32(dr["application_id"].ToString());

                    JobApplicationModel e = new JobApplicationModel()
                    {
                        opening_id = opening_id,
                        offering_package = offering_package,
                        job_description = job_description,
                        passing_year = passing_year,
                        experience_required = experience_required,
                        technnology = technnology,
                        location = location,
                        company_name = company_name,
                        opening_date = opening_date,
                        application_date = application_date,
                        is_interested = is_interested,
                        is_attended = is_attended,
                        student_id = student_id,
                        student_name = student_name,
                        status = status,
                        application_id = application_id
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<JobApplicationModel>> GetJobApplicationWiseRoundResults(int ApplicationId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobApplicationModel>> GetJobOpeningWiseApplications(int OpeningId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobApplicationModel>> GetJobOpeningWiseAttendedStudents(int OpeningId)
        {
            throw new NotImplementedException();
        }

        public async Task<JobOpeningModel> GetJobsOpeningById(int OpeningId)
        {
            JobOpeningModel st = new JobOpeningModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_Job_opening_by_id", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@opening_id", OpeningId);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int opening_id = Convert.ToInt32(dr["opening_id"].ToString());
                    DateTime opening_date = Convert.ToDateTime(dr["opening_date"].ToString());
                    string company_name = dr["company_name"].ToString();
                    string location = dr["location"].ToString();
                    string technnology = dr["technnology"].ToString();
                    string experience_required = dr["experience_required"].ToString();
                    string passing_year = dr["passing_year"].ToString();
                    string job_description = dr["job_description"].ToString();
                    string offering_package = dr["offering_package"].ToString();

                    st = new JobOpeningModel()
                    {
                        opening_id = opening_id,
                        offering_package = offering_package,
                        job_description = job_description,
                        passing_year = passing_year,
                        experience_required = experience_required,
                        technnology = technnology,
                        location = location,
                        company_name = company_name,
                        opening_date = opening_date
                    };
                    
                }
                con.Close();
            }
            return   st;
        }

        public async Task<List<JobApplicationModel>> GetJobsOpeningWiseApplications(int OpeningId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobApplicationModel>> GetStudentWiseJobApplications(int StudentId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JobApplicationModel>> GetStudentWiseRoundResults(int ApplicationId)
        {
            throw new NotImplementedException();
        }
    }
}
