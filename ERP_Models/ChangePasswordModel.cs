using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class ChangePasswordModel
    {
        public string current_password {  get; set; }
        public string new_password {  get; set; }
        public string re_password {  get; set; }
    }
}
