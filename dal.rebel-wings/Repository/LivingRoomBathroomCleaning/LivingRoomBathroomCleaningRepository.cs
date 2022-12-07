using biz.rebel_wings.Repository.Tip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.LivingRoomBathroomCleaning
{
    public class LivingRoomBathroomCleaningRepository : Generic.GenericRepository<biz.rebel_wings.Entities.LivingRoomBathroomCleaning>, ILivingRoomBathroomCleaningRepository
    {
        public LivingRoomBathroomCleaningRepository(DBContext.Db_Rebel_WingsContext context) : base(context) { }
    }
}
