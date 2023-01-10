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
public class AssistanceV2
{
    public DateTime Date { get; set; }
    public decimal Cashiers { get; set; }
    public decimal Sellers { get; set; }
    public decimal Chefs { get; set; }
    public int Status { get; set; }
    public int? Detail { get; set; }
}