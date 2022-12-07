using biz.rebel_wings.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.rebel_wings.Repository.CashRegisterShortage
{
    public interface ICashRegisterShortageRepository : IGenericRepository<Entities.CashRegisterShortage>
    {
        DateTime CashRegisterShortageCreate(int id_sucursal);
    }
}
