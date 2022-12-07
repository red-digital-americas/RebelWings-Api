using biz.rebel_wings.Repository.Role;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.Role
{
    public class RoleRepository : GenericRepository<biz.rebel_wings.Entities.CatRole> ,IRoleRepository
    {
        public RoleRepository(Db_Rebel_WingsContext context) : base(context) { }
    }
}
