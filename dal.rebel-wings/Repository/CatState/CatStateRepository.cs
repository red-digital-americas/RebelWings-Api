using biz.rebel_wings.Repository.CatState;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.CatState
{
    public class CatStateRepository : GenericRepository<biz.rebel_wings.Entities.CatState>, ICatStateRepository
    {
        public CatStateRepository(Db_Rebel_WingsContext context) : base(context)
        {
        }
    }
}
