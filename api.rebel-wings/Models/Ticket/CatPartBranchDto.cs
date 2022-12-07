namespace api.rebel_wings.Models.Ticket;

public class CatPartBranchDto
{
    public int Id { get; set; }
    public string PartBranch { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}