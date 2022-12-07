namespace api.rebel_wings.Models.EntriesChargedAsDeliveryNote;
/// <summary>
/// Model
/// </summary>
public class PhotoEntriesChargedAsDeliveryNoteDto
{
    /// <summary>
    /// ID ==> PK
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// FK
    /// </summary>
    public int EntriesChargedAsDeliveryNoteId { get; set; }
    /// <summary>
    /// Type
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// Photo
    /// </summary>
    public string Photo { get; set; } = null!;
    /// <summary>
    /// PAth
    /// </summary>
    public string PhotoPath { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}