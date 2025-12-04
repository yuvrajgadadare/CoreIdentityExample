
using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ERP_Services.Implementations
{
    public class EnquiryService : IEnquiryService
    {
        IExtraService extraService;
        public EnquiryService(IExtraService extraService)
        {
            this.extraService = extraService;
        }
        public async Task<int> AddEnquiry(EnquiryModel enquiry)
        {
            int output_id = 0;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblequniries", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@enquiry_id", enquiry.enquiry_id);
                cmd.Parameters.AddWithValue("@enquiry_date", enquiry.enquiry_date);
                cmd.Parameters.AddWithValue("@candidate_name", enquiry.candidate_name);
                cmd.Parameters.AddWithValue("@gender", enquiry.gender);
                cmd.Parameters.AddWithValue("@local_address", enquiry.local_address);
                cmd.Parameters.AddWithValue("@email_address", enquiry.email_address);
                cmd.Parameters.AddWithValue("@mobile_number", enquiry.mobile_number);
                cmd.Parameters.AddWithValue("@birth_date", enquiry.birth_date);
                cmd.Parameters.AddWithValue("@qualification", enquiry.qualification);
                cmd.Parameters.AddWithValue("@lead_sources", enquiry.lead_sources);
                cmd.Parameters.AddWithValue("@enquiry_fors", enquiry.enquiry_fors);
                cmd.Parameters.AddWithValue("@interested_topics", enquiry.interested_topics);
                cmd.Parameters.AddWithValue("@status", enquiry.status);
                cmd.Parameters.AddWithValue("@branch_id", enquiry.branch_id);
               // cmd.Parameters.AddWithValue("@output_id", 0);
                cmd.Parameters.Add("@output_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                int cnt = cmd.ExecuteNonQuery();
                  output_id = Convert.ToInt32(cmd.Parameters["@output_id"].Value);
                con.Close();
            }
           // EnquiryModel em = GetEnquiry(output_id);
            return output_id;

        }

        public async Task AddEnquiryFollowup(EnquiryFollowupModel m)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblenquiry_followups", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@followup_id", m.followup_id);
                cmd.Parameters.AddWithValue("@enquiry_id", m.enquiry_id);
                cmd.Parameters.AddWithValue("@follow_up_date", m.follow_up_date);
                cmd.Parameters.AddWithValue("@follow_up_by", m.follow_up_by);
                cmd.Parameters.AddWithValue("@description", m.description);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<EnquiryModel>> GetEnquiries(int branch_id)
        {
            List<EnquiryModel> lst = new List<EnquiryModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblenquiries", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branch_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["enquiry_id"].ToString());
                    DateTime enquiry_date =Convert.ToDateTime( dr["enquiry_date"].ToString());
                    string cname = dr["candidate_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string email = dr["email_address"].ToString();
                    string mob = dr["mobile_number"].ToString();

                    DateTime bdate = Convert.ToDateTime(dr["birth_date"].ToString());

                    string qualification = dr["qualification"].ToString();
                    string lead_sources = dr["lead_sources"].ToString();
                    string enquiry_fors = dr["enquiry_fors"].ToString();
                    string interested_topics = dr["interested_topics"].ToString();
                    string status = dr["status"].ToString();
                    string branch_name = dr["branch_name"].ToString();
                    EnquiryModel e = new EnquiryModel()
                    {
                        enquiry_id = id,
                        birth_date = bdate,
                        branch_id = branch_id,
                        branch_name = branch_name,
                        candidate_name = cname,
                        email_address = email,
                        enquiry_date = enquiry_date,
                        enquiry_fors = enquiry_fors,
                        gender = gender,
                        interested_topics = interested_topics,
                        lead_sources = lead_sources,
                        local_address = local_address,
                        mobile_number = mob,
                        qualification = qualification,
                        status = status
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<EnquiryModel> GetEnquiry(int enquiry_id)
        {
             EnquiryModel  st =new  EnquiryModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblenquiries", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@enquiry_id", enquiry_id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    int id = Convert.ToInt32(dr["enquiry_id"].ToString());
                    DateTime enquiry_date = Convert.ToDateTime(dr["enquiry_date"].ToString());
                    string cname = dr["candidate_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string email = dr["email_address"].ToString();
                    string mob = dr["mobile_number"].ToString();

                    DateTime bdate = Convert.ToDateTime(dr["birth_date"].ToString());

                    string qualification = dr["qualification"].ToString();
                    string lead_sources = dr["lead_sources"].ToString();
                    string enquiry_fors = dr["enquiry_fors"].ToString();
                    string interested_topics = dr["interested_topics"].ToString();
                    string status = dr["status"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                      st = new EnquiryModel()
                    {
                        enquiry_id = id,
                        birth_date = bdate,
                        branch_id = branch_id,
                        branch_name = branch_name,
                        candidate_name = cname,
                        email_address = email,
                        enquiry_date = enquiry_date,
                        enquiry_fors = enquiry_fors,
                        gender = gender,
                        interested_topics = interested_topics,
                        lead_sources = lead_sources,
                        local_address = local_address,
                        mobile_number = mob,
                        qualification = qualification,
                        status = status
                    };
                     
                }
                con.Close();
            }
            return st;
        }

        public async Task<List<EnquiryFollowupModel>> GetEnquiryFollowups()
        {
            List<EnquiryFollowupModel> lst = new List<EnquiryFollowupModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblenquiry_followups", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
               
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["enquiry_id"].ToString());
                    DateTime enquiry_date = Convert.ToDateTime(dr["enquiry_date"].ToString());
                    string cname = dr["candidate_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string email = dr["email_address"].ToString();
                    string mob = dr["mobile_number"].ToString();
                    DateTime bdate = Convert.ToDateTime(dr["birth_date"].ToString());

                    string qualification = dr["qualification"].ToString();
                    string lead_sources = dr["lead_sources"].ToString();
                    string enquiry_fors = dr["enquiry_fors"].ToString();
                    string interested_topics = dr["interested_topics"].ToString();
                    string status = dr["status"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    int follow_up_id = Convert.ToInt32(dr["followup_id"].ToString());
                    string follow_up_by = dr["follow_up_by"].ToString();
                    DateTime follow_up_date = Convert.ToDateTime(dr["follow_up_date"].ToString());
                    string description = dr["description"].ToString();
                    EnquiryFollowupModel e = new EnquiryFollowupModel()
                    {
                        enquiry_id = id,
                        birth_date = bdate,
                        branch_id = branch_id,
                        branch_name = branch_name,
                        candidate_name = cname,
                        email_address = email,
                        enquiry_date = enquiry_date,
                        enquiry_fors = enquiry_fors,
                        gender = gender,
                        interested_topics = interested_topics,
                        lead_sources = lead_sources,
                        local_address = local_address,
                        mobile_number = mob,
                        qualification = qualification,
                        status = status,
                        description = description,
                        followup_id = follow_up_id,
                        follow_up_by = follow_up_by,
                        follow_up_date = follow_up_date
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<EnquiryFollowupModel>> GetEnquiryWiseFollowups(int enquiry_id)
        {
            List<EnquiryFollowupModel> lst = new List<EnquiryFollowupModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblenquiry_wise_followups", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@enquiry_id", enquiry_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["enquiry_id"].ToString());
                    DateTime enquiry_date = Convert.ToDateTime(dr["enquiry_date"].ToString());
                    string cname = dr["candidate_name"].ToString();
                    string gender = dr["gender"].ToString();
                    string local_address = dr["local_address"].ToString();
                    string email = dr["email_address"].ToString();
                    string mob = dr["mobile_number"].ToString();
                    DateTime bdate = Convert.ToDateTime(dr["birth_date"].ToString());

                    string qualification = dr["qualification"].ToString();
                    string lead_sources = dr["lead_sources"].ToString();
                    string enquiry_fors = dr["enquiry_fors"].ToString();
                    string interested_topics = dr["interested_topics"].ToString();
                    string status = dr["status"].ToString();
                    int branch_id = Convert.ToInt32(dr["branch_id"].ToString());
                    string branch_name = dr["branch_name"].ToString();
                    int follow_up_id = Convert.ToInt32(dr["followup_id"].ToString());
                    string follow_up_by = dr["follow_up_by"].ToString();
                    DateTime follow_up_date = Convert.ToDateTime(dr["follow_up_date"].ToString());
                    string description = dr["description"].ToString();
                    EnquiryFollowupModel e = new EnquiryFollowupModel()
                    {
                        enquiry_id = id,
                        birth_date = bdate,
                        branch_id = branch_id,
                        branch_name = branch_name,
                        candidate_name = cname,
                        email_address = email,
                        enquiry_date = enquiry_date,
                        enquiry_fors = enquiry_fors,
                        gender = gender,
                        interested_topics = interested_topics,
                        lead_sources = lead_sources,
                        local_address = local_address,
                        mobile_number = mob,
                        qualification = qualification,
                        status = status,
                        description = description,
                        followup_id = follow_up_id,
                        follow_up_by = follow_up_by,
                        follow_up_date = follow_up_date
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<EnquiryPromotionModel>> GetEnquiryPromotions()
        {
            List<EnquiryPromotionModel> lst = new List<EnquiryPromotionModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblenquirypromotions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["enquiry_id"].ToString());
                    DateTime enquiry_date = Convert.ToDateTime(dr["enquiry_date"].ToString());
                    string cname = dr["candidate_name"].ToString();
                    string gender = dr["gender"].ToString();
                     
                    int promotion_id = Convert.ToInt32(dr["promotion_id"].ToString());
                    int message_id = Convert.ToInt32(dr["message_id"].ToString());
                    string message = dr["message"].ToString();
                    string message_title = dr["message_title"].ToString();
                    DateTime sent_time = Convert.ToDateTime(dr["sent_time"].ToString());

                    EnquiryPromotionModel e = new EnquiryPromotionModel()
                    {
                        enquiry_id = id,
                        candidate_name = cname,
                        gender = gender,
                        enquiry_date = enquiry_date,
                        message = message,
                        message_id = message_id,
                        message_title = message_title,
                        promotion_id = promotion_id,
                        sent_time = sent_time
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task SendPromotionalMessage(EnquiryPromotionModel m)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblenquirypromotions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@promotion_id", m.promotion_id);
                cmd.Parameters.AddWithValue("@enquiry_id", m.enquiry_id);
                cmd.Parameters.AddWithValue("@message_id", m.message_id);
                cmd.Parameters.AddWithValue("@sent_time", m.sent_time);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<PromotionalMessageModel>> GetPromotionMessages()
        {
            List<PromotionalMessageModel> lst = new List<PromotionalMessageModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblpromotional_messages", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@message_id",0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["message_id"].ToString());
                    string message_title = dr["message_title"].ToString();
                    string message = dr["message"].ToString();
                    PromotionalMessageModel e = new PromotionalMessageModel()
                    {
                        
                        message = message,
                        message_id = id,
                        message_title = message_title
                        
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

        public async Task<PromotionalMessageModel> GetPromotionMessage(int message_id)
        {
            PromotionalMessageModel e = new PromotionalMessageModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_tblpromotional_messages", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@message_id", message_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["message_id"].ToString());
                    string message_title = dr["message_title"].ToString();
                    string message = dr["message"].ToString();
                      e = new PromotionalMessageModel()
                    {

                        message = message,
                        message_id = id,
                        message_title = message_title

                    };
                    
                }
                con.Close();
            }
            return e;
        }

        public async Task AddLeads(List<CollegeLeadModel> leads)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_add_tblcollege_leads", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                
                DataTable dt = new DataTable();
                dt.Columns.Add("qualification", typeof(string));
                dt.Columns.Add("collegename", typeof(string));
                dt.Columns.Add("studentname", typeof(string));
                dt.Columns.Add("mothername", typeof(string));
                dt.Columns.Add("emailaddress", typeof(string));
                dt.Columns.Add("mobilenumber", typeof(string));
                dt.Columns.Add("gender", typeof(string));
                dt.Columns.Add("address", typeof(string));
                dt.Columns.Add("state", typeof(string));
                dt.Columns.Add("city", typeof(string));
                dt.Columns.Add("pincode", typeof(string));
                
                foreach (CollegeLeadModel f in leads)
                {
                    dt.Rows.Add(f.qualification, f.collegename, f.studentname,f.mothername, f.emailaddress, f.mobilenumber,f.gender,f.address,f.state,f.city,f.pincode);
                }
                cmd.Parameters.AddWithValue("@lead", dt);

                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<CollegeLeadModel>> GetLeads()
        {
            List<CollegeLeadModel> lst = new List<CollegeLeadModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblcollege_leads", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int leadId = Convert.ToInt32(dr["leadId"].ToString());
                    string qualification = dr["qualification"].ToString();
                    string collegename = dr["collegename"].ToString();
                    string studentname = dr["studentname"].ToString();
                    string mothername = dr["mothername"].ToString();
                    string emailaddress = dr["emailaddress"].ToString();
                    string mobilenumber = dr["mobilenumber"].ToString();
                    string gender = dr["gender"].ToString();
                    string address = dr["address"].ToString();
                    string state = dr["state"].ToString();
                    string city = dr["city"].ToString();
                    string pincode = dr["pincode"].ToString();
                    CollegeLeadModel e = new CollegeLeadModel()
                    {

                        leadId = leadId,
                        collegename = collegename,
                        pincode = pincode,
                        state = state,
                        address = address,
                        mothername = mothername,
                        qualification = qualification,
                        city = city,
                        emailaddress = emailaddress,
                        gender = gender,
                        mobilenumber = mobilenumber,
                        studentname = studentname

                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
    }
}
