using biz.rebel_wings.Repository.TabletSafeKeeping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.TabletSafeKeeping
{
    public class TabletSafeKeepingRepository : Generic.GenericRepository<biz.rebel_wings.Entities.TabletSafeKeeping>, ITabletSafeKeepingRepository
    {
        public TabletSafeKeepingRepository(DBContext.Db_Rebel_WingsContext context) : base(context) { }
    }
}
