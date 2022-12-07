namespace api.rebel_wings.Models.PrecookedChicken;
/// <summary>
/// Modelo de Pollo Precocido
/// </summary>
public class PrecookedChickenDto
{
    public PrecookedChickenDto()
    {
        PhotoPrecookedChickens = new HashSet<PhotoPrecookedChickenDto>();
    }

    public int Id { get; set; }
    /// <summary>
    /// ID de Sucursal
    /// </summary>
    public int BranchId { get; set; }
    public bool PrecookedChickenOnTheTable { get; set; }
    public int? AmountPrecookedChickenOnTheTable { get; set; }
    public int? AmountPrecookedChickenInGeneral { get; set; }
    public bool PrecookedChickenInGeneral { get; set; }
    public int? AmountBonelesOrPrecookedPotatoes { get; set; }
    public bool BonelesOrPrecookedPotatoes { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    public string? CommentPrecookedChickenOnTheTable { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    public string? CommentPrecookedChickenInGeneral { get; set; }
    /// <summary>
    /// Comment
    /// </summary>
    public string? CommentBonelesOrPrecookedPotatoes { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<PhotoPrecookedChickenDto> PhotoPrecookedChickens { get; set; }
}