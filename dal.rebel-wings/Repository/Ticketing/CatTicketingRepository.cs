using biz.rebel_wings.Repository.Ticketing;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Ticketing;

public class CatTicketingRepository : GenericRepository<biz.rebel_wings.Entities.CatTicketing>, ICatTicketingRepository
{
    public CatTicketingRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}