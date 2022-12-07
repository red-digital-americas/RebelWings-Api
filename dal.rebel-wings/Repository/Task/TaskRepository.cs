using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Task;
using dal.rebel_wings.DBContext;
using dal.rebel_wings.Repository.Generic;

namespace dal.rebel_wings.Repository.Task;

public class TaskRepository : GenericRepository<biz.rebel_wings.Entities.Task>, ITaskRepository
{
    public TaskRepository(Db_Rebel_WingsContext context) : base(context)
    {
        
    }

    public List<CatWorkShift> getWorkShifts()
    {
        return _context.CatWorkShifts.ToList();
    }

    public List<CatAssignedTo> getAssignedTos()
    {
        return _context.CatAssignedTos.ToList();
    }

    public List<CatTypeField> getTypeFields()
    {
        return _context.CatTypeFields.ToList();
    }
}