using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Fridge;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Fridge;

public class PhotoFridgeRepository : GenericRepository<PhotoFridge>, IPhotoFridgeRepository
{
    public PhotoFridgeRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}