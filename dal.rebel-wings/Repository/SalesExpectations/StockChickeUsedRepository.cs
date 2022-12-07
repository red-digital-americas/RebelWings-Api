using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.SalesExpectations;
using dal.rebel_wings.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.SalesExpectations
{
    public class StockChickeUsedRepository : GenericRepository<StockChickeUsed>, IStockChickeUsedRepository
    {
        public StockChickeUsedRepository(DBContext.Db_Rebel_WingsContext context) : base(context)
        {

        }
    }
}
