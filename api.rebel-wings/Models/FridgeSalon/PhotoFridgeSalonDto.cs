namespace api.rebel_wings.Models.FridgeSalon;

public class PhotoFridgeSalonDto
{
    public int Id { get; set; }
    public int FridgeSalonId { get; set; }
    public string Photo { get; set; } = null!;
    public string PhotoPath { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}