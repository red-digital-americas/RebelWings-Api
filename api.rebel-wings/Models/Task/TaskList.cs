namespace api.rebel_wings.Models.Task;

public class TaskList
{
    public int Id { get; set; }
    public string NameString { get; set; }
    public string Icon { get; set; }
    public string BranchName { get; set; }
    public string AssignedTo { get; set; }
    public string Description { get; set; }
}