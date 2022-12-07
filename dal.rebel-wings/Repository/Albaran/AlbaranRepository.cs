using biz.rebel_wings.Repository.Albaran;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Albaran;

public class AlbaranesRepository : GenericRepository<biz.rebel_wings.Entities.Albaran>, IAlbaranesRepository
{
    public AlbaranesRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}