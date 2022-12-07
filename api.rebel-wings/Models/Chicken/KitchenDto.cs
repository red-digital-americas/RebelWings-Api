namespace api.rebel_wings.Models.Chicken;
/// <summary>
/// Model
/// </summary>
public class KitchenDto
{
    public KitchenDto()
    {
        PhotoKitchens = new HashSet<PhotoKitchenDto>();
    }
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// FK
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Tarjas
    /// </summary>
    public bool Sink { get; set; }
    public string? CommentSink { get; set; }
    /// <summary>
    /// Mezcladoras
    /// </summary>
    public bool Mixer { get; set; }
    public string? CommentMixer { get; set; }
    /// <summary>
    /// Coladeras
    /// </summary>
    public bool Strainer { get; set; }
    public string? CommentStrainer { get; set; }
    /// <summary>
    /// Freidoras
    /// </summary>
    public bool Fryer { get; set; }
    public string? CommentFryer { get; set; }
    /// <summary>
    /// Extracción
    /// </summary>
    public bool Extractor { get; set; }
    public string? CommentExtractor { get; set; }
    /// <summary>
    /// Refrigeradores
    /// </summary>
    public bool Refrigerator { get; set; }
    public string? CommentRefrigerator { get; set; }
    /// <summary>
    /// Temperatura interior
    /// </summary>
    public bool InteriorTemperature { get; set; }
    public string? CommentInteriorTemperature { get; set; }
    /// <summary>
    /// Puertas
    /// </summary>
    public bool Doors { get; set; }
    public string? CommentDoors { get; set; }
    /// <summary>
    /// Distancia correcta hacia la pared
    /// </summary>
    public bool CorrectDistance { get; set; }
    public string? CommentCorrectDistance { get; set; }
    /// <summary>
    /// Conexiones eléctricas
    /// </summary>
    public bool ElectricalConnections { get; set; }
    public string? CommentElectricalConnections { get; set; }
    /// <summary>
    /// Luminarias
    /// </summary>
    public bool Luminaires { get; set; }
    public string? CommentLuminaires { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public virtual ICollection<PhotoKitchenDto> PhotoKitchens { get; set; }
}