using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.SalesExpectations;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.SalesExpectations;

public class StockChickenByBranchRepository : GenericRepository<StockChickenByBranch>, IStockChickenByBranchRepository
{
    public StockChickenByBranchRepository(Db_Rebel_WingsContext context) : base(context)
    {
        
    }
}