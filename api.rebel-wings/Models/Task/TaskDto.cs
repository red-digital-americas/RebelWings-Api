namespace api.rebel_wings.Models.Task;

public class TaskDto
{
    /// <summary>
    /// ID => PK
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Icono de tarea (Base64)
    /// </summary>
    public string Icon { get; set; }
    /// <summary>
    /// Extensión de imagen
    /// </summary>
    public string IconPath { get; set; }
    /// <summary>
    /// A quien se le asigna
    /// </summary>
    public int AssignedToId { get; set; }
    /// <summary>
    /// Turno
    /// </summary>
    public int WorkshiftId { get; set; }
    /// <summary>
    /// Nombre de Tarea
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Descripción
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Creado Por
    /// </summary>
    public int CreatedBy { get; set; }
    /// <summary>
    /// Fecha de creación
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Actualizado por
    /// </summary>
    public int? UpdatedBy { get; set; }
    /// <summary>
    /// Fecha actualizado
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
    /// <summary>
    /// Sucursales
    /// </summary>
    public virtual ICollection<TaskBranchDto> TaskBranches { get; set; }
    /// <summary>
    /// Campos
    /// </summary>
    public virtual ICollection<FormFieldDto> FormFields { get; set; }
}