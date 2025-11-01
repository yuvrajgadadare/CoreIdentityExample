using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class RegistrationCourseScheduleModel
    {
        public RegistrationModel Registration {  get; set; }
        public List<TopicScheduleModel> Topics { get; set; }
    }
}
