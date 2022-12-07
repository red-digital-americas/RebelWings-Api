using System.ComponentModel;

namespace api.rebel_wings.Models.Fridge;

public class FridgeDto
{
    public FridgeDto()
    {
        PhotoFridges = new HashSet<PhotoFridgeDto>();
    }
    [DefaultValue(0)]
    public int Id { get; set; }
    public int BranchId { get; set; }
    public string Comment { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<PhotoFridgeDto> PhotoFridges { get; set; }
}