using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class CollegeLeadModel
    {
       public int leadId { get; set; }
        public string qualification { get; set; }
        public string collegename  { get; set; }
        public string studentname  { get; set; }
        public string mothername { get; set; }
        public string emailaddress { get; set; }
        public string mobilenumber { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
    }
}
