using System.ComponentModel;

namespace api.rebel_wings.Models.FryerCleaning;

public class PhotoFryerCleaningDto
{
    public int Id { get; set; }
    public int FryerCleaningId { get; set; }
    public string Photo { get; set; }
    [DefaultValue("")]
    public string PhotoPath { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}