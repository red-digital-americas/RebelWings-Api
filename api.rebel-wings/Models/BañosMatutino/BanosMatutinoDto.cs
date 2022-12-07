using System.ComponentModel;

namespace api.rebel_wings.Models.BanosMatutino
{
  public class BanosMatutinoDto
  {
    [DefaultValue(0)]
    public int Id { get; set; }
    /// <summary>
    /// Sucursal
    /// </summary>
    [Description("Sucursal")]
    public int? Branch { get; set; }
    /// <summary>
    /// Comentarios
    /// </summary>
    [Description("Comentarios")]
    public string Comment { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public virtual ICollection<PhotoBanosMatutinoDto> PhotoBanosMatutinos { get; set; }
  }
}
