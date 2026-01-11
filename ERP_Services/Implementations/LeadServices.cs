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
    public class LeadServices : ILeadSource
    {
        public async Task AddLead(LeadModel lead)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_add_lead", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@candidate_name", lead.candidate_name);
                    cmd.Parameters.AddWithValue("@email_address", lead.email_address);
                    cmd.Parameters.AddWithValue("@mobile_number", lead.mobile_number);
                    cmd.Parameters.AddWithValue("@training_type", lead.training_type);
                    cmd.Parameters.AddWithValue("@description", lead.description);
                    cmd.Parameters.AddWithValue("@lead_date", lead.lead_date);
                    int cnt = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch(Exception ex)
            {
               string msg=ex.Message;
            }
        }

    

        public async Task<LeadModel> GetLead(int id)
        {
             LeadModel st =null;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_leads", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@lead_id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int lead_id = Convert.ToInt32(dr["lead_id"].ToString());

                    string candidate_name = dr["candidate_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string training_type = dr["training_type"].ToString();
                    string description = dr["description"].ToString();
                    DateTime lead_date = Convert.ToDateTime(dr["lead_date"].ToString());
                   st = new LeadModel()
                    {
                        candidate_name = candidate_name,
                        description = description,
                        lead_date = lead_date,
                        email_address = email_address,
                        lead_id = lead_id,
                        mobile_number = mobile_number,
                        training_type = training_type
                    };
                    
                }
                con.Close();
            }
            return  st;
        }

       
        public async Task<List<LeadModel>> GetLeads()
        {
            List<LeadModel> lst = new List<LeadModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_leads", con);
          
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int lead_id = Convert.ToInt32(dr["lead_id"].ToString());
                     
                    string candidate_name = dr["candidate_name"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string training_type = dr["training_type"].ToString();
                    string description = dr["description"].ToString();
                    DateTime lead_date = Convert.ToDateTime(dr["lead_date"].ToString());
                    LeadModel e = new LeadModel()
                    {
                        candidate_name = candidate_name,
                        description = description,
                        lead_date = lead_date,
                        email_address = email_address,
                        lead_id = lead_id,
                        mobile_number = mobile_number,
                        training_type = training_type
                    };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
    }
}
