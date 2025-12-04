namespace ERP_Models
{
    public class StudentModel
    {
        public int student_id { get; set; }
        public string student_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public string mobile_number { get; set; }
        public string whatsapp_number { get; set; }

        public string email_address { get; set; }
        public string local_address { get; set; }
        public string permanent_address { get; set; }
        public DateTime birth_date { get; set; }
        public string profile_photo { get; set; }
        public string password { get; set; }
        public string qualification { get; set; }                                           
        public List<RegistrationModel> registrations { get; set; }
        public List<StudentPaymentModel> payments { get; set; }
        public string parent_name{ get; set; }
        public string parent_number { get; set; }
        public string student_code {  get; set; }
        public string permanent_identification_number {  get; set; }
        public string aadhar_card_number {  get; set; }
        public string aadhar_card_photo {  get; set; }
        public int registration_id { get; set; }
        public DateTime registration_date { get; set; }
        public int branch_id { get; set; }
        public string? branch_name { get; set; }
        public List<StudentQualificationModel> qualifications { get; set; }
    }
}
