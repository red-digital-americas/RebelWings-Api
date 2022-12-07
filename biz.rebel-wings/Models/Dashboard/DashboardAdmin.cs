namespace biz.rebel_wings.Models.Dashboard;

public class DashboardAdmin
{
    /// <summary>
    /// Constructor
    /// </summary>
    public DashboardAdmin()
    {
        BranchWithMostOmissionsCollection = new HashSet<BranchWithMostOmissions>();
        MostOmittedActivitiesCollection = new HashSet<MostOmittedActivities>();
        OmissionsPerShifts = new HashSet<OmissionsPerShift>();
    }

    /// <summary>
    /// Activity Report 
    /// </summary>
    public virtual ActivityReportAdmin ActivityReportAdmin { get; set; }
    /// <summary>
    /// Branches with the most omissions
    /// </summary>
    public virtual ICollection<BranchWithMostOmissions> BranchWithMostOmissionsCollection { get; set; }
    /// <summary>
    /// Most omitted activities
    /// </summary>
    public virtual ICollection<MostOmittedActivities> MostOmittedActivitiesCollection { get; set; }
    public virtual ICollection<OmissionsPerShift> OmissionsPerShifts { get; set; }
}

public class ActivityReportAdmin
{
    /// <summary>
    /// Omission of Activities
    /// </summary>
    public decimal OmissionActivities { get; set; }
    /// <summary>
    /// Tickets
    /// </summary>
    public int Tickets { get; set; }
    /// <summary>
    /// Average number of attendances
    /// </summary>
    public decimal AverageNumberAttendances { get; set; }
    /// <summary>
    /// Average Evaluation
    /// </summary>
    public double AverageEvaluation { get; set; }
}

public class BranchWithMostOmissions
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Branch Name
    /// </summary>
    public string? BranchName { get; set; }
    /// <summary>
    /// Percentage
    /// </summary>
    public decimal? Percentage { get; set; }
}

public class MostOmittedActivities
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Percentage { get; set; }
}

public class OmissionsPerShift
{
    /// <summary>
    /// Shift
    /// </summary>
    public int Shift { get; set; }
    /// <summary>
    /// Count
    /// </summary>
    public int Count { get; set; }
}