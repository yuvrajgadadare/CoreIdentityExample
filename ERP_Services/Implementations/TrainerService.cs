 
using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace ERP_Services.Implementations
{
    public class TrainerService : ITrainerService
    {
        public  async Task< EmployeeModel> CheckEmployeeLogin(string employee_code, string password)
        {
            EmployeeModel tr = null;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_check_employee_login", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_code", employee_code);
                cmd.Parameters.AddWithValue("@password", password);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int id = Convert.ToInt32(dr["employee_id"].ToString());
                    string name = dr["employee_name"].ToString();
                   // string employee_code = dr["employee_code"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string profile_photo = dr["profile_photo"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string gender = dr["gender"].ToString();
                     tr = new EmployeeModel()
                    {
                        email_address = email_address,
                        gender = gender,
                        mobile_number = mobile_number,
                        profile_photo = profile_photo,
                        //qualification = qualification,
                        employee_id = id,
                        employee_name = name,
                        employee_code= employee_code
                     };
                  
                }
                 
                con.Close();
            }
            return tr;
        }
    }
}
