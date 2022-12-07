namespace api.rebel_wings.Models.RequestTransfer;
/// <summary>
/// Modelo de Solicitud/transferencias Lista New
/// </summary>
public class RequestTransferListNewDto
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
    public virtual List<TranseferRequestDto> TranferId { get; set; }
    /// <summary>
    /// ID de tiene Solicitado
    /// </summary>
    public virtual List<TranseferRequestDto> RequestId { get; set; }
}

public class TranseferRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int productId { get; set; }
}