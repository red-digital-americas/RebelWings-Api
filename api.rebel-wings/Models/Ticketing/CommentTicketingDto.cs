namespace api.rebel_wings.Models.Ticketing;

public class CommentTicketingDto
{
    public int Id { get; set; }
    public int TicketingId { get; set; }
    public string Comment { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}