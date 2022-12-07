using System.ComponentModel;

namespace api.rebel_wings.Models.Ticket;
/// <summary>
/// Modelo de Fotografías de Ticket
/// </summary>
public class PhotoTicketDto
{
    /// <summary>
    /// ID => PK
    /// </summary>
    [DefaultValue(0)]
    public int Id { get; set; }
    /// <summary>
    /// FK => Levantamiento de ticket
    /// </summary>
    public int TicketId { get; set; }
    /// <summary>
    /// Fotografía (Base64)
    /// </summary>
    public string Photo { get; set; }
    /// <summary>
    /// Extensión de fotografía
    /// </summary>
    public string PhotoPath { get; set; }
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
}