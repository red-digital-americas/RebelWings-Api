using biz.bd2.Repository.PedidoEntrega;
using dal.bd2.DBContext;
using dal.bd2.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.bd2.Repository.PedidoEntrega
{
    public class TPedidosEntregaRepository : GenericRepository<biz.bd2.Entities.TPedidosEntrega>, ITPedidosEntregaRepository
    {
        public TPedidosEntregaRepository(BD2Context context) : base(context)
        {

        }
        public string GetProveedor(int idProveedor)
        {
            string _idProveedor = _context.Proveedores.SingleOrDefault(x => x.Codproveedor == idProveedor).Nomproveedor;

            return _idProveedor;
        }
    }
}
