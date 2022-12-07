namespace api.rebel_wings.Models.TicketTable;
/// <summary>
/// Model
/// </summary>
public class TicketTableDto
{
    /// <summary>
    /// Constructor
    /// </summary>
    public TicketTableDto()
    {
        PhotoTicketTables = new HashSet<PhotoTicketTableDto>();
    }
    /// <summary>
    /// ID ==> PK
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// FK
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    public string CommentFoodOnTable { get; set; } = null!;
    /// <summary>
    /// Comment
    /// </summary>
    public string CommentTicket { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    /// <summary>
    /// Photos
    /// </summary>
    public virtual ICollection<PhotoTicketTableDto> PhotoTicketTables { get; set; }
}