using biz.rebel_wings.Repository.Tip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.Tip
{
    public class TipRepository : Generic.GenericRepository<biz.rebel_wings.Entities.Tip>, ITipRepository
    {
        public TipRepository(DBContext.Db_Rebel_WingsContext context) : base(context) { }
    }
}
