namespace api.rebel_wings.Models.RequestTransfer;
/// <summary>
/// Modelo de Transferencias
/// </summary>
public class RequestTransferConsultDto
{
    /// <summary>
    /// ID => PK
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 1 => Si es Transferido
    /// 2 => Si es Solicitado
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// Estatus 
    /// </summary>
    public int Status { get; set; }
    /// <summary>
    /// De Sucursal
    /// </summary>
    public int FromBranchId { get; set; }
    /// <summary>
    /// A Sucursal
    /// </summary>
    public int ToBranchId { get; set; }
    /// <summary>
    /// Fecha
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// Hora
    /// </summary>
    public TimeSpan Time { get; set; }
    /// <summary>
    /// Producto Id
    /// </summary>
    public int ProductId { get; set; }
    /// <summary>
    /// Nombre de producto
    /// </summary>
    public string Product { get; set; }
    /// <summary>
    /// Codigo
    /// </summary>
    public string? Code { get; set; }
    /// <summary>
    /// Cantidad
    /// </summary>
    public string Amount { get; set; } = null!;
    /// <summary>
    /// Comentario
    /// </summary>
    public string Comment { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}