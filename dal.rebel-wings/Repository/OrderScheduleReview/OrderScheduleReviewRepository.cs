using biz.rebel_wings.Repository.OrderScheduleReview;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.OrderScheduleReview;

public class OrderScheduleReviewRepository : GenericRepository<biz.rebel_wings.Entities.OrderScheduleReview>, IOrderScheduleReviewRepository
{
    public OrderScheduleReviewRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}