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
    public class CourseService:ICourseService
    {
        //ITopicService topicService;
        //public CourseService(ITopicService topicService)
        //{
        //    this.topicService = topicService;
        //}
        public async Task AddTrainingCourse(CourseModel course)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltraining_courses", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@course_id", course.course_id);
                cmd.Parameters.AddWithValue("@course_name", course.course_name);
                cmd.Parameters.AddWithValue("@cid", 0);

                DataTable dt = new DataTable();
                dt.Columns.Add("fees_amount", typeof(float));
                dt.Columns.Add("gst", typeof(float));
                dt.Columns.Add("fee_mode", typeof(string));
                dt.Columns.Add("fees_change_date", typeof(DateTime));
                foreach (CourseFeeModel f in course.courseFees)
                {
                    dt.Rows.Add(f.fees_amount, f.gst, f.fee_mode, f.fees_change_date);
                }
                cmd.Parameters.AddWithValue("@fee", dt);

                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task AddCourseFees(CourseFeeModel fee)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltraining_course_fees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@fee_id", fee.fee_id);
                cmd.Parameters.AddWithValue("@course_id", fee.course_id);
                cmd.Parameters.AddWithValue("@fees_amount", fee.fees_amount);
                cmd.Parameters.AddWithValue("@gst", fee.gst);
                cmd.Parameters.AddWithValue("@fee_mode", fee.fee_mode);
                cmd.Parameters.AddWithValue("@fees_change_date", fee.fees_change_date);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task<List<CourseFeeModel>> GetCourseWiseFees(int course_id)
        {
            List<CourseFeeModel> lst = new List<CourseFeeModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_coursewise_fees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@course_id", course_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    string fee_mode = dr["fee_mode"].ToString();
                    DateTime fees_change_date = Convert.ToDateTime(dr["fees_change_date"].ToString());

                    CourseFeeModel e = new CourseFeeModel()
                    {
                        course_id = course_id,
                        course_name = course_name,
                        fees_amount = fees_amount,
                        fee_id = fee_id,
                        fee_mode = fee_mode,
                        gst = gst,
                        fees_change_date = fees_change_date
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<CourseFeeModel>> GetCourseFees()
        {
            List<CourseFeeModel> lst = new List<CourseFeeModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltraining_course_fees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fee_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    string fee_mode = dr["fee_mode"].ToString();
                    DateTime fees_change_date = Convert.ToDateTime(dr["fees_change_date"].ToString());
                    CourseFeeModel e = new CourseFeeModel()
                    {
                        course_id = course_id,
                        course_name = course_name,
                        fees_amount = fees_amount,
                        fee_id = fee_id,
                        fee_mode = fee_mode,
                        gst = gst,
                        fees_change_date = fees_change_date,
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<CourseModel>> GetTrainingCourses()
        {
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
                   // List<TopicModel> topics = await topicService.GetCourseWiseTopics(course_id);
                    List<CourseFeeModel> fees = await GetCourseWiseFees(course_id);
                    CourseModel e = new CourseModel()
                    {
                        course_id = course_id,
                        course_name = course_name,
                        courseFees = fees,
                      //  topics = topics
                    };
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
                    List<CourseFeeModel> coursefees = await GetCourseWiseFees(course_id);
                   // List<TopicModel> topics = await topicService.GetCourseWiseTopics(course_id);
                    st = new CourseModel()
                    {
                        course_id = course_id,
                        course_name = course_name,
                        courseFees = coursefees,
                   //     topics = topics
                    };

                }
                con.Close();
            }
            return st;
        }
      
       
        public async Task DeleteCourse(int course_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltraining_courses", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@course_id", course_id);
                cmd.Parameters.AddWithValue("@course_name", "");
                cmd.Parameters.AddWithValue("@cid", 0);

                //DataTable dt = new DataTable();

                //cmd.Parameters.AddWithValue("@fee", dt);

                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task DeleteCourseFees(int fee_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tbltraining_course_fees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@fee_id", fee_id);
                cmd.Parameters.AddWithValue("@course_id", 0);
                cmd.Parameters.AddWithValue("@fees_amount", 0);
                cmd.Parameters.AddWithValue("@gst", 0);
                cmd.Parameters.AddWithValue("@fee_mode", "");
                cmd.Parameters.AddWithValue("@fees_change_date", null);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task<CourseFeeModel> GetCourseFee(int fee_id)
        {
            CourseFeeModel st = new CourseFeeModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbltraining_course_fees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fee_id", fee_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    // int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    string fee_mode = dr["fee_mode"].ToString();
                    // int total_installments = Convert.ToInt32(dr["total_installments"].ToString());
                    DateTime fees_change_date = Convert.ToDateTime(dr["fees_change_date"].ToString());

                    st = new CourseFeeModel()
                    {
                        course_id = course_id,
                        course_name = course_name,
                        fees_amount = fees_amount,
                        fee_id = fee_id,
                        fee_mode = fee_mode,
                        gst = gst,
                        fees_change_date = fees_change_date
                    };

                }
                con.Close();
            }
            return st;
        }
    }
}
