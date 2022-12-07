using biz.rebel_wings.Repository.FridgeSalon;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.FridgeSalon;

public class FridgeSalonRepository : GenericRepository<biz.rebel_wings.Entities.FridgeSalon>, IFridgeSalonRepository
{
    public FridgeSalonRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}