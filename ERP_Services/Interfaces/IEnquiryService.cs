using ERP_Models;

namespace ERP_Services.Interfaces
{
    public interface IEnquiryService
    {
        Task AddLeads(List<CollegeLeadModel> leads);
        Task< List<CollegeLeadModel>> GetLeads();

        Task< int> AddEnquiry(EnquiryModel enquiry);
        Task AddEnquiryFollowup(EnquiryFollowupModel m);
        Task SendPromotionalMessage(EnquiryPromotionModel m);
        Task< List<EnquiryModel>> GetEnquiries();
        Task<EnquiryModel> GetEnquiry(int enquiry_id);
        Task< List<EnquiryFollowupModel>> GetEnquiryFollowups();
        Task< List<EnquiryFollowupModel>> GetEnquiryWiseFollowups(int enquiry_id);
        Task<List<EnquiryPromotionModel>> GetEnquiryPromotions();
        Task<List<PromotionalMessageModel>> GetPromotionMessages();
        Task<PromotionalMessageModel> GetPromotionMessage(int message_id);

    }
}
