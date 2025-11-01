
namespace ERP_Models
{
    public class EnquiryFollowupModel
    {
        public int enquiry_id { get; set; }
        public DateTime enquiry_date { get; set; }
        public string candidate_name { get; set; }
        public string gender { get; set; }
        public string local_address { get; set; }
        public string email_address { get; set; }
        public string mobile_number { get; set; }
        public DateTime birth_date { get; set; }
        public string qualification { get; set; }
        public string lead_sources { get; set; }
        public string enquiry_fors { get; set; }
        public string interested_topics { get; set; }
        public string status { get; set; }
        public int branch_id { get; set; }
        public string branch_name { get; set; }
        public int followup_id { get; set; }
        public string follow_up_by { get; set; }
        public DateTime follow_up_date { get; set; }
        public string description { get; set; }

    }
}
