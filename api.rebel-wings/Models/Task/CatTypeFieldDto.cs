namespace api.rebel_wings.Models.Task;

public class CatTypeFieldDto
{
    public int Id { get; set; }
    public string TypeField { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}