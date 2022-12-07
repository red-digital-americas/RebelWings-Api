using System.ComponentModel;
using api.rebel_wings.Models.User;

namespace api.rebel_wings.Models.Ticket;

public class TicketGetDto
{
    /// <summary>
    /// ID => PK
    /// </summary>
    [DefaultValue(0)]
    public int Id { get; set; }
    /// <summary>
    /// ID de Sucursal
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// NOmbre de la Sucursal
    /// </summary>
    public int BranchName { get; set; }
    /// <summary>
    /// ¿En qué parte de la sucursal te encuentras?
    /// </summary>
    public int PartBranchId { get; set; }
    /// <summary>
    /// ID de Estatus
    /// </summary>
    public int StatusId { get; set; }
    /// <summary>
    /// Seleccina el lugar específico
    /// </summary>
    public int SpecificSectionId { get; set; }
    /// <summary>
    /// Describe el problema encontrado
    /// </summary>
    public string Problem { get; set; }
    /// <summary>
    /// Catalogo de Parte de la sucursal
    /// </summary>
    public virtual CatPartBranchDto PartBranch { get; set; }
    /// <summary>
    /// Catalogo de Sección específica
    /// </summary>
    public virtual CatSpecificSectionDto SpecificSection { get; set; }
    /// <summary>
    /// Catalogo de Estatus
    /// </summary>
    public virtual CatStatusTicketDto Status { get; set; }
    /// <summary>
    /// Fotografías
    /// </summary>
    public virtual ICollection<PhotoTicketDto> PhotoTickets { get; set; }
    public virtual UserDto CreatedByNavigation { get; set; }
}