using System.ComponentModel;

namespace api.rebel_wings.Models.DrinksTemperature;

public class DrinksTemperatureDto
{
    public DrinksTemperatureDto()
    {
        PhotoDrinksTemperatures = new HashSet<PhotoDrinksTemperatureDto>();
    }
    [DefaultValue(0)]
    public int Id { get; set; }
    /// <summary>
    /// ID de Sucursal
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Chope clara
    /// </summary>
    public bool LightBeer { get; set; }
    /// <summary>
    /// Chope oscura
    /// </summary>
    public bool DarkBeer { get; set; }
    /// <summary>
    /// Elige 5 bebidas aleatorias
    /// ¿Fría?
    /// </summary>
    public bool DrinkOne { get; set; }
    /// <summary>
    /// Elige 5 bebidas aleatorias
    /// ¿Fría?
    /// </summary>
    public bool DrinkTwo { get; set; }
    /// <summary>
    /// Elige 5 bebidas aleatorias
    /// ¿Fría?
    /// </summary>
    public bool DrinkThree { get; set; }
    /// <summary>
    /// Elige 5 bebidas aleatorias
    /// ¿Fría?
    /// </summary>
    public bool DrinkFour { get; set; }
    /// <summary>
    /// Elige 5 bebidas aleatorias
    /// ¿Fría?
    /// </summary>
    public bool DrinkFive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<PhotoDrinksTemperatureDto> PhotoDrinksTemperatures { get; set; }
    public bool Chope { get; set; }
}
