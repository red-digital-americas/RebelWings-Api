namespace api.rebel_wings.Models.Inventario;

public class InventarioDto
{
  
  public int Id { get; set; }
  public int Branch { get; set; }
  public decimal InvInicial { get; set; }
  public decimal InvReg { get; set; }
  public decimal Diferencia { get; set; }
  public int Intentos { get; set; }
  public string? Articulo { get; set; }
  public int CreatedBy { get; set; }
  public DateTime CreatedDate { get; set; }
  public int? UpdatedBy { get; set; }
  public DateTime? UpdatedDate { get; set; }
}
