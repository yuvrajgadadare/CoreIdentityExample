using ERP_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Interfaces
{
    public interface IInvoiceService
    {
        Task AddCustomer(CustomerModel customer);
        Task UpdateCustomer(CustomerModel customer);
        Task DeleteCustomer(int customer_id);
        Task< List<CustomerModel>> GetAllCustomers();
        Task< CustomerModel> GetCustomer(int customer_id);

        Task AddProduct(ProductModel product);
        Task UpdateProduct(ProductModel product);
        Task DeleteProduct(int product_id);
        Task<List<ProductModel>> GetAllProducts();
        Task<ProductModel> GetProduct(int product_id);
    }
}
