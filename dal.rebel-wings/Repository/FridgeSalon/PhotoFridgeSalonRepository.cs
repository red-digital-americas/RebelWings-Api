using biz.rebel_wings.Repository.FridgeSalon;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.FridgeSalon;

public class PhotoFridgeSalonRepository : GenericRepository<PhotoFridgeSalon>, IPhotoFridgeSalonRepository
{
    public PhotoFridgeSalonRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}