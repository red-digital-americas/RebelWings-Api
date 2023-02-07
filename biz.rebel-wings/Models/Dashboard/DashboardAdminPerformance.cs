namespace biz.rebel_wings.Models.Dashboard;

public class DashboardAdminPerformance
{
    public DashboardAdminPerformance()
    {
        Performances = new HashSet<DashboardAdminPerformanceByBranch>();
    }
    public int Id { get; set; }
    public decimal Complete { get; set; }
    public decimal NoComplete { get; set; }
    public decimal AverageEvaluation { get; set; }
    public virtual ICollection<DashboardAdminPerformanceByBranch> Performances { get; set; }
}

public class DashboardAdminPerformanceByBranch
{
    public int IdBranch { get; set; }
    public string? NameBranch { get; set; }
    public decimal Complete { get; set; }
    public decimal NoComplete { get; set; }
    public decimal AverageEvaluation { get; set; }
}

public class PerformanceByTask
{
    public decimal Complete { get; set; }
    public decimal NoComplete { get; set; }
}