namespace api.rebel_wings.Models.Ticket;

public class CatSpecificSectionDto
{
    public int Id { get; set; }
    public int PartBranchId { get; set; }
    public string SpecificSection { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}