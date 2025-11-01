using ERP_Models;
using ERPSystem_Models;
namespace ERP_Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeModel>> GetEmployees();
        //List<TrainerModel> GetTrainers();
        Task< string> NextEmployeeCode();
        Task< EmployeeModel>  GetEmployee(int id);
        //Task< EmployeeModel>  CheckEmployeeLogin(string employee_code,string password);
        Task AddEmployeeDetails(EmployeeModel employee);
        //void AddTrainerDetails(EmployeeModel trainer);
        Task UpdateEmployeeDetails(EmployeeModel employee);
        //Task ChangeEmployeeDetailPassword(EmployeeModel employee);
        Task ChangeProfilePhoto(EmployeeModel employee);
        Task AddEmployeeRole(RoleModel role);
        Task UpdateEmployeeRole(RoleModel role);
        Task DeleteEmployeeRole(int employee_role_id);
        //Task RestoreEmployeeRole(int employee_role_id);
       Task< EmployeeModel> GetEmployeeWiseRoles(int employee_id);
        List<RoleModel> GetRoleWiseEmployees(string role_id);

        Task AddTrainerTopics(List<TrainerTopicModel> topics);
        Task< List<TrainerTopicModel>> GetTrainerWiseTopics(int employee_id);

        Task<int> IsInRole(int employee_id, string role_id);

        Task<EmployeeModel> GetEmployeeByUserId(string user_id);
    }
}