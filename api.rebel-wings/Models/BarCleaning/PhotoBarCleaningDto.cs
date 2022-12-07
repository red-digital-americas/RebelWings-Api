namespace api.rebel_wings.Models.BarCleaning;

public class PhotoBarCleaningDto
{
    public int Id { get; set; }
    public int? BarCleaningId { get; set; }
    public string Photo { get; set; } = null!;
    public string PhotoPath { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
