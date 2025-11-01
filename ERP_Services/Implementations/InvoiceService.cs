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
    public class InvoiceService : IInvoiceService
    {
        public async Task AddCustomer(CustomerModel customer)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("invoice.sp_tblcustomer", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@customer_id", customer.customer_id);
                cmd.Parameters.AddWithValue("@customer_name", customer.customer_name);
                cmd.Parameters.AddWithValue("@mobile_number", customer.mobile_number);
                cmd.Parameters.AddWithValue("@email_address", customer.email_address);
                cmd.Parameters.AddWithValue("@city", customer.city);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task AddProduct(ProductModel product)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("invoice.sp_tblproduct", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Insert");
                cmd.Parameters.AddWithValue("@product_id", product.product_id);
                cmd.Parameters.AddWithValue("@product_name", product.product_name);
                cmd.Parameters.AddWithValue("@rate", product.rate);
                cmd.Parameters.AddWithValue("@gst", product.gst);
                cmd.Parameters.AddWithValue("@stock_quantity", product.stock_quantity);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task DeleteCustomer(int customer_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("invoice.sp_tblcustomer", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                cmd.Parameters.AddWithValue("@customer_name", "");
                cmd.Parameters.AddWithValue("@mobile_number", "");
                cmd.Parameters.AddWithValue("@email_address", "");
                cmd.Parameters.AddWithValue("@city", "");
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task DeleteProduct(int product_id)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("invoice.sp_tblproduct", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Delete");
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@product_name", "");
                cmd.Parameters.AddWithValue("@rate",0);
                cmd.Parameters.AddWithValue("@gst",0);
                cmd.Parameters.AddWithValue("@stock_quantity", 00);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task<List<CustomerModel>> GetAllCustomers()
        {
            List<CustomerModel> lst = new List<CustomerModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("invoice.sp_fetch_tblcustomers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@customer_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                 int   customer_id = Convert.ToInt32(dr["customer_id"].ToString());

                    string customer_name = dr["customer_name"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string city = dr["city"].ToString();
                    lst.Add(new CustomerModel()
                    {
                        customer_id = customer_id,
                        customer_name = customer_name,
                        city = city,
                        email_address = email_address,
                        mobile_number = mobile_number
                    });


                }
                con.Close();
            }
            return lst;
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            List<ProductModel> lst = new List<ProductModel>();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("invoice.sp_fetch_tblproducts", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@product_id", 0);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int product_id = Convert.ToInt32(dr["product_id"].ToString());

                    string product_name = dr["product_name"].ToString();
                    float rate =(float)Convert.ToDouble( dr["rate"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    int stock_quantity =Convert.ToInt32( dr["stock_quantity"].ToString());
                    lst.Add(new ProductModel()
                    {
                        product_id = product_id,
                         gst = gst,
                          product_name =product_name,
                           rate = rate,
                            stock_quantity = stock_quantity
                                
                    });


                }
                con.Close();
            }
            return lst;
        }

        public async Task<CustomerModel> GetCustomer(int customer_id)
        {
            CustomerModel  st = new CustomerModel();
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("invoice.tblcustomers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                      customer_id = Convert.ToInt32(dr["customer_id"].ToString());
 
                    string customer_name = dr["customer_name"].ToString();
                    string mobile_number = dr["mobile_number"].ToString();
                    string email_address = dr["email_address"].ToString();
                    string city = dr["city"].ToString();
                    st=new CustomerModel() { 
                     customer_id=customer_id,
                      customer_name=customer_name,
                       city=city,
                        email_address=email_address,
                         mobile_number=mobile_number
                    };


                }
                con.Close();
            }
            return st;
        }

        public async Task<ProductModel> GetProduct(int product_id)
        {
            ProductModel st = null;
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("invoice.sp_fetch_tblproducts", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@product_id", product_id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                      product_id = Convert.ToInt32(dr["product_id"].ToString());

                    string product_name = dr["product_name"].ToString();
                    float rate = (float)Convert.ToDouble(dr["rate"].ToString());
                    float gst = (float)Convert.ToDouble(dr["gst"].ToString());
                    int stock_quantity = Convert.ToInt32(dr["stock_quantity"].ToString());
                    st = new ProductModel()
                    {
                        product_id = product_id,
                        gst = gst,
                        product_name = product_name,
                        rate = rate,
                        stock_quantity = stock_quantity

                    };


                }
                con.Close();
            }
            return  st;
        }

        public async Task UpdateCustomer(CustomerModel customer)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("invoice.sp_tblcustomer", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@customer_id", customer.customer_id);
                cmd.Parameters.AddWithValue("@customer_name", customer.customer_name);
                cmd.Parameters.AddWithValue("@mobile_number", customer.mobile_number);
                cmd.Parameters.AddWithValue("@email_address", customer.email_address);
                cmd.Parameters.AddWithValue("@city", customer.city);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public async Task UpdateProduct(ProductModel product)
        {
            using (SqlConnection con = new SqlConnection(DatabaseOperations.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("invoice.sp_tblproduct", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type", "Update");
                cmd.Parameters.AddWithValue("@product_id", product.product_id);
                cmd.Parameters.AddWithValue("@product_name", product.product_name);
                cmd.Parameters.AddWithValue("@rate", product.rate);
                cmd.Parameters.AddWithValue("@gst", product.gst);
                cmd.Parameters.AddWithValue("@stock_quantity", product.stock_quantity);
                int cnt = cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
