using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Fryer;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Fryer;

public class PhotoFryerCleaningRepository : GenericRepository<PhotoFryerCleaning>, IPhotoFryerCleaningRepository
{
    public PhotoFryerCleaningRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}