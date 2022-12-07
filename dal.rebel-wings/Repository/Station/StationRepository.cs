using biz.rebel_wings.Repository.Station;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Station;

public class StationRepository : GenericRepository<biz.rebel_wings.Entities.Station>, IStationRepository
{
    public StationRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}