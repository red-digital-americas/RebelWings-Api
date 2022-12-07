namespace biz.rebel_wings.Models.Dashboard;

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

public class TaskPerShifts
{
    public string NameTask { get; set; }
    public string Supervisor { get; set; }
    public DateTime Date { get; set; }
    public decimal PercentageComplete { get; set; }
    public bool Status { get; set; }
    public int Detail { get; set; }
} 