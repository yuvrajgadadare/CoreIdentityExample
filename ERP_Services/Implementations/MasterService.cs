 
using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace ERP_Services.Implementations
{
    public class MasterService : IMasterService
    {
    
        public async Task<StudentModel> CheckStudentLogin(string email_address, string password)
        {
            StudentModel student = null;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_checkStudentLogin", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email_Address", email_address);
                cmd.Parameters.AddWithValue("@password", password);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    DateTime birth_date =Convert.ToDateTime( dr["birth_date"].ToString());
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();

                    student = new StudentModel()
                    {
                        student_id = student_id,
                        student_name = student_name,
                        email_address = email_address,
                        gender = gender,
                        birth_date = birth_date,
                        mobile_number = mobile_number,
                        profile_photo = profile_photo,
                        qualification = qualification
                    };
                     
                }
                else
                {
                    student = null;
                }
                con.Close();
            }
            return  student;
        }
        //public async Task<List<CourseFeeModel>> GetCourseFees()
        //{
        //    List<CourseFeeModel> lst = new List<CourseFeeModel>();
        //    using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("sp_fetch_tbltraining_course_fees", con);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@fee_id", 0);
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            int course_id = Convert.ToInt32(dr["course_id"].ToString());
        //            int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
        //            string course_name = dr["course_name"].ToString();
        //            float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
        //            float gst = (float)Convert.ToDouble(dr["gst"].ToString());
        //            string fee_mode = dr["fee_mode"].ToString();
        //            DateTime fees_change_date = Convert.ToDateTime(dr["fees_change_date"].ToString());
        //            CourseFeeModel e = new CourseFeeModel()
        //            {
        //                course_id = course_id,
        //                course_name = course_name,
        //                fees_amount = fees_amount,
        //                fee_id = fee_id,
        //                fee_mode = fee_mode,
        //                gst = gst,
        //                 fees_change_date=fees_change_date,
        //            };
        //            lst.Add(e);
        //        }
        //        con.Close();
        //    }
        //    return lst;
        //}
        public async Task<List<EnquiryForModel>> GetEnquiryFors()
        {
            List <EnquiryForModel> lst=new List<EnquiryForModel>();
            using (SqlConnection con=new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblenquiry_fors", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["enquiry_for_id"].ToString());
                    string name = dr["enquiry_for"].ToString();
                    EnquiryForModel e = new EnquiryForModel() { enquiry_for_id = id, enquiry_for = name ,is_selected=false };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<LeadSourceModel>> GetLeadSources()
        {
            List<LeadSourceModel> lst = new List<LeadSourceModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tbllead_sources", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["source_id"].ToString());
                    string name = dr["source_name"].ToString();
                    LeadSourceModel e = new LeadSourceModel() {  source_id = id,  source_name = name, is_selected = false };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<PromotionalMessageModel>> GetPromotionalMessages()
        {
            List<PromotionalMessageModel> lst = new List<PromotionalMessageModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblpromotional_messages", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@message_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["message_id"].ToString());
                    string title = dr["message_title"].ToString();
                    string message = dr["message"].ToString();
                    PromotionalMessageModel e = new PromotionalMessageModel() {  message_id = id,  message_title = title, message=message };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task< List<QualificationModel>>  GetQualifications()
        {
            List<QualificationModel> lst = new List<QualificationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblqualifications", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["qualification_id"].ToString());
                    string name = dr["qualification"].ToString();
                    QualificationModel e = new QualificationModel() {  qualification_id = id,  qualification = name };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<RoleModel>> GetAllRoles()
        {
            List<RoleModel> lst = new List<RoleModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblroles", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@role_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string role_id =  dr["role_id"].ToString() ;
                    string role_name = dr["role_name"].ToString();
               

                    RoleModel e = new RoleModel()
                    {
                        role_id=role_id,
                         role_name=role_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<RoleModel> GetRole(string role_id)
        {
            RoleModel st = new  RoleModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblroles", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@role_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                   
                    string role_name = dr["role_name"].ToString();


                    st = new RoleModel()
                    {
                        role_id = role_id,
                        role_name = role_name
                    };
                   
                }
                con.Close();
            }
            return  st;
        }
    }
}
