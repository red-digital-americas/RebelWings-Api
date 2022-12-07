using biz.rebel_wings.Repository.WaitlistTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.WaitlistTable
{
    public class WaitlistTableRepository : Generic.GenericRepository<biz.rebel_wings.Entities.WaitlistTable>, IWaitlistTableRepository
    {
        public WaitlistTableRepository(DBContext.Db_Rebel_WingsContext context) : base(context)
        {

        }
    }
}
