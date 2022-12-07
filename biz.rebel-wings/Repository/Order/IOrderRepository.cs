using biz.rebel_wings.Repository.Generic;

namespace biz.rebel_wings.Repository.Order;

public interface IOrderRepository : IGenericRepository<Entities.Order>
{
    Task<bool> RemovePhoto(int id);
}