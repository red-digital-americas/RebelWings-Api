using api.rebel_wings.Models.User;

namespace api.rebel_wings.Models.Task;

public class TaskGetDto
{
    public TaskGetDto()
    {
        TaskBranches = new HashSet<TaskBranchDto>();
    }

    public int Id { get; set; }
    public string Icon { get; set; }
    public int AssignedToId { get; set; }
    public int WorkshiftId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual CatAssignedToDto AssignedTo { get; set; }
    public virtual UserDto CreatedByNavigation { get; set; }
    public virtual UserDto UpdatedByNavigation { get; set; }
    public virtual CatWorkShiftDto Workshift { get; set; }
    public virtual ICollection<TaskBranchDto> TaskBranches { get; set; }
}