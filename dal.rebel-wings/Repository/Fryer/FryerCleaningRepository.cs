using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Fryer;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Fryer;

public class FryerCleaningRepository : GenericRepository<FryerCleaning>, IFryerCleaningRepository
{
    public FryerCleaningRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}