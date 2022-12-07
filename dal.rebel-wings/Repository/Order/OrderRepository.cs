using biz.rebel_wings.Repository.Order;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Order;

public class OrderRepository : GenericRepository<biz.rebel_wings.Entities.Order>, IOrderRepository
{
    public OrderRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }

    public async Task<bool> RemovePhoto(int id)
    {
        bool isSuccess = false;
        var p = _context.PhotoOrders.FirstOrDefault(f => f.Id == id);
        if (p != null)
        {
            _context.PhotoOrders.Remove(p);
            await SaveAsync();
        }

        return isSuccess;
    }
}