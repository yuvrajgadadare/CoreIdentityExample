using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class TopicScheduleModel
    {
        public int topic_id {  get; set; }
        public string topic_name { get; set; }
        public string status {  get; set; }
        public List<ContentScheduleModel> contents { get; set; }
        public BatchModel batch { get; set; }
    }
}
