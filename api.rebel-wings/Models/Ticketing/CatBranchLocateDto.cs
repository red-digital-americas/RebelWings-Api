namespace api.rebel_wings.Models.Ticketing;

public class CatBranchLocateDto
{
    public int Id { get; set; }
    public string Locate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}