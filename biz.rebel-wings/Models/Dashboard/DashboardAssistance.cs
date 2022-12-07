namespace biz.rebel_wings.Models.Dashboard;

public class DashboardAssistance
{
    public DashboardAssistance()
    {
        AssistanceMorningsCollection = new HashSet<Assistance>();
        AssistanceEveningsCollection = new HashSet<Assistance>();
    }
    public decimal PercentageAssistance { get; set; }
    public int Delays { get; set; }
    public int Absences { get; set; }
    public virtual ICollection<Assistance> AssistanceMorningsCollection { get; set; }
    public virtual ICollection<Assistance> AssistanceEveningsCollection { get; set; }
}

public class Assistance
{
    public string Name { get; set; }
    public string JobTittle { get; set; }
    public DateTime Date { get; set; }
    public string TimeArrive { get; set; }
    public int AssistanceStatus { get; set; }
    public int? Detail { get; set; }
}