using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.GeneralCleaning;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.GeneralCleaning;

public class PhotoGeneralCleaningRepository : GenericRepository<PhotoGeneralCleaning>, IPhotoGeneralCleaningRepository
{
    public PhotoGeneralCleaningRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}