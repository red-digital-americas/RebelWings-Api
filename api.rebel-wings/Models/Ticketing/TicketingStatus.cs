namespace api.rebel_wings.Models.Ticketing;
/// <summary>
/// Model
/// </summary>
public class TicketingStatus
{
    /// <summary>
    /// Fecha de cierre
    /// </summary>
    public DateTime ClosedDate { get; set; }
    /// <summary>
    /// Estatus
    /// </summary>
    public bool Status { get; set; }
    /// <summary>
    /// ID User
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// Monto
    /// </summary>
    public decimal Cost { get; set; }
}