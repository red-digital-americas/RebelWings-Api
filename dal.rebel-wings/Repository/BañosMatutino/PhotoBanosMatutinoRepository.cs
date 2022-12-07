using biz.rebel_wings.Repository.BanosMatutino;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.BanosMatutino;

public class PhotoBanosMatutinoRepository : GenericRepository<biz.rebel_wings.Entities.PhotoBanosMatutino>, IPhotoBanosMatutinoRepository
{
  public PhotoBanosMatutinoRepository(Db_Rebel_WingsContext context) : base(context)
  {
  }
}
