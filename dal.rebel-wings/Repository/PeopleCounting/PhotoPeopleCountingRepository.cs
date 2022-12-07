using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.PeopleCounting;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.PeopleCounting;

public class PhotoPeopleCountingRepository : GenericRepository<PhotoPeopleCounting>, IPhotoPeopleCountingRepository
{
  public PhotoPeopleCountingRepository(Db_Rebel_WingsContext context) : base(context)
  {
  }
}
