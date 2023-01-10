namespace api.rebel_wings.Models.PeopleCounting;

public class PeopleCountingDto
{
    public PeopleCountingDto()
    {
        PhotoPeoplesCountings = new HashSet<PhotoPeopleDto>();
    }
    public int Id { get; set; }
    public int BranchId { get; set; }
    public int Tables { get; set; }
    public int Dinners { get; set; }
    public string Comment { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<PhotoPeopleDto> PhotoPeoplesCountings { get; set; }
}
