using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.CashRegisterShortage;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.CashRegisterShortage;

public class PhotoCashRegisterShortageRepository : GenericRepository<PhotoCashRegisterShortage>, IPhotoCashRegisterShortageRepository
{
    public PhotoCashRegisterShortageRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}