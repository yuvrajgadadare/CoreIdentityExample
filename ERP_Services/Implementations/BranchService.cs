using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Implementations
{
    public class BranchService:IBranchService
    {
        public async Task<List<BranchModel>> GetBranches()
        {
            List<BranchModel> lst = new List<BranchModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblbranches", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["branch_id"].ToString());
                    string name = dr["branch_name"].ToString();
                    BranchModel e = new BranchModel() { branch_id = id, branch_name = name };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }
        public async Task<List<BranchModel>> GetAllBranches()
        {
            List<BranchModel> lst = new List<BranchModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_fetch_tblbranches", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["branch_id"].ToString());
                    string name = dr["branch_name"].ToString();
                    BranchModel e = new BranchModel() { branch_id = id, branch_name = name };
                    lst.Add(e);
                }
                con.Close();
            }
            return lst;
        }

    }
}
