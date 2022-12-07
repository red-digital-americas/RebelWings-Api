using biz.rebel_wings.Repository.BanosMatutino;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.BanosMatutino
{
  public class BanosMatutinoRepository : GenericRepository<biz.rebel_wings.Entities.BanosMatutino>, IBanosMatutinoRepository
  {
    public BanosMatutinoRepository(Db_Rebel_WingsContext context) : base(context)
    {

    }
  }
}
