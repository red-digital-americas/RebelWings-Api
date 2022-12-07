using biz.rebel_wings.Repository.Generic;
using biz.rebel_wings.Repository.SatisfactionSurvey;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.SatisfactionSurvey;

public class SatisfactionSurveyRepository : GenericRepository<biz.rebel_wings.Entities.SatisfactionSurvey>, ISatisfactionSurveyRepository
{
    public SatisfactionSurveyRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}