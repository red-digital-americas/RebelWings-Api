using biz.rebel_wings.Repository.BathRoomsOverallStatus;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.BathRoomsOverallStatus;

public class PhotoBathRoomsOverallStatusRepository : GenericRepository<biz.rebel_wings.Entities.PhotoBathRoomsOverallStatus>, IPhotoBathRoomsOverallStatusRepository
{
  public PhotoBathRoomsOverallStatusRepository(Db_Rebel_WingsContext context) : base(context)
  {
  }
}
