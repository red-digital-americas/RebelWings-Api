namespace api.rebel_wings.Models.Bar;
/// <summary>
/// Models
/// </summary>
public class BarDto
{
    public BarDto()
    {
        PhotoBars = new HashSet<PhotoBarDto>();
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
    /// Tarja
    /// </summary>
    public bool Sink { get; set; }
    public string? CommentSink { get; set; }
    /// <summary>
    /// Mezcladora
    /// </summary>
    public bool Mixer { get; set; }
    public string? CommentMixer { get; set; }
    /// <summary>
    /// Refrigerador
    /// </summary>
    public bool Refrigerator { get; set; }
    public string? CommentRefrigerator { get; set; }
    /// <summary>
    /// Conexiones eléctricas
    /// </summary>
    public bool ElectricalConnections { get; set; }
    public string? CommentElectricalConnections { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    
    public virtual ICollection<PhotoBarDto> PhotoBars { get; set; }
}