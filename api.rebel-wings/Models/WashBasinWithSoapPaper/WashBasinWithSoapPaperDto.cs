namespace api.rebel_wings.Models.WashBasinWithSoapPaper;
/// <summary>
/// Model WashBasin With Soap & Paper
/// </summary>
public class WashBasinWithSoapPaperDto
{
    /// <summary>
    /// Constructor
    /// </summary>
    public WashBasinWithSoapPaperDto()
    {
        PhotoWashBasinWithSoapPapers = new HashSet<PhotoWashBasinWithSoapPaperDto>();
    }
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// FK
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Is There a Soap and Paper
    /// </summary>
    public bool IsThereSoapPaper { get; set; }
    /// <summary>
    /// Is There a Soap and Paper
    /// </summary>
    public bool IsThereDryer { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    public string CommentSoapPaper { get; set; } = null!;
    public string CommentDryer { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    /// <summary>
    /// Collection Photos
    /// </summary>
    public virtual ICollection<PhotoWashBasinWithSoapPaperDto> PhotoWashBasinWithSoapPapers { get; set; }
}
