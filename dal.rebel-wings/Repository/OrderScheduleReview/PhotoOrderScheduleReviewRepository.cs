using biz.rebel_wings.Repository.OrderScheduleReview;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.OrderScheduleReview;

public class PhotoOrderScheduleReviewRepository : GenericRepository<biz.rebel_wings.Entities.PhotoOrderScheduleReview>, IPhotoOrderScheduleReviewRepository
{
    public PhotoOrderScheduleReviewRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}