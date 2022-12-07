using System.ComponentModel;

namespace api.rebel_wings.Models.DrinksTemperature;
/// <summary>
/// Modelo de Fotos de Temperatura de bebidas
/// </summary>
public class PhotoDrinksTemperatureDto
{
    [DefaultValue(0)]
    public int Id { get; set; }
    /// <summary>
    /// FK => Lave foranea de Temperatura de bebidas
    /// </summary>
    public int DrinkTemperatureId { get; set; }
    /// <summary>
    /// Foto
    /// </summary>
    public string Photo { get; set; }
    /// <summary>
    /// Extensión
    /// </summary>
    public string PhotoPath { get; set; }
    /// <summary>
    /// Tipo de Foto:
    /// 1 ==> Chope clara
    /// 2 ==> Chope oscura
    /// 3 ==> 5 bebidas aleatorias
    /// </summary>
    public int Type { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

}