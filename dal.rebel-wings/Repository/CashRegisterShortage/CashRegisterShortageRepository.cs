using biz.rebel_wings.Repository.CashRegisterShortage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dal.rebel_wings.Repository.CashRegisterShortage
{
    public class CashRegisterShortageRepository : Generic.GenericRepository<biz.rebel_wings.Entities.CashRegisterShortage>, ICashRegisterShortageRepository
    {
        public CashRegisterShortageRepository(DBContext.Db_Rebel_WingsContext context) : base(context) { }

        public DateTime CashRegisterShortageCreate(int id_sucursal)
        {
            DateTime _date = _context.CashRegisterShortages
                .OrderByDescending(d => d.Id).FirstOrDefault(x => x.BranchId == id_sucursal)
                ?.CreatedDate == null
                ? Convert.ToDateTime("01/01/1900").Date : _context.CashRegisterShortages
                .OrderByDescending(d => d.Id).FirstOrDefault(x => x.BranchId == id_sucursal).CreatedDate;

            return _date;
        }

    }
}
