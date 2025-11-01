using ERP_Models;
using ERP_Services.Interfaces;
using ExcelDataReader;
using MailKit.Net.Smtp;
 
using MimeKit;
using System.Data;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ERP_Services.Implementations
{
    public class ExtraService : IExtraService
    {
        //EmailSettings _settings;
        //public ExtraService(IOptions<EmailSettings> settings)
        //{
        //    _settings = settings.Value;
        //}
        public async Task<List<int>> GetYears()
        {
            List<int> lst = new List<int>();
            int current_year = DateTime.Now.Year;
            for (int i = current_year; i >= 2016; i--)
            {
                lst.Add(i);
            }
            return lst;
        }
        public async Task SendEmail(EmailModel email,EmailSettings _settings)
        {
            MimeMessage message = new MimeMessage();
            MailboxAddress emailfrom = new MailboxAddress(_settings.Name, _settings.EmailId);
            MailboxAddress emailto = new MailboxAddress(email.UserName, email.EmailAddress);
            message.From.Add(emailfrom);
            message.To.Add(emailto);
            message.Subject = email.Subject;
            BodyBuilder body = new BodyBuilder();
            //body.TextBody = email.Message;
            body.HtmlBody = email.Message;
            message.Body = body.ToMessageBody();
            SmtpClient smtp = new SmtpClient();
            smtp.Timeout = 2000000;
            smtp.Connect(_settings.Host, _settings.Port, _settings.UseSSL);
            smtp.Authenticate(_settings.EmailId, _settings.Password);
            smtp.Send(message);
            smtp.Disconnect(true);
            smtp.Dispose();
        }

        public async Task<string> SendTransactionalSMS(string mobile_number, string message)
        {
            Uri targetUri = new Uri("https://manage.hivemsg.com/api/send_transactional_sms.php?username=u7539&msg_token=V2n4dJ&sender_id=CIITST&message=" + message + "&mobile=" + mobile_number + "");
            HttpWebRequest webRequest = (HttpWebRequest)System.Net.HttpWebRequest.Create(targetUri);
            webRequest.Method = WebRequestMethods.Http.Get;
            try
            {
                string webResponse = string.Empty;
                using (HttpWebResponse getresponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader reader = new
                    StreamReader(getresponse.GetResponseStream()))
                    {
                        webResponse = reader.ReadToEnd();
                        reader.Close();
                    }
                    getresponse.Close();
                }
                return webResponse;
            }
            catch (System.Net.WebException ex)
            {
                return "Request-Timeout";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }


        public async Task<string> Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                    clearText = clearText.Replace("=", "");
                    clearText = clearText.Replace("+", "-");
                }
            }

            return clearText;
        }

        public async Task<string> Decrypt(string cipherText)
        {
            cipherText = cipherText + "==";
            cipherText = cipherText.Replace("-", "+");
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }


        public async Task<string> GetRandomPassword(int length)
        {

            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }


        private static String[] units = { "Zero", "One", "Two", "Three",
    "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
    "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
    "Seventeen", "Eighteen", "Nineteen" };
        private static String[] tens = { "", "", "Twenty", "Thirty", "Forty",
    "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        public async Task<String> ConvertAmount(double amount)
        {
            try
            {
                Int64 amount_int = (Int64)amount;
                Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
                if (amount_dec == 0)
                {
                    return ConvertAmountInWord(amount_int) + " Only.";
                }
                else
                {
                    return ConvertAmountInWord(amount_int) + " Point " + ConvertAmountInWord(amount_dec) + " Only.";
                }
            }
            catch (Exception e)
            {
                // TODO: handle exception  
            }
            return "";
        }

        public async Task<String> ConvertAmountInWord(Int64 i)
        {
            if (i < 20)
            {
                return units[i];
            }
            if (i < 100)
            {
                return tens[i / 10] + ((i % 10 > 0) ? " " + ConvertAmountInWord(i % 10) : "");
            }
            if (i < 1000)
            {
                return units[i / 100] + " Hundred"
                        + ((i % 100 > 0) ? " And " + ConvertAmountInWord(i % 100) : "");
            }
            if (i < 100000)
            {
                return ConvertAmountInWord(i / 1000) + " Thousand "
                        + ((i % 1000 > 0) ? " " + ConvertAmountInWord(i % 1000) : "");
            }
            if (i < 10000000)
            {
                return ConvertAmountInWord(i / 100000) + " Lakh "
                        + ((i % 100000 > 0) ? " " + ConvertAmountInWord(i % 100000) : "");
            }
            if (i < 1000000000)
            {
                return ConvertAmountInWord(i / 10000000) + " Crore "
                        + ((i % 10000000 > 0) ? " " + ConvertAmountInWord(i % 10000000) : "");
            }
            return ConvertAmountInWord(i / 1000000000) + " Arab "
                    + ((i % 1000000000 > 0) ? " " + ConvertAmountInWord(i % 1000000000) : "");
        }

        public async Task<string> GenerateOTP(int size)
        {
            const string chars = "0123456789";

            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < size; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }

        public DataTable ReadExcelFiles(string file)
        {


            using (var stream = System.IO.File.Open(file, FileMode.Open, FileAccess.Read))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var encoding = Encoding.GetEncoding("UTF-8");
                using (var reader = ExcelReaderFactory.CreateReader(stream,
                  new ExcelReaderConfiguration() { FallbackEncoding = encoding }))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                    });

                    if (result.Tables.Count > 0)
                    {
                        return result.Tables[0];
                    }
                }
            }
            return null;
        }
    }
}
