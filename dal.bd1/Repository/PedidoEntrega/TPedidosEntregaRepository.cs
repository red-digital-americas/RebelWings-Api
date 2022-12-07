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
    public class TPedidosEntregaRepository : GenericRepository<biz.bd1.Entities.TPedidosEntrega>, ITPedidosEntregaRepository
    {
        public TPedidosEntregaRepository(BD1Context context) : base(context)
        {
           
        }
        public string GetProveedor(int idProveedor)
        {
            string _idProveedor = _context.Proveedores.SingleOrDefault(x => x.Codproveedor == idProveedor).Nomproveedor;

            return _idProveedor;
        }
    }
}
