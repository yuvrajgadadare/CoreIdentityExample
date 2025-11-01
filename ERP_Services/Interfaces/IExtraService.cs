
using ERP_Models;
using System.Data;

namespace ERP_Services.Interfaces
{
    public interface IExtraService
    {
        Task SendEmail(EmailModel email, EmailSettings _settings);
        Task< string> SendTransactionalSMS(string mobile_number, string message);
        Task <string> Encrypt(string clearText);
        Task< string> Decrypt(string cipherText);
        Task<string> GetRandomPassword(int length);
        Task<string> ConvertAmount(double amount);
        Task<string> ConvertAmountInWord(long amount);
        Task<string> GenerateOTP(int size);
        Task<List<int>> GetYears();
        DataTable ReadExcelFiles(string filePath);

    }
}