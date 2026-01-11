using ERP_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Interfaces
{
    public interface ILeadSource
    {
        Task AddLead(LeadModel lead);
        Task<List<LeadModel>> GetLeads();
        Task<LeadModel> GetLead(int id);
    }
}
