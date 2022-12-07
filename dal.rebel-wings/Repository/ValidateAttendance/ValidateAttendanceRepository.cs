using biz.rebel_wings.Repository.ValidateAttendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.ValidateAttendance
{
    public class ValidateAttendanceRepository : Generic.GenericRepository<biz.rebel_wings.Entities.ValidateAttendance>, IValidateAttendanceRepository
    {
        public ValidateAttendanceRepository(DBContext.Db_Rebel_WingsContext context) : base(context)
        {

        }
    }
}
