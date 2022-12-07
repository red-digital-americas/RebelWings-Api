using System.ComponentModel;

namespace api.rebel_wings.Models.ToSetTable
{
  public class PhotoToSetTableDto
  {
    public int Id { get; set; }
    public int ToSetTableId { get; set; }
    public string Photo { get; set; } = null!;
    public string? PhotoPath { get; set; }
    public int Type { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
  }
}
