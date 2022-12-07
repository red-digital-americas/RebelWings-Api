using biz.bd2.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.bd2.Repository.Stock
{
    public interface IStockRepository : IGenericRepository<biz.bd2.Entities.Stock>
    {
        List<biz.bd2.Models.StockDto> GetStock(int id_sucursal);
        decimal StockValidate(int id_sucursal, int codarticulo);
        biz.bd2.Models.StockDto UpdateStock(int codArticulo, string codAlmacen, int cantidad);
    }
}
