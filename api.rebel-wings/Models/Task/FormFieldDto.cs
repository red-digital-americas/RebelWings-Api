namespace api.rebel_wings.Models.Task;

public class FormFieldDto
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public int TypeFieldId { get; set; }
    public string NameField { get; set; }
    public string Align { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}