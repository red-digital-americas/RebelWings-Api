using biz.rebel_wings.Repository.Bathroom;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Bathroom;

public class PhotoBathroomRepository : GenericRepository<biz.rebel_wings.Entities.PhotoBathroom>, IPhotoBathroomRepository
{
    public PhotoBathroomRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}