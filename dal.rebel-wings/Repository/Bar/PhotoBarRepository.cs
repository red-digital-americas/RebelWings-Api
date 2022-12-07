using biz.rebel_wings.Repository.Bar;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Bar;

public class PhotoBarRepository : GenericRepository<biz.rebel_wings.Entities.PhotoBar>, IPhotoBarRepository
{
    public PhotoBarRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}