using biz.rebel_wings.Repository.WashBasinWithSoapPaper;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.WashBasinWithSoapPaper;

public class WashBasinWithSoapPaperRepository : GenericRepository<biz.rebel_wings.Entities.WashBasinWithSoapPaper>, IWashBasinWithSoapPaperRepository
{
    public WashBasinWithSoapPaperRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}