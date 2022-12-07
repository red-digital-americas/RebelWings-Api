namespace api.rebel_wings.Models.OrderScheduleReview;

public class PhotoOrderScheduleReviewDto
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Fk
    /// </summary>
    public int? OrderScheduleReviewId { get; set; }
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