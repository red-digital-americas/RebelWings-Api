using biz.rebel_wings.Repository.Bar;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Bar;

public class BarRepository : GenericRepository<biz.rebel_wings.Entities.Bar>, IBarRepository
{
    public BarRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}