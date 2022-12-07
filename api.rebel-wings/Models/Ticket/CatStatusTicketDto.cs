namespace api.rebel_wings.Models.Ticket;

public class CatStatusTicketDto
{
    public int Id { get; set; }
    public string Status { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}