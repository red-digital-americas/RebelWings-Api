using biz.rebel_wings.Repository.PrecookedChicken;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.PrecookedChicken;

public class PrecookedChickenRepository : GenericRepository<biz.rebel_wings.Entities.PrecookedChicken>, IPrecookedChickenRepository
{
    public PrecookedChickenRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}