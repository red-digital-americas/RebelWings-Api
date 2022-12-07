namespace api.rebel_wings.Models.EntriesChargedAsDeliveryNote;
/// <summary>
/// Model
/// </summary>
public class EntriesChargedAsDeliveryNoteDto
{
    /// <summary>
    /// Constructor
    /// </summary>
    public EntriesChargedAsDeliveryNoteDto()
    {
        PhotoEntriesChargedAsDeliveryNotes = new HashSet<PhotoEntriesChargedAsDeliveryNoteDto>();
    }
    /// <summary>
    /// ID ==> PK
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Branch
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    public string CommentDirectDeliveriesPerDay { get; set; } = null!;
    /// <summary>
    /// Revision Number
    /// </summary>
    public int RevisionNumber { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    public string CommentRevisionNumber { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    /// <summary>
    /// Photos
    /// </summary>
    public virtual ICollection<PhotoEntriesChargedAsDeliveryNoteDto> PhotoEntriesChargedAsDeliveryNotes { get; set; }
}