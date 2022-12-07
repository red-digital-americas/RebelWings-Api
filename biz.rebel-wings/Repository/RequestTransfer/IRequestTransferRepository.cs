using biz.rebel_wings.Repository.Generic;

namespace biz.rebel_wings.Repository.RequestTransfer;

public interface IRequestTransferRepository : IGenericRepository<Entities.RequestTransfer>
{
    List<Entities.CatStatusRequestTransfer> GetStatus();
}