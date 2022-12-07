using biz.rebel_wings.Repository.RiskProduct;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.RiskProduct
{
    public class RiskProductRepository : GenericRepository<biz.rebel_wings.Entities.RiskProduct>, IRiskProductRepository
    {
        public RiskProductRepository(Db_Rebel_WingsContext context) : base(context)
        {
            
        }
    }
}