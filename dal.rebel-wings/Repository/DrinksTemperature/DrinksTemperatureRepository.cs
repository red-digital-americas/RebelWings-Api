using biz.rebel_wings.Repository.DrinksTemperature;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.DrinksTemperature;

public class DrinksTemperatureRepository : GenericRepository<biz.rebel_wings.Entities.DrinksTemperature>, IDrinksTemperatureRepository 
{
    public DrinksTemperatureRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}