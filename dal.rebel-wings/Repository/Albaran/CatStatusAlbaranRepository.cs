using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Albaran;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Albaran;

public class CatStatusAlbaranRepository : GenericRepository<CatStatusAlbaran>, ICatStatusAlbaranRepository
{
    public CatStatusAlbaranRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}