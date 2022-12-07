using biz.rebel_wings.Repository.ValidateAttendance;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.ValidateAttendance;

public class PhotoValidateAttendanceRepository : GenericRepository<biz.rebel_wings.Entities.PhotoValidateAttendance>, IPhotoValidateAttendanceRepository
{
  public PhotoValidateAttendanceRepository(Db_Rebel_WingsContext context) : base(context)
  {
  }
}
