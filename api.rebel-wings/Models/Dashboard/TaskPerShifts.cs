namespace api.rebel_wings.Models.Dashboard;

public class TaskPerShifts
{
    public string NameTask { get; set; }
    public string Supervisor { get; set; }
    public DateTime Date { get; set; }
    public decimal PercentageComplete { get; set; }
    public bool Status { get; set; }
    public int Detail { get; set; }
}