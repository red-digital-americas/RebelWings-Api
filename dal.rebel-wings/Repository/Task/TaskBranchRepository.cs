using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Task;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Task;

public class TaskBranchRepository : GenericRepository<TaskBranch>, ITaskBranchRepository
{
    public TaskBranchRepository(Db_Rebel_WingsContext context) : base(context)
    {
        
    }
}