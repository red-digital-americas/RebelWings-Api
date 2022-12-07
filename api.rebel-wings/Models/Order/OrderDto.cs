using System.ComponentModel;

namespace api.rebel_wings.Models.Order;

public class OrderDto
{
    public OrderDto()
    {
        PhotoOrders = new HashSet<PhotoOrderDto>();
    }
    [DefaultValue(0)]
    public int Id { get; set; }
    public int BranchId { get; set; }
    public int AverageTime { get; set; }
    public string Comment { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<PhotoOrderDto> PhotoOrders { get; set; }
}
