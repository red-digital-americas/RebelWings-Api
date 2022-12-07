namespace api.rebel_wings.Models.BarCleaning;

public class BarCleaningDto
{
    public BarCleaningDto()
    {
        PhotoBarCleanings = new HashSet<PhotoBarCleaningDto>();
    }
    public int Id { get; set; }
    public int BranchId { get; set; }
    public bool IsClean { get; set; }
    public string Comment { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<PhotoBarCleaningDto> PhotoBarCleanings { get; set; }
}
