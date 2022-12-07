using biz.rebel_wings.Repository.Permission;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.Permission
{
    public class PermissionRepository : GenericRepository<biz.rebel_wings.Entities.Permission>, IPermissionRepository
    {
        public PermissionRepository(Db_Rebel_WingsContext context) : base(context) { }
    }
}
