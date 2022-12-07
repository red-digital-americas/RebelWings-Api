using System.ComponentModel;

namespace api.rebel_wings.Models.BanosMatutino
{
  public class PhotoBanosMatutinoDto
  {
    [DefaultValue(0)]
    public int Id { get; set; }
    /// <summary>
    /// Llave foranea de To Set Table (Salón Montado)
    /// </summary>
    public int? BanosMatutinoId { get; set; }
    /// <summary>
    /// Extensión de Foto
    /// </summary>
    [DefaultValue("")]
    public string PhotoPath { get; set; }
    /// <summary>
    /// Base64 de la Foto
    /// </summary>
    public string Photo { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
  }
}
