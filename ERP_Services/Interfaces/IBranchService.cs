using ERP_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Interfaces
{
    public interface IBranchService
    {
        Task<List<BranchModel>> GetBranches();
        Task<List<BranchModel>> GetAllBranches();

    }
}
