namespace api.rebel_wings.Models.Spotlight;

public class SpotlightDto
{
    public SpotlightDto()
    {
        PhotoSpotlights = new HashSet<PhotoSpotlightDto>();
    }
    public int Id { get; set; }
    public int BranchId { get; set; }
    public bool BrokenSpotlight { get; set; }
    public bool IsThereOk { get; set; }
    public string CommentFoco { get; set; } = null!;
    public string CommentAutorizados { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public virtual ICollection<PhotoSpotlightDto> PhotoSpotlights { get; set; }
}
