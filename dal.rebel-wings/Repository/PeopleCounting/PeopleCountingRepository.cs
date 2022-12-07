using biz.rebel_wings.Repository.PeopleCounting;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.PeopleCounting;

public class PeopleCountingRepository : GenericRepository<biz.rebel_wings.Entities.PeopleCounting>, IPeopleCountingRepository
{
    public PeopleCountingRepository(Db_Rebel_WingsContext context) : base(context)
    {
    }

}
