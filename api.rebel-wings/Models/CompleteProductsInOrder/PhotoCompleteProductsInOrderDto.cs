namespace api.rebel_wings.Models.CompleteProductsInOrder;

public class PhotoCompleteProductsInOrderDto
{
    public int Id { get; set; }
    public int CompleteProductsInOrderId { get; set; }
    public string Photo { get; set; }
    public string PhotoPath { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}