using System.ComponentModel;

namespace api.rebel_wings.Models.SalesExpectations;
/// <summary>
/// Modelo De Stock de Pollo para Admin
/// </summary>
public class StockChickenByBranchDto
{
    /// <summary>
    /// ID => PK
    /// </summary>
    [DefaultValue(0)]
    public int Id { get; set; }
    /// <summary>
    /// ID de sucursal
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Expectativa de Ventas
    /// </summary>
    public decimal SalesExpectations { get; set; }
    /// <summary>
    /// Cantidad
    /// </summary>
    public decimal Amount { get; set; }
    /// <summary>
    /// Creado Por
    /// </summary>
    public int CreatedBy { get; set; }
    /// <summary>
    /// Fecha de Creación
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Actualizado Por
    /// </summary>
    public int? UpdatedBy { get; set; }
    /// <summary>
    /// Fecha de Actualización
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
}