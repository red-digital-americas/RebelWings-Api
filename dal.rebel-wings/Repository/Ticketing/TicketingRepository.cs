using biz.rebel_wings.Repository.Ticketing;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Ticketing;

public class TicketingRepository : GenericRepository<biz.rebel_wings.Entities.Ticketing>, ITicketingRepository
{
    public TicketingRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}