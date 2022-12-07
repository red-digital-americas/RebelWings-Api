using biz.rebel_wings.Repository.Tip;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.LivingRoomBathroomCleaning;

public class PhotoLivingRoomBathroomCleaningRepository : GenericRepository<biz.rebel_wings.Entities.PhotoLivingRoomBathroomCleaning>, IPhotoLivingRoomBathroomCleaningRepository
{
    public PhotoLivingRoomBathroomCleaningRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}