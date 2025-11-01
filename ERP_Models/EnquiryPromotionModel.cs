namespace ERP_Models
{
    public class EnquiryPromotionModel
    {
        public int enquiry_id { get; set; }
        public DateTime enquiry_date { get; set; }
        public string candidate_name { get; set; }
        public string gender { get; set; }
        public  int promotion_id{ get; set; }
        public int message_id { get; set; }
        public string message_title { get; set; }
        public string message  { get; set; }
        public DateTime sent_time { get; set; }
    }
}
