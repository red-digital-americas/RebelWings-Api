namespace api.rebel_wings.Models.WashBasinWithSoapPaper;
/// <summary>
/// Model Photos
/// </summary>
public class PhotoWashBasinWithSoapPaperDto
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// FK
    /// </summary>
    public int WashbasinWithSoapPaperId { get; set; }
    /// <summary>
    /// Photo
    /// </summary>
    public string Photo { get; set; } = null!;
    /// <summary>
    /// Photo Path
    /// </summary>
    public string PhotoPath { get; set; }
    public int Type { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
