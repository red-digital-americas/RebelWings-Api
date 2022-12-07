namespace api.rebel_wings.Models.Dashboard;
/// <summary>
/// Model
/// </summary>
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
    public int AverageNumberAttendances { get; set; }
    /// <summary>
    /// Average Evaluation
    /// </summary>
    public int AverageEvaluation { get; set; }
}