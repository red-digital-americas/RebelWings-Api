namespace api.rebel_wings.Models.RiskProduct;
/// <summary>
/// Model Producto en riesgo GET
/// </summary>
public class RiskProductGetDto
{
    /// <summary>
    /// ID => PK
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// ID de Sucursal 
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// ID de Producto
    /// </summary>
    public int ProductId { get; set; }
    /// <summary>
    /// Nombre de producto
    /// </summary>
    public string? Product { get; set; }
    // /// <summary>
    // /// Codigo ID
    // /// </summary>
    // public int CodeId { get; set; }
    /// <summary>
    /// Codigo
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// Comentario
    /// </summary>
    public string Comment { get; set; }
    /// <summary>
    /// Quien lo creo
    /// </summary>
    public int CreatedBy { get; set; }
    /// <summary>
    /// Fecha de Creación
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Quien Actualizo
    /// </summary>
    public int? UpdatedBy { get; set; }
    /// <summary>
    /// Fecha de Actualizaión
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
}