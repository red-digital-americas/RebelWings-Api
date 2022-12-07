using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.CatStatusSalesExpectations;
using dal.rebel_wings.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.CatStatusSalesExpectations
{
    public class CatStatusSalesExpectationRepository : GenericRepository<CatStatusStockChicken>, ICatStatusStockChickenRepository
    {
        public CatStatusSalesExpectationRepository(DBContext.Db_Rebel_WingsContext context) : base(context)
        {

        }
    }
}
