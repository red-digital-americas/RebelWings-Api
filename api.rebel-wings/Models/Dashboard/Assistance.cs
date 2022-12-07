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