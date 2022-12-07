using biz.rebel_wings.Repository.Salon;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Salon;

public class SalonRepository : GenericRepository<biz.rebel_wings.Entities.Salon>, ISalonRepository
{
    public SalonRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}