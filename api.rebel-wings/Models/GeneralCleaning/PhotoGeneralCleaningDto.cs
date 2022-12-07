namespace api.rebel_wings.Models.GeneralCleaning;

public class PhotoGeneralCleaningDto
{
    public int Id { get; set; }
    public int GeneralCleaningId { get; set; }
    public string Photo { get; set; }
    public string PhotoPath { get; set; }
    public int Type { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}