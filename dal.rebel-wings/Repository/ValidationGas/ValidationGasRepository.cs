using biz.rebel_wings.Repository.ValidationGas;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.ValidationGas
{
    public class ValidationGasRepository : GenericRepository<biz.rebel_wings.Entities.ValidationGa>, IValidationGasRepository
    {
        public ValidationGasRepository(Db_Rebel_WingsContext context) : base(context)
        {

        }
    }
}
