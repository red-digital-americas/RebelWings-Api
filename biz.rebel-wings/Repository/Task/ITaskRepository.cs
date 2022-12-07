using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Generic;

namespace biz.rebel_wings.Repository.Task;

public interface ITaskRepository : IGenericRepository<Entities.Task>
{
    List<CatWorkShift> getWorkShifts();
    List<CatAssignedTo> getAssignedTos();
    List<CatTypeField> getTypeFields();
}