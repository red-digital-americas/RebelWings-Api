namespace api.rebel_wings.Models.Ticketing;

public class CatTicketingDto
{
    public int Id { get; set; }
    public string Category { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}