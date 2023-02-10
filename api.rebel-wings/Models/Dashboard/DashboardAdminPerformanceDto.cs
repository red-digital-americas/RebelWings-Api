namespace api.rebel_wings.Models.Dashboard;

public class DashboardAdminPerformanceDto
{
    public DashboardAdminPerformanceDto()
    {
        Performances = new HashSet<DashboardAdminPerformanceByBranchDto>();
    }
    public int Id { get; set; }
    public decimal Complete { get; set; }
    public decimal NoComplete { get; set; }
    public decimal AverageEvaluation { get; set; }
    public virtual ICollection<DashboardAdminPerformanceByBranchDto> Performances { get; set; }
}

public class DashboardAdminPerformanceByBranchDto
{
    public int IdBranch { get; set; }
    public string? NameBranch { get; set; }
    public decimal Complete { get; set; }
    public decimal NoComplete { get; set; }
    public decimal AverageEvaluation { get; set; }
}