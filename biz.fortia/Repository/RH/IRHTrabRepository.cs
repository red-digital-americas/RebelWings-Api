using biz.fortia.Models;
using biz.fortia.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.fortia.Repository.RH
{
    public interface IRHTrabRepository : IGenericRepository<Entities.RhTrab>
    {
        string GetBranchName(int clab_trab);
        string GetBranchNameById(int branchId);
        int GetBranchId(int clab_trab);
        List<AttendanceList> GetAttendanceLists(int branch);
        List<TransferList> GetBranchList();
    }
}
