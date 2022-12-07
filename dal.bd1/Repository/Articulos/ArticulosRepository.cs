using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.bd1.Repository.Articulos
{
    public class ArticulosRepository : Generic.GenericRepository<biz.bd1.Entities.Articulo1>, biz.bd1.Repository.Articulos.IArticulosRespository
    {
        public ArticulosRepository(DBContext.BD1Context context) : base(context) { }
    }
}
