using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Ticket;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Ticket;

public class PhotoTicketRepository : GenericRepository<PhotoTicket>, IPhotoTicketRepository
{
    public PhotoTicketRepository(Db_Rebel_WingsContext context) : base(context)
    {
        
    }
}