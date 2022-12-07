using biz.rebel_wings.Repository.Kitchen;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Kitchen;

public class PhotoKitchenRepository : GenericRepository<biz.rebel_wings.Entities.PhotoKitchen>, IPhotoKitchenRepository
{
    public PhotoKitchenRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}