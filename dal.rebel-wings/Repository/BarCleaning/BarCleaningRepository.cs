using biz.rebel_wings.Repository.BarCleaning;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.BarCleaning;

public class BarCleaningRepository : GenericRepository<biz.rebel_wings.Entities.BarCleaning>, IBarCleaningRepository
{
    public BarCleaningRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}