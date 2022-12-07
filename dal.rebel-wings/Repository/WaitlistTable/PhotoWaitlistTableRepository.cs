using biz.rebel_wings.Repository.WaitlistTable;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.WaitlistTable;

public class PhotoWaitlistTableRepository : GenericRepository<biz.rebel_wings.Entities.PhotoWaitlistTable>, IPhotoWaitlistTableRepository
{
    public PhotoWaitlistTableRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}
