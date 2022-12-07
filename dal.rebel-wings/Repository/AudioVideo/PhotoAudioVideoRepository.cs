using biz.rebel_wings.Repository.AudioVideo;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.AudioVideo;

public class PhotoAudioVideoRepository : GenericRepository<biz.rebel_wings.Entities.PhotoAudioVideo>, IPhotoAudioVideoRepository
{
  public PhotoAudioVideoRepository(Db_Rebel_WingsContext context) : base(context)
  {
  }
}
