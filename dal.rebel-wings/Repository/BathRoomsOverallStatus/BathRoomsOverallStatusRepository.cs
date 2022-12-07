using biz.rebel_wings.Repository.BathRoomsOverallStatus;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.BathRoomsOverallStatus;

public class BathRoomsOverallStatusRepository : GenericRepository<biz.rebel_wings.Entities.BathRoomsOverallStatus>, IBathRoomsOverallStatusRepository
{
    public BathRoomsOverallStatusRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}