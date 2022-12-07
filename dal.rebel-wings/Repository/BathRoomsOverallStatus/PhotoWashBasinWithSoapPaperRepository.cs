using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.BathRoomsOverallStatus;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.BathRoomsOverallStatus;

public class PhotoWashBasinWithSoapPaperRepository : GenericRepository<PhotoWashBasinWithSoapPaper>, IPhotoWashBasinWithSoapPaperRepository
{
    public PhotoWashBasinWithSoapPaperRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}