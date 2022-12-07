using biz.bd1.Repository.PedidoEntrega;
using dal.bd1.DBContext;
using dal.bd1.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.bd1.Repository.PedidoEntrega
{
    public class TEstatusPedidosEntregaRepository : GenericRepository<biz.bd1.Entities.TEstatusPedidosEntrega>, ITEstatusPedidosEntregaRepository
    {
        public TEstatusPedidosEntregaRepository(BD1Context context) : base(context)
        {

        }
    }
}
