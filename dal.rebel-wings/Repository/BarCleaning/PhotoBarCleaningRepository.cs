using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.BarCleaning;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.BarCleaning;

public class PhotoBarCleaningRepository : GenericRepository<PhotoBarCleaning>, IPhotoBarCleaningRepository
{
    public PhotoBarCleaningRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}