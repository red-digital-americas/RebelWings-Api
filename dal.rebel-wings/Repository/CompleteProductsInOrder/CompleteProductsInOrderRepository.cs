using biz.rebel_wings.Repository.CompleteProductsInOrder;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.CompleteProductsInOrder;

public class CompleteProductsInOrderRepository : GenericRepository<biz.rebel_wings.Entities.CompleteProductsInOrder>, ICompleteProductsInOrderRepository
{
    public CompleteProductsInOrderRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}