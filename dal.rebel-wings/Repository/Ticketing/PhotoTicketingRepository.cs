using biz.rebel_wings.Repository.Ticketing;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Ticketing;

public class PhotoTicketingRepository : GenericRepository<biz.rebel_wings.Entities.PhotoTicketing>, IPhotoTicketingRepository
{
    public PhotoTicketingRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}