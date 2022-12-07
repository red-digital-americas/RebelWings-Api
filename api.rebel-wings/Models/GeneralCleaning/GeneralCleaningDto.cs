namespace api.rebel_wings.Models.GeneralCleaning;

public class GeneralCleaningDto
{
    public GeneralCleaningDto()
    {
        PhotoGeneralCleanings = new HashSet<PhotoGeneralCleaningDto>();
    }

    public int Id { get; set; }
    public int BranchId { get; set; }
    public string TableN { get; set; }
    public bool CleanlinessInSalon { get; set; }
    public bool CleaningInBuckets { get; set; }
    public bool CleaningBooths { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<PhotoGeneralCleaningDto> PhotoGeneralCleanings { get; set; }
}