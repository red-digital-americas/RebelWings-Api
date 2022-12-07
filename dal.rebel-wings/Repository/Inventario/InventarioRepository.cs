using biz.rebel_wings.Repository.Inventario;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Inventario;

public class InventarioRepository : GenericRepository<biz.rebel_wings.Entities.Inventario>, IInventarioRepository
{
  public InventarioRepository(Db_Rebel_WingsContext context) : base(context)
  {
  }
}
