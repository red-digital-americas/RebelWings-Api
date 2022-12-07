namespace api.rebel_wings.Models.Dashboard;

public class DashboardSupervisor
{
    public DashboardSupervisor()
    {
        TasksMorningsCollection = new HashSet<TaskPerShifts>();
        TasksEveningsCollection = new HashSet<TaskPerShifts>();
    }
    public decimal OmissionsActivities { get; set; }
    public int Tickets { get; set; }
    public decimal AverageAttendance { get; set; }
    public virtual ICollection<TaskPerShifts> TasksMorningsCollection { get; set; }
    public virtual ICollection<TaskPerShifts> TasksEveningsCollection{ get; set; }
}