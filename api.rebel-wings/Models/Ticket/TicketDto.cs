using System.ComponentModel;

namespace api.rebel_wings.Models.Ticket;
/// <summary>
/// Modelo de Levantamiento de ticket
/// </summary>
public class TicketDto
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
    /// ¿En qué parte de la sucursal te encuentras?
    /// </summary>
    public int PartBranchId { get; set; }
    /// <summary>
    /// ID de Estatus
    /// </summary>
    [DefaultValue(1)]
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
    /// Quien lo creo
    /// </summary>
    public int CreatedBy { get; set; }
    /// <summary>
    /// Cuando se creo
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Quien Actualizo
    /// </summary>
    public int? UpdatedBy { get; set; }
    /// <summary>
    /// Fecha actualizado
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
    /// <summary>
    /// Fotografías
    /// </summary>
    public virtual ICollection<PhotoTicketDto> PhotoTickets { get; set; }
}