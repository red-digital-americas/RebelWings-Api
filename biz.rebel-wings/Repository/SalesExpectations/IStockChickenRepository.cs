using biz.rebel_wings.Models.StockChicken;
using biz.rebel_wings.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.rebel_wings.Repository.SalesExpectations
{
    public interface IStockChickenRepository : IGenericRepository<Entities.StockChicken>
    {
        Task<List<StockChickenGet>> GetAll(int branch);
    }
}
