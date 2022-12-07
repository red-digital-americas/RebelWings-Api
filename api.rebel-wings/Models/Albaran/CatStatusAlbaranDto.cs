namespace api.rebel_wings.Models.Albaran;

public class CatStatusAlbaranDto
{
    public int Id { get; set; }
    public string Status { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}