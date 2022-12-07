namespace api.rebel_wings.Models.RequestTransfer;
/// <summary>
/// Catalogo de Montos
/// </summary>
public class CatAmountDto
{
    public int Id { get; set; }
    public string Amount { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}