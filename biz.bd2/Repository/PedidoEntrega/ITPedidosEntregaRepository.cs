using biz.bd2.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.bd2.Repository.PedidoEntrega
{
    public interface ITPedidosEntregaRepository : IGenericRepository<biz.bd2.Entities.TPedidosEntrega>
    {
        string GetProveedor(int idProveedor);
    }
}
