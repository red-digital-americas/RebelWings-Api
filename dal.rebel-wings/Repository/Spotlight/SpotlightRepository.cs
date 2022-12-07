using biz.rebel_wings.Repository.Spotlight;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Spotlight;

public class SpotlightRepository : GenericRepository<biz.rebel_wings.Entities.Spotlight>, ISpotlightRepository
{
    public SpotlightRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}