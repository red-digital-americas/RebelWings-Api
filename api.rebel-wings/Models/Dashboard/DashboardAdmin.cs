namespace api.rebel_wings.Models.Dashboard;
/// <summary>
/// Model
/// </summary>
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
    public virtual  ICollection<BranchWithMostOmissions> BranchWithMostOmissionsCollection { get; set; }
    /// <summary>
    /// Most omitted activities
    /// </summary>
    public virtual  ICollection<MostOmittedActivities> MostOmittedActivitiesCollection { get; set; }
    public virtual ICollection<OmissionsPerShift> OmissionsPerShifts { get; set; }
    
}