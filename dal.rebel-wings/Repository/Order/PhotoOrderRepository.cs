using biz.rebel_wings.Repository.Order;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Order;

public class PhotoOrderRepository : GenericRepository<PhotoOrder>, IPhotoOrderRepository
{
    public PhotoOrderRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}