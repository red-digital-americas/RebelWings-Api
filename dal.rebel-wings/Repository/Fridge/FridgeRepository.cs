using biz.rebel_wings.Repository.Fridge;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Fridge;

public class FridgeRepository : GenericRepository<biz.rebel_wings.Entities.Fridge>, IFridgeRepository
{
    public FridgeRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}