using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Station;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Station;

public class PhotoStationRepository : GenericRepository<PhotoStation>, IPhotoStationRepository
{
    public PhotoStationRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}