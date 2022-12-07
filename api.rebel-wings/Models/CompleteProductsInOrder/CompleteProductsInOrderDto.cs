namespace api.rebel_wings.Models.CompleteProductsInOrder;

public class CompleteProductsInOrderDto
{
    public CompleteProductsInOrderDto()
    {
        PhotoCompleteProductsInOrders = new HashSet<PhotoCompleteProductsInOrderDto>();
    }

    public int Id { get; set; }
    public int BranchId { get; set; }
    public string Code { get; set; }
    public string Comment { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<PhotoCompleteProductsInOrderDto> PhotoCompleteProductsInOrders { get; set; }
}