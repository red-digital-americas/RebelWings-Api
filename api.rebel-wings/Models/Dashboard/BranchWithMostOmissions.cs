namespace api.rebel_wings.Models.Dashboard;
/// <summary>
/// Model
/// </summary>
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