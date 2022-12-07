using biz.rebel_wings.Repository.Spotlight;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Spotlight;

public class PhotoSpotlightRepository : GenericRepository<biz.rebel_wings.Entities.PhotoSpotlight>, IPhotoSpotlightRepository
{
  public PhotoSpotlightRepository(Db_Rebel_WingsContext context) : base(context)
  {
  }
}
