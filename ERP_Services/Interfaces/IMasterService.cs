using ERP_Models;

namespace ERP_Services.Interfaces
{
    public interface IMasterService
    {

        Task< List<LeadSourceModel>> GetLeadSources();
        Task<List<EnquiryForModel>> GetEnquiryFors();
        Task< List<PromotionalMessageModel>> GetPromotionalMessages();
        Task<List<QualificationModel>>  GetQualifications();
        Task< StudentModel> CheckStudentLogin(string email_address,string password);
        Task< List<RoleModel>> GetAllRoles();
        Task< RoleModel>  GetRole(string role_id);
    }
}
