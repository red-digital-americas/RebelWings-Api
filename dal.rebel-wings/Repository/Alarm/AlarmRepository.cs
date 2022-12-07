using biz.rebel_wings.Repository.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.Alarm
{
    public class AlarmRepository : Generic.GenericRepository<biz.rebel_wings.Entities.Alarm>, IAlarmRepository
    {
        public AlarmRepository(DBContext.Db_Rebel_WingsContext context) : base(context) { }
    }
}
