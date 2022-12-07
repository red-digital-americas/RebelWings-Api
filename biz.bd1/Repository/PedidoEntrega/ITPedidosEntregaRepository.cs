using biz.bd1.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.bd1.Repository.PedidoEntrega
{
    public interface ITPedidosEntregaRepository : IGenericRepository<biz.bd1.Entities.TPedidosEntrega>
    {
        string GetProveedor(int idProveedor);
    }
}
