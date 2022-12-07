namespace api.rebel_wings.Models.Station;

public class PhotoStationDto
{
    public int Id { get; set; }
    public int StationId { get; set; }
    public string Photo { get; set; }
    public string PhotoPath { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}