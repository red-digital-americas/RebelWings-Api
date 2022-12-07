namespace api.rebel_wings.Models.Ticketing;

public class TicketingGet
{
    /// <summary>
    /// ID Ticketing
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// No. Ticketing
    /// </summary>
    public string NoTicketing { set; get; }
    /// <summary>
    /// Status
    /// </summary>
    public bool Status { get; set; }
    /// <summary>
    /// Opened Date
    /// </summary>
    public DateTime Opened { get; set; }
    /// <summary>
    /// Closed Date
    /// </summary>
    public DateTime? Closed { get; set; }
    /// <summary>
    /// Regional
    /// </summary>
    public string Regional { get; set; }
    /// <summary>
    /// Category
    /// </summary>
    public string Category { get; set; }
    /// <summary>
    /// Branch
    /// </summary>
    public string? Branch { get; set; }
    /// <summary>
    /// Branch FK
    /// </summary>
    public int BranchId { get; set; }
}