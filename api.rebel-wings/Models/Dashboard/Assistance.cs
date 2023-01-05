namespace api.rebel_wings.Models.Dashboard;

public class Assistance
{
    public string Name { get; set; }
    public string JobTittle { get; set; }
    public DateTime Date { get; set; }
    public string TimeArrive { get; set; }
    public int AssistanceStatus { get; set; }
    public int? Detail { get; set; }
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