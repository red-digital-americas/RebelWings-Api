namespace api.rebel_wings.Models.Station;

public class StationDto
{
    public StationDto()
    {
        PhotoStations = new HashSet<PhotoStationDto>();
    }

    public int Id { get; set; }
    public int BranchId { get; set; }
    public bool Chips { get; set; }
    public bool Clean { get; set; }
    public bool CompleteAssembly { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<PhotoStationDto> PhotoStations { get; set; }
}