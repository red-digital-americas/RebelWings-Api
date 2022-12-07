using biz.rebel_wings.Repository.TicketTable;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.TicketTable;

public class TicketTableRepository : GenericRepository<biz.rebel_wings.Entities.TicketTable>, ITicketTableRepository
{
    public TicketTableRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}