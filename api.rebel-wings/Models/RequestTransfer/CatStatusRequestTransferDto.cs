namespace api.rebel_wings.Models.RequestTransfer;

public class CatStatusRequestTransferDto
{
    public int Id { get; set; }
    public string? Status { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}