using biz.rebel_wings.Repository.ToSetTable;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.ToSetTable
{
    public class ToSetTableRepository : GenericRepository<biz.rebel_wings.Entities.ToSetTable>, IToSetTableRepository
    {
        public ToSetTableRepository(Db_Rebel_WingsContext context) : base(context)
        {

        }
    }
}
