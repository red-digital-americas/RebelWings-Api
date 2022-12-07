using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Alarm;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Alarm;

public class PhotoAlarmRepository : GenericRepository<PhotoAlarm>, IPhotoAlarmRepository
{
    public PhotoAlarmRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}