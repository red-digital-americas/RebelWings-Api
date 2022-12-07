using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.RequestTransfer;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.RequestTransfer;

public class CatAmountRepository : GenericRepository<CatAmount>, ICatAmountRepository
{
    public CatAmountRepository(Db_Rebel_WingsContext context) : base(context)
    {
        
    }
}