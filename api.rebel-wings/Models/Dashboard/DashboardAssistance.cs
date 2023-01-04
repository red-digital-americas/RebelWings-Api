namespace api.rebel_wings.Models.Dashboard;
/// <summary>
/// Model
/// </summary>
public class DashboardAssistance
{
    /// <summary>
    /// Constructor
    /// </summary>
    public DashboardAssistance()
    {
        AssistanceMorningsCollection = new HashSet<Assistance>();
        AssistanceEveningsCollection = new HashSet<Assistance>();
    }
    public decimal PercentageAssistance { get; set; }
    public int Delays { get; set; }
    public int Absences { get; set; }
    /// <summary>
    /// Mañana
    /// </summary>
    public virtual ICollection<Assistance> AssistanceMorningsCollection { get; set; }
    /// <summary>
    /// Vespertino
    /// </summary>
    public virtual ICollection<Assistance> AssistanceEveningsCollection { get; set; }
}

public class DashboardAssistanceV2
{
    public DashboardAssistanceV2()
    {
        AssistanceMorningsCollection = new HashSet<AssistanceV2>();
        AssistanceEveningsCollection = new HashSet<AssistanceV2>();
    }
    public decimal PercentageAssistance { get; set; }
    public int Delays { get; set; }
    public int Absences { get; set; }
    public virtual ICollection<AssistanceV2> AssistanceMorningsCollection { get; set; }
    public virtual ICollection<AssistanceV2> AssistanceEveningsCollection { get; set; }
}