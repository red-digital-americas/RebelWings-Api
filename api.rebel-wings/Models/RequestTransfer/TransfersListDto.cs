namespace api.rebel_wings.Models.RequestTransfer;
/// <summary>
/// Lista de Transferencias
/// </summary>
public class TransfersListDto
{
    /// <summary>
    /// ID de Sucursal
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Nombre de Sucursal
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Descripción
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// ID si tiene Transferido
    /// </summary>
    public int? TranferId { get; set; }
    /// <summary>
    /// ID de tiene Solicitado
    /// </summary>
    public int? RequestId { get; set; }
}