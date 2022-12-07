using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.RequestTransfer;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.RequestTransfer;

public class RequestTransferRepository : GenericRepository<biz.rebel_wings.Entities.RequestTransfer>, IRequestTransferRepository
{
    public RequestTransferRepository(Db_Rebel_WingsContext context) : base(context)
    {
        
    }

    public List<CatStatusRequestTransfer> GetStatus()
    {
        return _context.CatStatusRequestTransfers.ToList();
    }
}