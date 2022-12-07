namespace api.rebel_wings.Models.BathRoomsOverallStatus;
/// <summary>
/// Model
/// </summary>
public class BathRoomsOverallStatusDto
{

    public BathRoomsOverallStatusDto()
    {
        PhotoBathRoomsOverallStatuss = new HashSet<PhotoBathRoomsOverallStatusDto>();
    }
    /// <summary>
    /// PK
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// FK
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Does Any Bathroom Leak Men
    /// </summary>
    public bool DoesAnyBathroomLeakMen { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    public string? CommentDoesAnyBathroomLeakMen { get; set; }
    /// <summary>
    /// Is There Any Faults Men
    /// </summary>
    public bool IsThereAnyFaultsMen { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    public string? CommentAreThereAnyFaultsMen { get; set; }
    /// <summary>
    /// Does Any Bathroom Leak Women
    /// </summary>
    public bool DoesAnyBathroomLeakWomen { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    public string? CommentDoesAnyBathroomLeakWomen { get; set; }
    /// <summary>
    /// Is There Any Faults Women
    /// </summary>
    public bool IsThereAnyFaultsWomen { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    public string? CommentAreThereAnyFaultsWomen { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public virtual ICollection<PhotoBathRoomsOverallStatusDto> PhotoBathRoomsOverallStatuss { get; set; }
}
