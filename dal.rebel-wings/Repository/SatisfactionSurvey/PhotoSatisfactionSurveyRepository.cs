using biz.rebel_wings.Repository.SatisfactionSurvey;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.SatisfactionSurvey;

public class PhotoSatisfactionSurveyRepository : GenericRepository<biz.rebel_wings.Entities.PhotoSatisfactionSurvey>, IPhotoSatisfactionSurveyRepository
{
  public PhotoSatisfactionSurveyRepository(Db_Rebel_WingsContext context) : base(context)
  {
  }
}
