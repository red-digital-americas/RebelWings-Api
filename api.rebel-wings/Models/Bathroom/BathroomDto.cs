namespace api.rebel_wings.Models.Bathroom;
/// <summary>
/// Models
/// </summary>
public class BathroomDto
{
    public BathroomDto()
    {
        PhotoBathrooms = new HashSet<PhotoBathroomDto>();
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
    /// Mingitorios
    /// </summary>
    public bool Urinals { get; set; }
    public string? CommentUrinals { get; set; }
    /// <summary>
    /// Lavamanos
    /// </summary>
    public bool HandWashBasin { get; set; }
    public string? CommentHandWashBasin { get; set; }
    /// <summary>
    /// Luminarias
    /// </summary>
    public bool Luminaires { get; set; }
    public string? CommentLuminaires { get; set; }
    /// <summary>
    /// Puertas
    /// </summary>
    public bool Doors { get; set; }        
    public string? CommentDoors { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public virtual ICollection<PhotoBathroomDto> PhotoBathrooms { get; set; }

}