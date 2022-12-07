using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biz.rebel_wings.Repository.SalesExpectations;
using biz.rebel_wings.Repository.Generic;
using biz.rebel_wings.Entities;
using dal.rebel_wings.Repository.Generic;
using biz.rebel_wings.Models.StockChicken;

namespace dal.rebel_wings.Repository.SalesExpectations
{
    public class SalesExpectationRepository : GenericRepository<StockChicken>, IStockChickenRepository
    {
        public SalesExpectationRepository(DBContext.Db_Rebel_WingsContext context) : base(context) { }

        public async Task<List<StockChickenGet>> GetAll(int branch)
        {
            List<StockChickenGet> stockChickenGets = _context.StockChickens.Where(x => x.Branch == branch && x.StatusId != 2).Select(s => new StockChickenGet()
            {
                Amount = s.Amount,
                Code = s.Code,
                Id = s.Id,
                Status = s.StatusId == 3 ? $"{s.Amount - s.StockChickeUseds.Sum(q => q.AmountUsed)} Kgs {s.Status.Status}" : s.Status.Status,
                Created = s.CreatedDate
            }).ToList();
            List<StockChickenGet> stockChickenUsed =
                _context.StockChickens.Where(x => x.Branch == branch && x.StatusId == 2 && x.CreatedDate.Date == DateTime.Now.Date ).Select(s =>
                    new StockChickenGet()
                    {
                        Amount = s.Amount,
                        Code = s.Code,
                        Id = s.Id,
                        Status = s.Status.Status,
                        Created = s.CreatedDate
                    }).ToList();
            return stockChickenGets.Union(stockChickenUsed).ToList();
        }

    }
}
