using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.PrecookedChicken;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.PrecookedChicken;

public class PhotoPrecookedChickenRepository : GenericRepository<PhotoPrecookedChicken>, IPhotoPrecookedChickenRepository
{
    public PhotoPrecookedChickenRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}