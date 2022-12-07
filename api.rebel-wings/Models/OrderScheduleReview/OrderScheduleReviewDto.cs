namespace api.rebel_wings.Models.OrderScheduleReview;
/// <summary>
/// Model
/// </summary>
public class OrderScheduleReviewDto
{
    /// <summary>
    /// Constructor
    /// </summary>
    public OrderScheduleReviewDto()
    {
        PhotoOrderScheduleReviews = new HashSet<PhotoOrderScheduleReviewDto>();
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
    /// Comment
    /// </summary>
    public string Comment { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    /// <summary>
    /// Collection Photos
    /// </summary>
    public virtual ICollection<PhotoOrderScheduleReviewDto> PhotoOrderScheduleReviews { get; set; }
}