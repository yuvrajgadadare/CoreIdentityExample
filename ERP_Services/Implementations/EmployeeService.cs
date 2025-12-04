
using ERP_Models;
using ERP_Services.Interfaces;
using ERPSystem_Models;
using Microsoft.Data.SqlClient;
using System.Data;
namespace ERP_Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        ITopicService topicService;
        public EmployeeService(ITopicService topicService)
        {
            this.topicService = topicService;
        }
        public async Task AddEmployeeDetails(EmployeeModel employee)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblemployees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@employee_id", employee.employee_id);
                cmd.Parameters.AddWithValue("@user_id", employee.user_id);
                cmd.Parameters.AddWithValue("@employee_name", employee.employee_name);
                cmd.Parameters.AddWithValue("@branch_id", employee.branch_id);
                cmd.Parameters.AddWithValue("@employee_code", employee.employee_code);
                   cmd.Parameters.AddWithValue("@email_address", employee.email_address);
                cmd.Parameters.AddWithValue("@mobile_number", employee.mobile_number);
                cmd.Parameters.AddWithValue("@profile_photo", employee.profile_photo);
                cmd.Parameters.AddWithValue("@gender", employee.gender);
                cmd.Parameters.AddWithValue("@qualification", employee.qualification);
                cmd.Parameters.AddWithValue("@birth_date", employee.birth_date);
                cmd.Parameters.AddWithValue("@joining_date", employee.joining_date);
                cmd.Parameters.AddWithValue("@salary", employee.salary);
               // cmd.Parameters.AddWithValue("@password", employee.password);

                SqlDataReader dr = cmd.ExecuteReader();
            }
        }

        public async Task AddEmployeeRole(RoleModel role)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_employee_role", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@employee_role_id", role.employee_role_id);
                cmd.Parameters.AddWithValue("@employee_id", role.employee_id);
                cmd.Parameters.AddWithValue("@role_id", role.role_id);
                
                SqlDataReader dr = cmd.ExecuteReader();
            }
        }

        public async Task AddTrainerTopics(List<TrainerTopicModel> topics)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_addtrainer_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                DataTable trainertopics = new DataTable();
                trainertopics.Columns.Add("employee_id", typeof(int));
                trainertopics.Columns.Add("topic_id", typeof(int));
                foreach (TrainerTopicModel e in topics)
                {
                    trainertopics.Rows.Add(e.employee_id, e.topic_id);
                }
                cmd.Parameters.AddWithValue("@emptype", trainertopics);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //public void AddTrainerDetails(EmployeeModel trainer)
        //{
        //    using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("sp_tbltrainers", con);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@type", "Insert");
        //        cmd.Parameters.AddWithValue("@employee_id", trainer.employee_id);
        //        cmd.Parameters.AddWithValue("@employee_name", trainer.employee_name);
        //        cmd.Parameters.AddWithValue("@gender", trainer.gender);
        //        cmd.Parameters.AddWithValue("@qualification", trainer.qualification);
        //        cmd.Parameters.AddWithValue("@mobile_number", trainer.mobile_number);
        //        cmd.Parameters.AddWithValue("@email_address", trainer.email_address);
        //        cmd.Parameters.AddWithValue("@password", trainer.password);
        //        cmd.Parameters.AddWithValue("@profile_photo", trainer.profile_photo);
        //        SqlDataReader dr = cmd.ExecuteReader();
        //    }
        //}

        //public async Task ChangeEmployeeDetailPassword(EmployeeModel employee)
        //{
        //    //using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
        //    //{
        //    //    con.Open();
        //    //    SqlCommand cmd = new SqlCommand("sp_change_employee_password", con);
        //    //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    //    cmd.Parameters.AddWithValue("@employee_id", employee.employee_id);
        //    //    //cmd.Parameters.AddWithValue("@password", employee.password);
        //    //    SqlDataReader dr = cmd.ExecuteReader();
        //    //}
        //}

        public async Task ChangeProfilePhoto(EmployeeModel employee)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_change_employee_photo", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", employee.employee_id);
                cmd.Parameters.AddWithValue("@profile_photo", employee.profile_photo);
                SqlDataReader dr = cmd.ExecuteReader();
            }
        }

        public async Task<EmployeeModel> CheckEmployeeLogin(string employee_code, string password)
        {
            EmployeeModel st = new EmployeeModel();
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

                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    DateTime joining_date = Convert.ToDateTime(dr["joining_date"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string role_name = dr["role_name"].ToString();
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string gender = dr["gender"].ToString();
                    string role_id =  dr["role_id"].ToString() ;
                    float salary = (float)Convert.ToDouble(dr["salary"].ToString());
                    st = new EmployeeModel()
                    {
                        employee_id = id,
                        birth_date = birth_date,
                        email_address = email_address,
                        employee_code = employee_code,
                        employee_name = employee_name,
                        joining_date = joining_date,
                        mobile_number = mobile_number,
                        role_id = role_id,
                        role_name = role_name,
                        salary = salary,
                         gender=gender,
                          qualification=qualification,
                         profile_photo=profile_photo
                    };
                    con.Close();
                    return st;
                }
                else
                {
                    con.Close();
                    return null;
                }
                
            }
            
        }

        public async Task DeleteEmployeeRole(int employee_role_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_employee_role", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@employee_role_id", employee_role_id);
                cmd.Parameters.AddWithValue("@employee_id", 0);
                cmd.Parameters.AddWithValue("@role_id", 0);

                SqlDataReader dr = cmd.ExecuteReader();
            }
        }

        public async Task<EmployeeModel> GetEmployee(int id)
        {
            EmployeeModel st = new EmployeeModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblemployees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    DateTime joining_date = Convert.ToDateTime(dr["joining_date"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    string employee_code = dr["employee_code"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string gender = dr["gender"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string role_name = dr["role_name"].ToString();
                    string profile_photo = dr["profile_photo"].ToString();
                    string role_id =  dr["role_id"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name =  dr["branch_name"].ToString();
                    float salary = (float)Convert.ToDouble(dr["salary"].ToString());
                    st = new EmployeeModel()
                    {
                        employee_id = id,
                        birth_date = birth_date,
                        email_address = email_address,
                        employee_code = employee_code,
                        employee_name = employee_name,
                        joining_date = joining_date,
                        mobile_number = mobile_number,
                        role_id = role_id,
                        role_name = role_name,
                        salary = salary,
                        profile_photo = profile_photo,
                        qualification = qualification,
                        gender = gender,
                         branch_name = branch_name,
                          branch_id = branch_id
                           
                    };
                     
                }
                con.Close();
            }
            return  st;
        }

        public async Task<EmployeeModel> GetEmployeeByUserId(string user_id)
        {
            EmployeeModel st = null;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblemployee_by_user_id", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_id", user_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    DateTime joining_date = Convert.ToDateTime(dr["joining_date"].ToString());
                    int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    string employee_code = dr["employee_code"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string gender = dr["gender"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string role_name = dr["role_name"].ToString();
                    string profile_photo = dr["profile_photo"].ToString();
                    string role_id =  dr["role_id"].ToString();
                    float salary = (float)Convert.ToDouble(dr["salary"].ToString());
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString(); st = new EmployeeModel()
                    {
                        employee_id = employee_id,
                        birth_date = birth_date,
                        email_address = email_address,
                        employee_code = employee_code,
                        employee_name = employee_name,
                        joining_date = joining_date,
                        mobile_number = mobile_number,
                        role_id = role_id,
                        role_name = role_name,
                        salary = salary,
                        profile_photo = profile_photo,
                        qualification = qualification,
                        gender = gender,
                         branch_id = branch_id,
                          branch_name = branch_name,
                    };

                }
                con.Close();
            }
            return   st;
        }

        public async Task<List<EmployeeModel>> GetEmployees(int branch_id)
        {
            List<EmployeeModel> lst = new List<EmployeeModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblemployees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["employee_id"].ToString());
                    EmployeeModel emp =await GetEmployeeWiseRoles(id);
                    List<RoleModel> roles =  emp.roles;
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    DateTime joining_date = Convert.ToDateTime(dr["joining_date"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    string employee_code = dr["employee_code"].ToString();
                    string gender = dr["gender"].ToString();
                    string qualification = dr["qualification"].ToString(); string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string role_name = dr["role_name"].ToString();
                    string profile_photo = dr["profile_photo"].ToString();
                    string role_id =  dr["role_id"].ToString();
                    float salary = (float)Convert.ToDouble(dr["salary"].ToString());
                   // int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    EmployeeModel e = new EmployeeModel()
                    {
                        employee_id = id,
                        birth_date = birth_date,
                        email_address = email_address,
                        employee_code = employee_code,
                        employee_name = employee_name,
                        joining_date = joining_date,
                        mobile_number = mobile_number,
                        role_id = role_id,
                        role_name = role_name,
                        salary = salary,
                         profile_photo=profile_photo,
                          gender=gender,
                           qualification=qualification,
                            roles= roles,
                             branch_id=branch_id,
                              branch_name=branch_name,
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task< EmployeeModel> GetEmployeeWiseRoles(int employee_id)
        {
            List<RoleModel> lst = new List<RoleModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_employee_wise_roles", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", employee_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string employee_name = dr["employee_name"].ToString();
                    string employee_code = dr["employee_code"].ToString();
                    int employee_role_id = Convert.ToInt32(dr["employee_role_id"].ToString());
                    string role_name = dr["role_name"].ToString();
                    string role_id =  dr["role_id"].ToString();


                    RoleModel r = new RoleModel()
                    {
                        employee_id = employee_id,

                        role_id = role_id,
                        role_name = role_name,
                        employee_name = employee_name,
                        employee_role_id = employee_role_id
                         
                    };
                    lst.Add(r);
                }
                con.Close();
            }
            EmployeeModel emp =await GetEmployee(employee_id);
            emp.roles= lst;
            return emp;
        }

        public List<RoleModel> GetRoleWiseEmployees(string role_id)
        {
            List<RoleModel> lst = new List<RoleModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_role_wise_employees", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@role_id", role_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    string employee_code = dr["employee_code"].ToString();
                    int employee_role_id = Convert.ToInt32(dr["employee_role_id"].ToString());
                    string role_name = dr["role_name"].ToString();


                    RoleModel r = new RoleModel()
                    {
                        employee_id = employee_id,

                        role_id = role_id,
                        role_name = role_name,
                        employee_name = employee_name,
                        employee_role_id = employee_role_id
                    };
                    lst.Add(r);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<TrainerTopicModel>> GetTrainerWiseTopics(int employee_id)
        {
            List<TrainerTopicModel> lst = new List<TrainerTopicModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_trainer_wise_topics", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", employee_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int trainer_topic_d = Convert.ToInt32(dr["trainer_topic_id"].ToString());
                    //int employee_id = Convert.ToInt32(dr["employee_id"].ToString());
                    string employee_name = dr["employee_name"].ToString();
                    int topic_id = Convert.ToInt32(dr["topic_id"].ToString());
                    string topic_name = dr["topic_name"].ToString();

                    TrainerTopicModel e = new TrainerTopicModel()
                    {
                        trainer_topic_id = trainer_topic_d,
                        employee_id = employee_id,
                        employee_name = employee_name,
                        topic_id = topic_id,
                        topic_name = topic_name
                    };
                    
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<int> IsInRole(int employee_id, string role_id)
        {
            int status;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select erpuser.IsEmployeeInRole("+employee_id+","+role_id+")", con);
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                status = (int)cmd.ExecuteScalar();
                
            }
            return   status;
        }

        public async Task<string> NextEmployeeCode()
        {
            string empcode = "";
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select dbo.fun_nextemployeecode()", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    empcode = dr[0].ToString();
                }
                con.Close();
            }
            return empcode;
        }

        //public async Task RestoreEmployeeRole(int employee_role_id)
        //{
        //    using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("sp_employee_role", con);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@type", "Restore");
        //        cmd.Parameters.AddWithValue("@employee_role_id", employee_role_id);
        //        cmd.Parameters.AddWithValue("@employee_id", 0);
        //        cmd.Parameters.AddWithValue("@role_id",0);

        //        SqlDataReader dr = cmd.ExecuteReader();
        //    }
        //}

        public async Task UpdateEmployeeDetails(EmployeeModel employee)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblemployees_modify", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employee_id", employee.employee_id);
                cmd.Parameters.AddWithValue("@employee_name", employee.employee_name);
                cmd.Parameters.AddWithValue("@email_address", employee.email_address);
                cmd.Parameters.AddWithValue("@qualification", employee.qualification);
                cmd.Parameters.AddWithValue("@gender", employee.gender);
                cmd.Parameters.AddWithValue("@mobile_number", employee.mobile_number);
                cmd.Parameters.AddWithValue("@birth_date", employee.birth_date);
                SqlDataReader dr = cmd.ExecuteReader();
            }
        }

        public async Task UpdateEmployeeRole(RoleModel role)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_employee_role", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@employee_role_id", role.employee_role_id);
                cmd.Parameters.AddWithValue("@employee_id", role.employee_id);
                cmd.Parameters.AddWithValue("@role_id", role.role_id);

                SqlDataReader dr = cmd.ExecuteReader();
            }
        }
    }
}