using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Generic;

namespace biz.rebel_wings.Repository.Ticket;

public interface ITicketRepository : IGenericRepository<Entities.Ticket>
{
    List<CatStatusTicket> getStatusTickets();
    List<CatPartBranch> getPartBranches();
    List<CatSpecificSection> getCatSpecificSections();
}