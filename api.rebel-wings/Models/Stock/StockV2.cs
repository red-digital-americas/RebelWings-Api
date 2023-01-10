using api.rebel_wings.Models.User;

namespace api.rebel_wings.Models.Stock;

public class StockV2
{
    /// <summary>
    /// ID Primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Branch sucursal
    /// </summary>
    public int? Branch { get; set; }
    /// <summary>
    /// Inventory initial
    /// </summary>
    public decimal InvInicial { get; set; }
    /// <summary>
    /// Inventory registry
    /// </summary>
    public decimal InvReg { get; set; }
    /// <summary>
    /// defference
    /// </summary>
    public decimal Diferencia { get; set; }
    /// <summary>
    /// Intents
    /// </summary>
    public int? Intentos { get; set; }
    /// <summary>
    /// Arcticle
    /// </summary>
    public string? Articulo { get; set; }
    /// <summary>
    /// Created by
    /// </summary>
    public int CreatedBy { get; set; }
    /// <summary>
    /// Created date
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Updated by
    /// </summary>
    public int? UpdatedBy { get; set; }
    /// <summary>
    /// Updated date
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
    /// <summary>
    ///  User model
    /// </summary>
    public virtual UserDto CreatedByNavigation { get; set; } = null!;
}

public class StockV2Response
{
    /// <summary>
    /// ID Primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Branch sucursal
    /// </summary>
    public int? Branch { get; set; }
    public string BranchName { get; set; }
    /// <summary>
    /// Inventory initial
    /// </summary>
    public decimal InvInicial { get; set; }
    /// <summary>
    /// Inventory registry
    /// </summary>
    public decimal InvReg { get; set; }
    /// <summary>
    /// defference
    /// </summary>
    public decimal Diferencia { get; set; }
    /// <summary>
    /// Intents
    /// </summary>
    public int? Intentos { get; set; }
    /// <summary>
    /// Arcticle
    /// </summary>
    public string? Articulo { get; set; }
    /// <summary>
    /// Created by
    /// </summary>
    public int CreatedBy { get; set; }
    /// <summary>
    /// Created date
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Updated by
    /// </summary>
    public int? UpdatedBy { get; set; }
    /// <summary>
    /// Updated date
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
    /// <summary>
    ///  User model
    /// </summary>
    public virtual UserDto CreatedByNavigation { get; set; } = null!;
}