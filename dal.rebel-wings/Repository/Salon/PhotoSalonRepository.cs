using biz.rebel_wings.Repository.Salon;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Salon;

public class PhotoSalonRepository : GenericRepository<biz.rebel_wings.Entities.PhotoSalon>, IPhotoSalonRepository
{
    public PhotoSalonRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}