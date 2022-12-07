using biz.bd2.Repository.Articulos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.bd2.Repository.Articulos
{
    public class ArticulosRepository : Generic.GenericRepository<biz.bd2.Entities.Articulo1>, IArticulosRepository
    {
        public ArticulosRepository(DBContext.BD2Context context) : base(context) { }
    }
}
