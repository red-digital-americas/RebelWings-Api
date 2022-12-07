using biz.rebel_wings.Repository.EntriesChargedAsDeliveryNote;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.EntriesChargedAsDeliveryNote;

public class EntriesChargedAsDeliveryNoteRepository : GenericRepository<biz.rebel_wings.Entities.EntriesChargedAsDeliveryNote>, IEntriesChargedAsDeliveryNoteRepository
{
    public EntriesChargedAsDeliveryNoteRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}