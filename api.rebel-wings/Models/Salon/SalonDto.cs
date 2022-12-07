namespace api.rebel_wings.Models.Salon;
/// <summary>
/// Models
/// </summary>
public class SalonDto
{
    public SalonDto()
    {
        PhotoSalons = new HashSet<PhotoSalonDto>();
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
    /// Puertas de acceso
    /// </summary>
    public bool AccessDoors { get; set; }
    public string? CommentAccessDoors { get; set; }
    /// <summary>
    /// Chapas / Badges
    /// </summary>
    public bool Badges { get; set; }
    public string? CommentBadges { get; set; }
    /// <summary>
    /// Luminarias
    /// </summary>
    public bool Luminaires { get; set; }
    public string? CommentLuminaires { get; set; }
    /// <summary>
    /// Contactos y apagadores
    /// </summary>
    public bool Switches { get; set; }
    public string? CommentSwitches { get; set; }
    /// <summary>
    /// Mobiliario
    /// </summary>
    public bool FurnitureOne { get; set; }
    public string? CommentFurnitureOne { get; set; }
    /// <summary>
    /// Mobiliario
    /// </summary>
    public bool FurnitureTwo { get; set; }
    public string? CommentFurnitureTwo { get; set; }
    /// <summary>
    /// Boths
    /// </summary>
    public bool Boths { get; set; }
    public string? CommentBoths { get; set; }
    /// <summary>
    /// Extintores
    /// </summary>
    public bool FireExtinguishers { get; set; }
    public string? CommentFireExtinguishers { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public virtual ICollection<PhotoSalonDto> PhotoSalons { get; set; }
}