using biz.rebel_wings.Repository.DrinksTemperature;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.DrinksTemperature;

public class PhotoDrinksTemperatureRepository : GenericRepository<PhotoDrinksTemperature>, IPhotoDrinksTemperatureRepository
{
    public PhotoDrinksTemperatureRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}