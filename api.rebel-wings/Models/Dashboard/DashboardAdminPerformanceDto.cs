namespace api.rebel_wings.Models.Dashboard;

public class DashboardAdminPerformanceDto
{
    public DashboardAdminPerformanceDto()
    {
        Performances = new HashSet<DashboardAdminPerformanceByBranchDto>();
        TopOmittedTask = new HashSet<TaskNoCompleteDto>();
        Multi = new HashSet<BranchChartBarVerticalDto>();
    }
    public int Id { get; set; }
    public decimal Complete { get; set; }
    public decimal NoComplete { get; set; }
    public decimal AverageEvaluation { get; set; }
    public virtual ICollection<DashboardAdminPerformanceByBranchDto> Performances { get; set; }
    public virtual ICollection<TaskNoCompleteDto> TopOmittedTask { get; set; }
    public virtual ICollection<BranchChartBarVerticalDto> Multi { get; set; }
}

public class DashboardAdminPerformanceByBranchDto
{
    public int IdBranch { get; set; }
    public string? NameBranch { get; set; }
    public decimal Complete { get; set; }
    public decimal NoComplete { get; set; }
    public decimal AverageEvaluation { get; set; }
}

public class TaskNoCompleteDto
{
    public string Name { get; set; }
    public int Value { get; set; }
}

public class BranchChartBarVerticalDto
{
    public string Name { get; set; }
    public ICollection<SerieDto> Series { get; set; }
}

public class SerieDto
{
    public string Name { get; set; }
    public decimal Value { get; set; }
}