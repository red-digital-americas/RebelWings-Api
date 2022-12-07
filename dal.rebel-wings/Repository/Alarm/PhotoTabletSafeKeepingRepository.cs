using biz.rebel_wings.Repository.Alarm;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Alarm;

public class PhotoTabletSafeKeepingRepository : GenericRepository<biz.rebel_wings.Entities.PhotoTabletSageKeeping>, IPhotoTabletSafeKeepingRepository
{
    public PhotoTabletSafeKeepingRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}