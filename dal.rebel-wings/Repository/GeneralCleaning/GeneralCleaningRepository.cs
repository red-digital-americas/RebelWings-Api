using biz.rebel_wings.Repository.GeneralCleaning;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.GeneralCleaning;

public class GeneralCleaningRepository : GenericRepository<biz.rebel_wings.Entities.GeneralCleaning>, IGeneralCleaningRepository
{
    public GeneralCleaningRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}