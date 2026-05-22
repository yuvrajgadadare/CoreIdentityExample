using ERP_Models;
namespace ERP_Services.Interfaces
{
    public interface IStudentService
    {
        Task<string> NextPINNumber();
        Task ChangeStudentCourse(RegistrationModel r);

        Task<string> AddStudentRegistration(StudentModel sm);
        Task AddStudentPaymentSchedule(int registration_id, float registration_amount);
        Task UpdateStudentDetails(StudentModel sm);
        Task<bool> IsEmailExist(string email_address);
        Task<bool> IsMobileExist(string mobile_number);
        Task<List<StudentModel>> GetYearAndBranchWiseStudents(int branch_id,int year);
        Task<List<StudentModel>>  GetStudents(int branch_id);
        Task<List<StudentModel>> GetYearWiseStudents(int year, int branch_id);
        Task<List<StudentModel>>  GetAllStudents(int branch_id);
        Task<List<StudentModel>>  GetGuestStudents(int branch_id);
        Task<StudentModel>  GetStudent(int id);
        Task<List<RegistrationModel>> GetStudentWiseRegistrations(int student_id);
        Task<List<RegistrationModel>> GetAllRegistrations(int branch_id);
        Task<List<RegistrationModel>> GetAllGuestRegistrations(int branch_id);
        Task<RegistrationModel> GetGuestRegistration(int id);
        Task<RegistrationModel> GetRegistration(int id);
        Task<List<StudentPaymentModel>> GetStudentPayments(int branch_id);
        Task<StudentPaymentModel> GetStudentPayment(int payment_id);
        Task<List<StudentPaymentModel>> GetStudentsWiseRemainingPayments(int registration_id);
        Task<StudentPaymentModel> GetStudentsNextPaymentDetails(int registration_id);
        Task<List<StudentPaymentModel>> GetStudentWisePreviousPayments(int registration_id, int payment_id);
        Task<List<StudentPaymentModel>> GetStudentWisePayments(int student_id);
        Task<List<StudentPaymentModel>> GetRegistrationWisePayments(int registration_id);

        Task AddPayment(StudentPaymentModel p);
        Task UpdatePayment(StudentPaymentModel p);
        Task AddQualification(StudentQualificationModel p);
        Task UpdateQualification(StudentQualificationModel p);
        Task DeleteQualification(int qualification_id);
        Task RestoreQualification(int qualification_id);
        Task<List<StudentQualificationModel>> GetStudentWiseQualifications(int student_id);
        Task ChangeStudentProfilePhoto(int student_id, string imgname);
        Task ChangeStudentAadharPhoto(int student_id, string imgname);
        Task ChangeStudentPassword(int student_id, string password);
        Task<StudentModel> GetStudentByEmailAddress(string email_address);
        //Task<List<StudentPaymentModel>> GetStudentsWiseRemainingPayments(int registration_id);
     Task<RegistrationCourseScheduleModel> GetStudentRegistrationWiseCourseSchedule(int registration_id);
        Task<string> AddGuestStudentRegistration(GuestRegistrationModel sm);
    }
}
