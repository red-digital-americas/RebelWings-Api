using System.ComponentModel;

namespace api.rebel_wings.Models.RequestTransfer;
/// <summary>
/// Modelo Transferencias
/// </summary>
public class RequestTransferDto
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
    public int FromBranchId { get; set; }
    public int ToBranchId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan? Time { get; set; }
    public int ProductId { get; set; }
    public string? Code { get; set; }
    public string Amount { get; set; } = null!;
    public string Comment { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}