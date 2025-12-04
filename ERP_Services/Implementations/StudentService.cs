using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace ERP_Services.Implementations
{
    public class StudentService : IStudentService
    {

        IContentService contentService;
        ITopicService topicService;
        IBatchService batchService;

        public StudentService(ITopicService topicService, IContentService contentService, IBatchService batchService)
        {

            this.topicService = topicService;
            this.contentService = contentService;
            this.batchService = batchService;


        }
        public async Task AddPayment(StudentPaymentModel p)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@payment_id", p.payment_id);
                cmd.Parameters.AddWithValue("@registration_id", p.registration_id);
                //cmd.Parameters.AddWithValue("@expected_payment_date", p.expected_payment_date);
                cmd.Parameters.AddWithValue("@payment_date", p.payment_date);
                cmd.Parameters.AddWithValue("@payment_amount", p.payment_amount);
                cmd.Parameters.AddWithValue("@payment_mode", p.payment_mode);
                cmd.Parameters.AddWithValue("@payment_description", p.payment_description);
                cmd.Parameters.AddWithValue("@is_paid", p.is_paid);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task UpdatePayment(StudentPaymentModel p)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@payment_id", p.payment_id);
                cmd.Parameters.AddWithValue("@registration_id", p.registration_id);
                //cmd.Parameters.AddWithValue("@expected_payment_date", p.expected_payment_date);
                cmd.Parameters.AddWithValue("@payment_date", p.payment_date);
                cmd.Parameters.AddWithValue("@payment_amount", p.payment_amount);
                cmd.Parameters.AddWithValue("@payment_mode", p.payment_mode);
                cmd.Parameters.AddWithValue("@payment_description", p.payment_description);
                cmd.Parameters.AddWithValue("@is_paid", p.is_paid);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task AddStudentRegistration(StudentModel sm)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@student_id", sm.student_id);
                cmd.Parameters.AddWithValue("@branch_id", sm.branch_id);
                cmd.Parameters.AddWithValue("@student_name", sm.student_name);
                cmd.Parameters.AddWithValue("@last_name", sm.last_name);
                cmd.Parameters.AddWithValue("@gender", sm.gender);
                cmd.Parameters.AddWithValue("@mobile_number",sm.mobile_number);
                cmd.Parameters.AddWithValue("@whatsapp_number", sm.whatsapp_number);
                cmd.Parameters.AddWithValue("@email_address", sm.email_address);
                cmd.Parameters.AddWithValue("@local_address", sm.local_address);
                cmd.Parameters.AddWithValue("@permanent_address", sm.permanent_address);
                cmd.Parameters.AddWithValue("@password", sm.password);
                cmd.Parameters.AddWithValue("@birth_date", sm.birth_date);
                cmd.Parameters.AddWithValue("@profile_photo", "");
              //  cmd.Parameters.AddWithValue("@qualification", sm.qualification);
                cmd.Parameters.AddWithValue("@parent_name", sm.parent_name);
                cmd.Parameters.AddWithValue("@parent_number", sm.parent_number);
                cmd.Parameters.AddWithValue("@student_code", sm.student_code);
                cmd.Parameters.AddWithValue("@permanent_identification_number", sm.permanent_identification_number);
                cmd.Parameters.AddWithValue("@aadhar_card_number", sm.aadhar_card_number);
                cmd.Parameters.AddWithValue("@aadhar_card_photo", "");
                cmd.Parameters.AddWithValue("@id", 0);

                DataTable dt = new DataTable();
                dt.Columns.Add("registration_date", typeof(DateTime));
                dt.Columns.Add("discount", typeof(float));
                dt.Columns.Add("fee_id", typeof(int));
                foreach(RegistrationModel r in sm.registrations)
                {
                    dt.Rows.Add(r.registration_date,r.discount,r.fee_id);
                }
                cmd.Parameters.AddWithValue("@registration", dt);

                DataTable qdt = new DataTable();
                qdt.Columns.Add("qualification", typeof(string));
                qdt.Columns.Add("passing_year", typeof(int));
                qdt.Columns.Add("university", typeof(string));
                qdt.Columns.Add("medium", typeof(string));
                qdt.Columns.Add("percentage", typeof(float));
                foreach (StudentQualificationModel q in sm.qualifications)
                {
                    qdt.Rows.Add(q.qualification, q.passing_year, q.university, q.medium, q.percentage);
                }
                cmd.Parameters.AddWithValue("@qualification", qdt);

                DataTable pdt = new DataTable();
                pdt.Columns.Add("payment_date", typeof(DateTime));
                pdt.Columns.Add("payment_amount", typeof(float));
                pdt.Columns.Add("payment_mode", typeof(string));
                pdt.Columns.Add("payment_description", typeof(string));
                pdt.Columns.Add("is_paid", typeof(int));
                foreach (StudentPaymentModel q in sm.payments)
                {
                    pdt.Rows.Add(q.payment_date, q.payment_amount, q.payment_mode, q.payment_description, 1);
                }
                cmd.Parameters.AddWithValue("@payment", pdt);
                try
                {
                    int cnt = cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    string msg = ex.Message;
                }
                con.Close();
            }
        }
        public async Task UpdateStudentDetails(StudentModel sm)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@student_id", sm.student_id);
                cmd.Parameters.AddWithValue("@branch_id", sm.branch_id);
                cmd.Parameters.AddWithValue("@student_name", sm.student_name);
                cmd.Parameters.AddWithValue("@last_name", sm.last_name);
                cmd.Parameters.AddWithValue("@gender", sm.gender);
                cmd.Parameters.AddWithValue("@mobile_number", sm.mobile_number);
                cmd.Parameters.AddWithValue("@whatsapp_number", sm.whatsapp_number);
                cmd.Parameters.AddWithValue("@email_address", sm.email_address);
                cmd.Parameters.AddWithValue("@local_address", sm.local_address);
                cmd.Parameters.AddWithValue("@permanent_address", sm.permanent_address);
                cmd.Parameters.AddWithValue("@password", "");
                cmd.Parameters.AddWithValue("@birth_date", sm.birth_date);
                cmd.Parameters.AddWithValue("@profile_photo","");
                cmd.Parameters.AddWithValue("@qualification", sm.qualification);
                cmd.Parameters.AddWithValue("@parent_name", sm.parent_name);
                cmd.Parameters.AddWithValue("@parent_number", sm.parent_number);
                cmd.Parameters.AddWithValue("@student_code","");
                cmd.Parameters.AddWithValue("@permanent_identification_number", "");
                cmd.Parameters.AddWithValue("@aadhar_card_number", sm.aadhar_card_number);
                cmd.Parameters.AddWithValue("@aadhar_card_photo", "");
                cmd.Parameters.AddWithValue("@id", 0);

                DataTable dt = new DataTable();
                dt.Columns.Add("registration_date", typeof(DateTime));
                dt.Columns.Add("discount", typeof(float));
                dt.Columns.Add("fee_id", typeof(int));
     

                cmd.Parameters.AddWithValue("@registration", dt);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<StudentPaymentModel>> GetRegistrationWisePayments(int registration_id)
        {
            List<StudentPaymentModel> lst = new List<StudentPaymentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_registration_wise_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    //DateTime expected_payment_date = Convert.ToDateTime(dr["expected_payment_date"].ToString());
                    DateTime payment_date=new DateTime ();
                    string pdt = dr["payment_date"].ToString();
                    if (pdt != "")
                    {
                        payment_date = Convert.ToDateTime(pdt);
                    }

                    float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    string payment_mode = dr["payment_mode"].ToString();
                    string payment_description = dr["payment_description"].ToString();
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();

                    StudentPaymentModel e = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                        payment_amount = payment_amount,
                        //expected_payment_date= expected_payment_date,
                        payment_date = payment_date,
                        payment_description = payment_description,
                        payment_id = payment_id,
                        payment_mode = payment_mode,
                        student_name = student_name,
                         branch_id = branch_id,
                          branch_name=branch_name
                          
                         
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<StudentPaymentModel>> GetStudentWisePayments(int student_id)
        {
            List<StudentPaymentModel> lst = new List<StudentPaymentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_student_wise_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    //DateTime expected_payment_date = Convert.ToDateTime(dr["expected_payment_date"].ToString());

                    string pdt = dr["payment_date"].ToString();

                    DateTime payment_date = Convert.ToDateTime(pdt);

                    float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    string payment_mode = dr["payment_mode"].ToString();
                    string payment_description = dr["payment_description"].ToString();
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    StudentPaymentModel e = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                        payment_amount = payment_amount,
                        //expected_payment_date = expected_payment_date,
                        payment_date = payment_date,
                        payment_description = payment_description,
                        payment_id = payment_id,
                        payment_mode = payment_mode,
                        student_name = student_name,
                         branch_name=branch_name,
                          branch_id = branch_id
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<StudentModel> GetStudent(int id)
        {
            StudentModel sd = new StudentModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_alltblstudent_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    string last_name = dr["last_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string permanent_address = dr["permanent_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string whatsapp_number = dr["whatsapp_number"].ToString();
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string parent_name = dr["parent_name"].ToString();
                    string parent_number = dr["parent_number"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string permanent_identification_number = dr["permanent_identification_number"].ToString();
                    string aadhar_card_photo = dr["aadhar_card_photo"].ToString();
                    string aadhar_card_number = dr["aadhar_card_number"].ToString();
                    List<RegistrationModel> registrations = await GetStudentWiseRegistrations(student_id);
                    //List<StudentPaymentModel>payments=GetStudentWisePayments(student_id);
                    //float paidamount = 0, remaining_amount = 0;
                    //float total_amount = registrations[0].final_fees_amount;
                    //string status = "";
                    //if (payments.Count > 0)
                    //{
                    //    paidamount = payments.Sum(e => e.payment_amount);

                    //}
                    //remaining_amount = total_amount - paidamount;
                    //if (paidamount == 0)
                    //{
                    //    status = "Un Paid";
                    //}
                    //else if(paidamount>0 && paidamount<total_amount)
                    //{
                    //    status = "Partial Paid";

                    //}
                    //else
                    //{
                    //    status = "Paid";

                    //}
                    sd = new StudentModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        birth_date = birth_date,
                        gender = gender,
                        mobile_number = mobile_number,
                        student_name = student_name,
                        profile_photo = profile_photo,
                        qualification = qualification,
                        parent_name = parent_name,
                        parent_number = parent_number,
                        student_code = student_code,
                        registrations = registrations,
                        aadhar_card_number = aadhar_card_number,
                        aadhar_card_photo = aadhar_card_photo,
                        last_name = last_name,
                        local_address =local_address,
                         permanent_address=permanent_address,
                        permanent_identification_number = permanent_identification_number,
                           whatsapp_number=whatsapp_number,
                            branch_id=branch_id,
                             branch_name = branch_name
                           
                         
                     
                    };

                }
                con.Close();
            }
            return sd;
        }
        public async Task<StudentPaymentModel>  GetStudentPayment(int payment_id)
        {
             StudentPaymentModel  st = new StudentPaymentModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@payment_id", payment_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                  //  int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    //DateTime expected_payment_date = Convert.ToDateTime(dr["expected_payment_date"].ToString());
                    DateTime payment_date = Convert.ToDateTime(dr["payment_date"].ToString());
                    float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    string payment_mode = dr["payment_mode"].ToString();
                    string payment_description = dr["payment_description"].ToString();
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    st = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                        payment_amount = payment_amount,
                        //expected_payment_date= expected_payment_date,
                        payment_date = payment_date,
                        payment_description = payment_description,
                        payment_id = payment_id,
                        payment_mode = payment_mode,
                        student_name = student_name,
                         branch_id = branch_id,
                          branch_name=branch_name
                    };
                    
                }
                con.Close();
            }
            return st;
        }

        public async Task<StudentPaymentModel> GetStudentsNextPaymentDetails(int registration_id)
        {
            StudentPaymentModel st = new StudentPaymentModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_registration_wise_next_payment", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                     int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    DateTime expected_payment_date = Convert.ToDateTime(dr["expected_payment_date"].ToString());
                    float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    st = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                        payment_amount = payment_amount,
                        //expected_payment_date = expected_payment_date,
                       // payment_description = payment_description,
                        payment_id = payment_id,
                       // payment_mode = payment_mode,
                        student_name = student_name,
                         branch_name=branch_name,
                          branch_id=branch_id
                    };

                }
                con.Close();
            }
            return st;
        }
        public async Task<List<StudentPaymentModel>> GetStudentPayments(int GetStudentPayments)
        {
            List<StudentPaymentModel> lst = new List<StudentPaymentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", GetStudentPayments);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    //DateTime expected_payment_date = Convert.ToDateTime(dr["expected_payment_date"].ToString());
                    DateTime payment_date = Convert.ToDateTime(dr["payment_date"].ToString());
                    float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    string payment_mode = dr["payment_mode"].ToString();
                    string payment_description= dr["payment_description"].ToString();
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();

                    StudentPaymentModel e = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                        payment_amount = payment_amount,
                        //expected_payment_date= expected_payment_date,
                        payment_date = payment_date,
                        payment_description = payment_description,
                        payment_id = payment_id,
                        payment_mode = payment_mode,
                        student_name = student_name,
                         branch_id = branch_id,
                          branch_name=branch_name
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<StudentPaymentModel>> GetStudentWisePreviousPayments(int registration_id, int payment_id)
        {
            List<StudentPaymentModel> lst = new List<StudentPaymentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_wise_previous_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                cmd.Parameters.AddWithValue("@payment_id", payment_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                //    int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    //DateTime expected_payment_date = Convert.ToDateTime(dr["expected_payment_date"].ToString());
                    DateTime payment_date = Convert.ToDateTime(dr["payment_date"].ToString());
                    float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    string payment_mode = dr["payment_mode"].ToString();
                    string payment_description = dr["payment_description"].ToString();
                //    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();

                    StudentPaymentModel e = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                        payment_amount = payment_amount,
                        //expected_payment_date= expected_payment_date,
                        payment_date = payment_date,
                        payment_description = payment_description,
                        payment_id = payment_id,
                        payment_mode = payment_mode,
                        student_name = student_name,
                         branch_name=branch_name,
                          branch_id=branch_id
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<StudentModel>> GetStudents(int branch_id)
        {
            List<StudentModel> lst = new List<StudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string last_name = dr["last_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string permanent_address = dr["permanent_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string whatsapp_number = dr["whatsapp_number"].ToString();
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string parent_name = dr["parent_name"].ToString();
                    string parent_number = dr["parent_number"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string permanent_identification_number = dr["permanent_identification_number"].ToString();
                    string aadhar_card_photo = dr["aadhar_card_photo"].ToString();
                    string aadhar_card_number = dr["aadhar_card_number"].ToString();
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                  //  int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();

                    DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString()); List<RegistrationModel> registrations = await GetStudentWiseRegistrations(student_id);
                        StudentModel e = new StudentModel()
                        {
                            student_id = student_id,
                            email_address = email_address,
                            birth_date = birth_date,
                            gender = gender,
                            mobile_number = mobile_number,
                            student_name = student_name,
                            profile_photo = profile_photo,
                            qualification = qualification,
                            parent_name = parent_name,
                            parent_number = parent_number,
                            student_code = student_code,
                            registrations = registrations,
                            aadhar_card_number = aadhar_card_number,
                            aadhar_card_photo = aadhar_card_photo,
                            last_name = last_name,
                            local_address = local_address,
                            permanent_address = permanent_address,
                            permanent_identification_number = permanent_identification_number,
                            whatsapp_number = whatsapp_number,
                             registration_date=registration_date,
                              registration_id=registration_id,
                               branch_id=branch_id,
                                branch_name=branch_name
                        };
                        lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<StudentModel>> GetYearWiseStudents(int year,int branch_id)
        {

            List<StudentModel> lst =   GetStudents(branch_id).Result.Where(e => e.registration_date.Year.Equals(year)).ToList();
            return lst;
        }
        public async Task<bool> IsEmailExist(string email_address)
        {
            bool status = false;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("isEmailExist", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email_address", email_address);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    status = true;
                }
            }
            return status;
        }
        public async Task<bool> IsMobileExist(string mobile_number)
        {
            bool status = false;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("ismobileExist", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mobile_number", mobile_number);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    status = true;
                }
            }
            return status;
        }
        public async Task<List<StudentModel>> GetAllStudents(int branch_id)
        {
            List<StudentModel> lst = new List<StudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_alltblstudent_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string last_name = dr["last_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string permanent_address = dr["permanent_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string whatsapp_number = dr["whatsapp_number"].ToString();
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string parent_name = dr["parent_name"].ToString();
                    string parent_number = dr["parent_number"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string permanent_identification_number = dr["permanent_identification_number"].ToString();
                    string aadhar_card_photo = dr["aadhar_card_photo"].ToString();
                    string aadhar_card_number = dr["aadhar_card_number"].ToString();
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    List<RegistrationModel> registrations = await GetStudentWiseRegistrations(student_id);
                    StudentModel e = new StudentModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        birth_date = birth_date,
                        gender = gender,
                        mobile_number = mobile_number,
                        student_name = student_name,
                        profile_photo = profile_photo,
                        qualification = qualification,
                        parent_name = parent_name,
                        parent_number = parent_number,
                        student_code = student_code,
                        registrations = registrations,
                        aadhar_card_number = aadhar_card_number,
                        aadhar_card_photo = aadhar_card_photo,
                        last_name = last_name,
                        local_address = local_address,
                        permanent_address = permanent_address,
                        permanent_identification_number = permanent_identification_number,
                        whatsapp_number = whatsapp_number,
                        registration_date = registration_date,
                        registration_id = registration_id,
                         branch_name = branch_name,
                          branch_id = branch_id
                    };
                    lst.Add(e);

                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<StudentModel>> GetGuestStudents(int branch_id)
        {
            List<StudentModel> lst = new List<StudentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_guest_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string branch_name = dr["branch_name"].ToString();
                    string last_name = dr["last_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string permanent_address = dr["permanent_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string whatsapp_number = dr["whatsapp_number"].ToString();
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string parent_name = dr["parent_name"].ToString();
                    string parent_number = dr["parent_number"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string permanent_identification_number = dr["permanent_identification_number"].ToString();
                    string aadhar_card_photo = dr["aadhar_card_photo"].ToString();
                    string aadhar_card_number = dr["aadhar_card_number"].ToString();
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());
                    List<RegistrationModel> registrations = await GetStudentWiseRegistrations(student_id);

                    StudentModel e = new StudentModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        birth_date = birth_date,
                        gender = gender,
                        mobile_number = mobile_number,
                        student_name = student_name,
                        profile_photo = profile_photo,
                        qualification = qualification,
                        parent_name = parent_name,
                        parent_number = parent_number,
                        student_code = student_code,
                        registrations = registrations,
                        aadhar_card_number = aadhar_card_number,
                        aadhar_card_photo = aadhar_card_photo,
                        last_name = last_name,
                        local_address = local_address,
                        permanent_address = permanent_address,
                        permanent_identification_number = permanent_identification_number,
                        whatsapp_number = whatsapp_number,
                        registration_date=registration_date,
                         registration_id=registration_id,
                          branch_id=branch_id,
                           branch_name=branch_name
                    };
                    lst.Add(e);

                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<RegistrationModel>> GetStudentWiseRegistrations(int student_id)
        {
            List<RegistrationModel> lst = new List<RegistrationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_studentWise_registrations", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string current_status = dr["current_status"].ToString();
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    float discount = (float)Convert.ToDouble(dr["discount"].ToString());
                    float final_fees_amount = (float)Convert.ToDouble(dr["final_fees_amount"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    List<StudentPaymentModel> payments = await GetRegistrationWisePayments(registration_id);
                    float paid_amount = 0, remaining_amount = 0;
                   
                        if (payments.Count() > 0)
                        {
                            paid_amount = payments.Sum(e => e.payment_amount);
                        }
                        remaining_amount = final_fees_amount - paid_amount;
                        string status = "";

                        if (paid_amount == 0)
                        {
                            status = "Un Paid";
                        }
                        else if (paid_amount > 0 && paid_amount < final_fees_amount)
                        {
                            status = "Partial Paid";

                        }
                        else
                        {
                            status = "Paid";

                        }
                        string fees_mode = dr["fee_mode"].ToString();
                        DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());

                        RegistrationModel e = new RegistrationModel()
                        {
                            student_id = student_id,
                            student_name = student_name,
                            course_id = course_id,
                            course_name = course_name,
                            discount = discount,
                            fees_amount = fees_amount,
                            fees_mode = fees_mode,
                            fee_id = fee_id,
                            gst = gst,
                            registration_date = registration_date,
                            registration_id = registration_id,
                            final_fees_amount = final_fees_amount,
                            paid_amount = paid_amount,
                            remaining_amount = remaining_amount,
                            fees_status = status,
                            current_status=current_status,
                               branch_id = branch_id,
                                branch_name=branch_name


                        };
                        lst.Add(e);
                    
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<RegistrationModel>> GetAllRegistrations(int branch_id)
        {
            List<RegistrationModel> lst = new List<RegistrationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_registrations", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", branch_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string branch_name = dr["branch_name"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string current_status = dr["current_status"].ToString();
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    float discount = (float)Convert.ToDouble(dr["discount"].ToString());
                    float final_fees_amount = (float)Convert.ToDouble(dr["final_fees_amount"].ToString());
                    List<StudentPaymentModel> payments = await GetRegistrationWisePayments(registration_id);
                     
                        float paid_amount = 0, remaining_amount = 0;
                        if (payments.Count() > 0)
                        {
                            paid_amount = payments.Sum(e => e.payment_amount);
                        }
                        remaining_amount = final_fees_amount - paid_amount;
                        string status = "";

                        if (paid_amount == 0)
                        {
                            status = "Un Paid";
                        }
                        else if (paid_amount > 0 && paid_amount < final_fees_amount)
                        {
                            status = "Partial Paid";

                        }
                        else
                        {
                            status = "Paid";

                        }
                        string fees_mode = dr["fee_mode"].ToString();
                        DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());

                        RegistrationModel e = new RegistrationModel()
                        {
                            student_id = student_id,
                            student_name = student_name,
                            course_id = course_id,
                            course_name = course_name,
                            discount = discount,
                            fees_amount = fees_amount,
                            fees_mode = fees_mode,
                            fee_id = fee_id,
                            gst = gst,
                            registration_date = registration_date,
                            registration_id = registration_id,
                            final_fees_amount = final_fees_amount,
                            paid_amount = paid_amount,
                            remaining_amount = remaining_amount,
                            fees_status = status,
                            student_code = student_code,
                             branch_name=branch_name,
                              branch_id = branch_id


                        };
                        lst.Add(e);
                     
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<RegistrationModel>> GetAllGuestRegistrations(int branch_id)
        {
            List<RegistrationModel> lst = new List<RegistrationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_guest_registrations", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string branch_name = dr["branch_name"].ToString();
                    string student_code = dr["student_code"].ToString();
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    float discount = (float)Convert.ToDouble(dr["discount"].ToString());
                    float final_fees_amount = (float)Convert.ToDouble(dr["final_fees_amount"].ToString());
                    List<StudentPaymentModel> payments = await GetRegistrationWisePayments(registration_id);

                    float paid_amount = 0, remaining_amount = 0;
                    if (payments.Count() > 0)
                    {
                        paid_amount = payments.Sum(e => e.payment_amount);
                    }
                    remaining_amount = final_fees_amount - paid_amount;
                    string status = "";

                    if (paid_amount == 0)
                    {
                        status = "Un Paid";
                    }
                    else if (paid_amount > 0 && paid_amount < final_fees_amount)
                    {
                        status = "Partial Paid";

                    }
                    else
                    {
                        status = "Paid";

                    }
                    string fees_mode = dr["fee_mode"].ToString();
                    DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());

                    RegistrationModel e = new RegistrationModel()
                    {
                        student_id = student_id,
                        student_name = student_name,
                        course_id = course_id,
                        course_name = course_name,
                        discount = discount,
                        fees_amount = fees_amount,
                        fees_mode = fees_mode,
                        fee_id = fee_id,
                        gst = gst,
                        registration_date = registration_date,
                        registration_id = registration_id,
                        final_fees_amount = final_fees_amount,
                        paid_amount = paid_amount,
                        remaining_amount = remaining_amount,
                        fees_status = status,
                        student_code = student_code,
                         branch_id = branch_id,
                          branch_name=branch_name


                    };
                    lst.Add(e);

                }
                con.Close();
            }
            return lst;
        }
        public async Task<RegistrationModel> GetRegistration(int id)
        {
          RegistrationModel st = new  RegistrationModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblstudent_registrations", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string current_status = dr["current_status"].ToString();
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    float discount = (float)Convert.ToDouble(dr["discount"].ToString());
                    float final_fees_amount = (float)Convert.ToDouble(dr["final_fees_amount"].ToString());
                    List<StudentPaymentModel> payments = await GetRegistrationWisePayments(registration_id);
                    float paid_amount = 0, remaining_amount = 0;
                    
                        if (payments.Count() > 0)
                        {
                            paid_amount = payments.Sum(e => e.payment_amount);
                        }
                        remaining_amount = final_fees_amount - paid_amount;
                        string status = "";

                        if (paid_amount == 0)
                        {
                            status = "Un Paid";
                        }
                        else if (paid_amount > 0 && paid_amount < final_fees_amount)
                        {
                            status = "Partial Paid";

                        }
                        else
                        {
                            status = "Paid";

                        }
                        string fees_mode = dr["fee_mode"].ToString();
                        DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());

                        st = new RegistrationModel()
                        {
                            student_id = student_id,
                            student_name = student_name,
                            course_id = course_id,
                            course_name = course_name,
                            discount = discount,
                            fees_amount = fees_amount,
                            fees_mode = fees_mode,
                            fee_id = fee_id,
                            gst = gst,
                            registration_date = registration_date,
                            registration_id = registration_id,
                            final_fees_amount = final_fees_amount,
                            paid_amount = paid_amount,
                            remaining_amount = remaining_amount,
                            fees_status = status,
                             branch_name=branch_name,
                              branch_id = branch_id


                        };
                     
                   
                }
                con.Close();
            }
            return  st;
        }

        public async Task<RegistrationModel> GetGuestRegistration(int id)
        {
            RegistrationModel st = new RegistrationModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_guest_registrations", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string current_status = dr["current_status"].ToString();
                    int fee_id = Convert.ToInt32(dr["fee_id"].ToString());
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    float fees_amount = (float)Convert.ToDouble(dr["fees_amount"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    float discount = (float)Convert.ToDouble(dr["discount"].ToString());
                    float final_fees_amount = (float)Convert.ToDouble(dr["final_fees_amount"].ToString());
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString(); List<StudentPaymentModel> payments = await GetRegistrationWisePayments(registration_id);
                    float paid_amount = 0, remaining_amount = 0;

                    if (payments.Count() > 0)
                    {
                        paid_amount = payments.Sum(e => e.payment_amount);
                    }
                    remaining_amount = final_fees_amount - paid_amount;
                    string status = "";

                    if (paid_amount == 0)
                    {
                        status = "Un Paid";
                    }
                    else if (paid_amount > 0 && paid_amount < final_fees_amount)
                    {
                        status = "Partial Paid";

                    }
                    else
                    {
                        status = "Paid";

                    }
                    string fees_mode = dr["fee_mode"].ToString();
                    DateTime registration_date = Convert.ToDateTime(dr["registration_date"].ToString());

                    st = new RegistrationModel()
                    {
                        student_id = student_id,
                        student_name = student_name,
                        course_id = course_id,
                        course_name = course_name,
                        discount = discount,
                        fees_amount = fees_amount,
                        fees_mode = fees_mode,
                        fee_id = fee_id,
                        gst = gst,
                        registration_date = registration_date,
                        registration_id = registration_id,
                        final_fees_amount = final_fees_amount,
                        paid_amount = paid_amount,
                        remaining_amount = remaining_amount,
                        fees_status = status,
                         branch_id = branch_id,
                          branch_name=branch_name


                    };


                }
                con.Close();
            }
            return st;
        }

        public async Task<string> NextPINNumber()
        {
            string pinnumber = "";
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select  erpuser.fun_nextPINNumber()", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    pinnumber = dr[0].ToString();
                }
                con.Close();
            }
            return pinnumber;
        }

        public async Task AddQualification(StudentQualificationModel p)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_qualifications", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@qualification_id", p.qualification_id);
                cmd.Parameters.AddWithValue("@student_id", p.student_id);
                cmd.Parameters.AddWithValue("@qualification", p.qualification);
                cmd.Parameters.AddWithValue("@passing_year", p.passing_year);
                cmd.Parameters.AddWithValue("@university",p.university);
                cmd.Parameters.AddWithValue("@medium", p.medium);
                cmd.Parameters.AddWithValue("@percentage",p.percentage);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task UpdateQualification(StudentQualificationModel p)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_qualifications", con);
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@qualification_id", p.qualification_id);
                cmd.Parameters.AddWithValue("@student_id", p.student_id);
                cmd.Parameters.AddWithValue("@qualification", p.qualification);
                cmd.Parameters.AddWithValue("@passing_year", p.passing_year);
                cmd.Parameters.AddWithValue("@university", p.university);
                cmd.Parameters.AddWithValue("@medium", p.medium);
                cmd.Parameters.AddWithValue("@percentage", p.percentage);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task DeleteQualification(int qualification_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_qualifications", con);
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@qualification_id", qualification_id);
                cmd.Parameters.AddWithValue("@student_id", 0);
                cmd.Parameters.AddWithValue("@qualification", "");
                cmd.Parameters.AddWithValue("@passing_year",0);
                cmd.Parameters.AddWithValue("@university", "");
                cmd.Parameters.AddWithValue("@medium", "");
                cmd.Parameters.AddWithValue("@percentage",0);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task RestoreQualification(int qualification_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblstudent_qualifications", con);
                cmd.Parameters.AddWithValue("@type", "Restore");
                cmd.Parameters.AddWithValue("@qualification_id", qualification_id);
                cmd.Parameters.AddWithValue("@student_id", 0);
                cmd.Parameters.AddWithValue("@qualification", "");
                cmd.Parameters.AddWithValue("@passing_year", 0);
                cmd.Parameters.AddWithValue("@university", "");
                cmd.Parameters.AddWithValue("@medium", "");
                cmd.Parameters.AddWithValue("@percentage", 0);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<StudentQualificationModel>> GetStudentWiseQualifications(int student_id)
        {
            List<StudentQualificationModel> lst=new List<StudentQualificationModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_Student_Wise_qualifications", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                     int qualification_id = Convert.ToInt32(dr["qualification_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string university = dr["university"].ToString();
                    string medium = dr["medium"].ToString();
                    int passing_year =Convert.ToInt32( dr["passing_year"].ToString());
                    float percentage = (float)Convert.ToDouble(dr["percentage"].ToString());
                    StudentQualificationModel e = new StudentQualificationModel()
                    {
                        qualification_id = qualification_id,
                        percentage = percentage,
                        medium = medium,
                        passing_year = passing_year,
                        student_name = student_name,
                        university = university,
                        student_id = student_id,
                        qualification = qualification,
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task ChangeStudentProfilePhoto(int student_id, string imgname)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_change_student_photo", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id",  student_id);
                cmd.Parameters.AddWithValue("@profile_photo", imgname);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task ChangeStudentAadharPhoto(int student_id, string imgname)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_change_student_aadharcardphoto", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                cmd.Parameters.AddWithValue("@aadhar_card_photo", imgname);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public async Task ChangeStudentPassword(int student_id,string password)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_change_student_password", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@student_id", student_id);
                cmd.Parameters.AddWithValue("@new_password", password);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<StudentPaymentModel>> GetStudentsWiseRemainingPayments(int registration_id)
        {
            List<StudentPaymentModel> lst = new List<StudentPaymentModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_registration_wise_pending_payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", registration_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                { 
                    int payment_id = Convert.ToInt32(dr["payment_id"].ToString());
                    //DateTime expected_payment_date = Convert.ToDateTime(dr["expected_payment_date"].ToString());
                   // DateTime payment_date = Convert.ToDateTime(dr["payment_date"].ToString());
                   float payment_amount = (float)Convert.ToDouble(dr["payment_amount"].ToString());
                    string payment_mode = dr["payment_mode"].ToString();
                    string payment_description = dr["payment_description"].ToString();
                    //    int registration_id = Convert.ToInt32(dr["registration_id"].ToString());
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    int course_id = Convert.ToInt32(dr["course_id"].ToString());
                    string course_name = dr["course_name"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString(); List<StudentPaymentModel> payments = await GetRegistrationWisePayments(registration_id);

                    StudentPaymentModel e = new StudentPaymentModel()
                    {
                        student_id = student_id,
                        registration_id = registration_id,
                        course_id = course_id,
                        course_name = course_name,
                    payment_amount = payment_amount,
                        //expected_payment_date = expected_payment_date,
                       // payment_date = payment_date,
                        payment_description = payment_description,
                        payment_id = payment_id,
                        payment_mode = payment_mode,
                        student_name = student_name,
                         branch_name=branch_name,
                          branch_id = branch_id
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

    

        public async Task AddStudentPaymentSchedule(int registration_id,float registration_amount)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_student_schedule_Payments", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id",registration_id);
                cmd.Parameters.AddWithValue("@registration_amount", registration_amount);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<StudentModel> GetStudentByEmailAddress(string email_address)
        {
            StudentModel sd = new StudentModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_student_by_email_address", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email_address", email_address);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int student_id = Convert.ToInt32(dr["student_id"].ToString());
                    string student_name = dr["student_name"].ToString();
                    string last_name = dr["last_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string permanent_address = dr["permanent_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string whatsapp_number = dr["whatsapp_number"].ToString();
                    DateTime birth_date = Convert.ToDateTime(dr["birth_date"].ToString());
                    string profile_photo = dr["profile_photo"].ToString();
                    string qualification = dr["qualification"].ToString();
                    string parent_name = dr["parent_name"].ToString();
                    string parent_number = dr["parent_number"].ToString();
                    string student_code = dr["student_code"].ToString();
                    string permanent_identification_number = dr["permanent_identification_number"].ToString();
                    string aadhar_card_photo = dr["aadhar_card_photo"].ToString();
                    string aadhar_card_number = dr["aadhar_card_number"].ToString();
                   
                    sd = new StudentModel()
                    {
                        student_id = student_id,
                        email_address = email_address,
                        birth_date = birth_date,
                        gender = gender,
                        mobile_number = mobile_number,
                        student_name = student_name,
                        profile_photo = profile_photo,
                        qualification = qualification,
                        parent_name = parent_name,
                        parent_number = parent_number,
                        student_code = student_code,
                        aadhar_card_number = aadhar_card_number,
                        aadhar_card_photo = aadhar_card_photo,
                        last_name = last_name,
                        local_address = local_address,
                        permanent_address = permanent_address,
                        permanent_identification_number = permanent_identification_number,
                        whatsapp_number = whatsapp_number
                    };
                }
                con.Close();
            }
            return sd;
        }

        public async Task ChangeStudentCourse(RegistrationModel r)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_change_student_course", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@registration_id", r.registration_id);
                cmd.Parameters.AddWithValue("@discount", r.discount);
                cmd.Parameters.AddWithValue("@fee_id", r.fee_id); 
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<RegistrationCourseScheduleModel> GetStudentRegistrationWiseCourseSchedule(int registration_id)
        {
            RegistrationCourseScheduleModel rsmodel = new RegistrationCourseScheduleModel();
            RegistrationModel r = await GetRegistration(registration_id);

            List<TopicModel> topics = await topicService.GetCourseWiseTopics(r.course_id);
            DateTime start_date = r.registration_date;
            DateTime next_date = start_date;
            List<TopicScheduleModel> topicschedulelist = new List<TopicScheduleModel>();
            foreach (TopicModel t in topics)
            {
                List<ContentScheduleModel> contents = new List<ContentScheduleModel>();
                BatchModel b = await batchService.GetRegistrationAndTopicWiseBatch(r.registration_id, t.topic_id);
             //   List<ExamModel> exams = await examService.GetRegistrationAndBatchWiseScheduledExams(b.batch_id, r.registration_id);
                int cnt = b.total_leactures;
              //  b.exams = exams;

                List<StudentMarkAttendance> attendance = await batchService.GetBatchWiseStudentAttendance(b.batch_id, r.registration_id);
                foreach (ContentModel c in await contentService.GetTopicWiseContents(t.topic_id))
                {
                    ContentScheduleModel csm = new ContentScheduleModel()
                    {
                        content_id = c.content_id,
                        content_name = c.content_name,
                        expected_date = next_date
                    };

                    if (attendance.Count() > 0)
                    {
                        StudentMarkAttendance sm = attendance.FirstOrDefault(e => e.content_id.Equals(c.content_id));
                        //actual_date = sm.actual_date;
                        if (sm != null)
                        {
                            csm.actual_date = sm.attendance_date;
                            csm.is_present = sm.is_present;
                            csm.attendance = sm.attendance;
                        }


                    }

                    next_date = next_date.AddDays(1);
                    if (next_date.DayOfWeek.ToString().ToLower().Equals("saturday"))
                    {
                        next_date = next_date.AddDays(2);
                    }
                    contents.Add(csm);

                }
                TopicScheduleModel ts = new TopicScheduleModel()
                {
                    topic_id = t.topic_id,
                    topic_name = t.topic_name,
                    contents = contents,
                    batch = b,
                    status = ""
                };
                topicschedulelist.Add(ts);
            }
            rsmodel.Registration = r;
            rsmodel.Topics = topicschedulelist;
            return rsmodel;

        }


    }
}
