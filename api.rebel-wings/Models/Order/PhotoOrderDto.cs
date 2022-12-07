using System.ComponentModel;

namespace api.rebel_wings.Models.Order;

public class PhotoOrderDto
{
    [DefaultValue(0)]
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Photo { get; set; }
    public string PhotoPath { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}