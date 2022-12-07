using biz.rebel_wings.Repository.Tip;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Tip;

public class PhotoTipRepository : GenericRepository<biz.rebel_wings.Entities.PhotoTip>, IPhotoTipRepository
{
    public PhotoTipRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}