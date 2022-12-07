namespace biz.rebel_wings.Models.Dashboard;
/// <summary>
/// Model
/// </summary>
public class DashboardRegional
{
    /// <summary>
    /// Constructor
    /// </summary>
    public DashboardRegional()
    {
        TasksKitchenCollection = new HashSet<Task>();
        TasksSalonCollection = new HashSet<Task>();
        TasksBathroomsCollection = new HashSet<Task>();
        TasksSystemCollection = new HashSet<Task>();
        TasksMaintenanceCollection = new HashSet<Task>();
    }

    public decimal OmissionsActivities { get; set; }
    public int Tickets { get; set; }
    public decimal AverageEvaluation { get; set; }
    public virtual ICollection<Task> TasksKitchenCollection { get; set; }
    public virtual ICollection<Task> TasksSalonCollection { get; set; }
    public virtual ICollection<Task> TasksBathroomsCollection { get; set; }
    public virtual ICollection<Task> TasksSystemCollection { get; set; }
    public virtual ICollection<Task> TasksMaintenanceCollection { get; set; }
}
/// <summary>
/// Tasks
/// </summary>
public class Task
{
    public int Detail { get; set; }
    public string? Name { get; set; }
    public DateTime Date { get; set; }
    public Decimal PercentageComplete { get; set; }
    public int Status { get; set; }
    public string? Regional { get; set; }
}