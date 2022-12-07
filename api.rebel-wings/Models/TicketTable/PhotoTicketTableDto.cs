namespace api.rebel_wings.Models.TicketTable;
/// <summary>
/// Model
/// </summary>
public class PhotoTicketTableDto
{
    /// <summary>
    /// ID ==> PK
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// FK
    /// </summary>
    public int TicketTableId { get; set; }
    /// <summary>
    /// Type
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// Photo
    /// </summary>
    public string Photo { get; set; } = null!;
    /// <summary>
    /// Path
    /// </summary>
    public string PhotoPath { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}