namespace biz.rebel_wings.Models.Dashboard;

public class DashboardAdminPerformance
{
    public DashboardAdminPerformance()
    {
        Performances = new HashSet<DashboardAdminPerformanceByBranch>();
        TopOmittedTask = new HashSet<TaskNoComplete>();
        Multi = new HashSet<BranchChartBarVertical>();
    }
    public int Id { get; set; }
    public decimal Complete { get; set; }
    public decimal NoComplete { get; set; }
    public decimal AverageEvaluation { get; set; }
    public virtual ICollection<TaskNoComplete> TopOmittedTask { get; set; }
    public virtual ICollection<DashboardAdminPerformanceByBranch> Performances { get; set; }
    public virtual ICollection<BranchChartBarVertical> Multi { get; set; }
}

public class DashboardAdminPerformanceByBranch
{ 
    public int IdBranch { get; set; }
    public string? NameBranch { get; set; }
    public decimal Complete { get; set; }
    public decimal NoComplete { get; set; }
    public decimal AverageEvaluation { get; set; }
}

public class TaskNoComplete
{
    public string Name { get; set; }
    public int Value { get; set; }
}

public class PerformanceByTask
{
    public decimal Complete { get; set; }
    public decimal NoComplete { get; set; }
}

public class BranchChartBarVertical
{
    public string Name { get; set; }
    public ICollection<Serie> Series { get; set; }
}

public class Serie
{
    public string Name { get; set; }
    public decimal Value { get; set; }
}