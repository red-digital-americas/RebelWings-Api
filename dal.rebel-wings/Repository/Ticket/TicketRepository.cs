using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Ticket;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Ticket;

public class TicketRepository : GenericRepository<biz.rebel_wings.Entities.Ticket>, ITicketRepository
{
    private ITicketRepository _ticketRepositoryImplementation;

    public TicketRepository(Db_Rebel_WingsContext context) : base(context)
    {
        
    }

    public List<CatStatusTicket> getStatusTickets()
    {
        return _context.CatStatusTickets.ToList();
    }

    public List<CatPartBranch> getPartBranches()
    {
        return _context.CatPartBranches.ToList();
    }

    public List<CatSpecificSection> getCatSpecificSections()
    {
        return _context.CatSpecificSections.ToList();
    }
}