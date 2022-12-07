using biz.bd1.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.bd1.Repository.Stock
{
    public interface IStockRepository : IGenericRepository<biz.bd1.Entities.Stock>
    {
        List<biz.bd1.Models.StockDto> GetStock(int id_sucursal);
        decimal StockValidate(int id_sucursal, int codarticulo);
        biz.bd1.Models.StockDto UpdateStock(int codArticulo, string codAlmacen, int cantidad);
    }
}
