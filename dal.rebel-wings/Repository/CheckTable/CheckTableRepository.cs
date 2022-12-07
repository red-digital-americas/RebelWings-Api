using biz.rebel_wings.Repository.CheckTable;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.CheckTable;

public class CheckTableRepository : GenericRepository<biz.rebel_wings.Entities.CheckTable>, ICheckTableRepository
{
    public CheckTableRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}