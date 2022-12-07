using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.CompleteProductsInOrder;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.CompleteProductsInOrder;

public class PhotoCompleteProductsInOrderRepository : GenericRepository<PhotoCompleteProductsInOrder>, IPhotoCompleteProductsInOrderRepository
{
    public PhotoCompleteProductsInOrderRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}