using biz.rebel_wings.Repository.ValidationGas;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.ValidationGas;

public class PhotoValidationGasRepository : GenericRepository<biz.rebel_wings.Entities.PhotoValidationGa>, IPhotoValidationGasRepository
{
    public PhotoValidationGasRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }
}
