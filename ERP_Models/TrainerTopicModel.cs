using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSystem_Models
{
    public class TrainerTopicModel
    {
        public int trainer_topic_id {  get; set; }
        public int employee_id {  get; set; }
        public  string employee_name {  get; set; }
        public int topic_id { get; set; }
        public string topic_name { get; set; }
        public string topics { get; set; }
    }
}
