using biz.rebel_wings.Repository.Stock;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Stock;

public class StockRepository : GenericRepository<biz.rebel_wings.Entities.Inventario>, IStockRepository
{
    public StockRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}