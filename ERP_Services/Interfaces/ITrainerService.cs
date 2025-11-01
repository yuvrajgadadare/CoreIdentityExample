using ERP_Models;

namespace ERP_Services.Interfaces
{
    public interface ITrainerService
    {
        Task<EmployeeModel> CheckEmployeeLogin(string email_address, string password);
    }
}
