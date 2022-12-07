using biz.rebel_wings.Repository.ToSetTable;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.ToSetTable;

public class PhotoToSetTableRepository : GenericRepository<biz.rebel_wings.Entities.PhotoToSetTable>, IPhotoToSetTableRepository
{
    public PhotoToSetTableRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}