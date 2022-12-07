namespace api.rebel_wings.Models.Ticketing;

public class TicketingDto
{
    public TicketingDto()
    {
        CommentTicketings = new HashSet<CommentTicketingDto>();
        PhotoTicketings = new HashSet<PhotoTicketingDto>();
    }

    public int Id { get; set; }
    public int BranchId { get; set; }
    public bool Status { get; set; }
    public int WhereAreYouLocated { get; set; }
    public string SpecificLocation { get; set; } = null!;
    public int Category { get; set; }
    public string NoTicket { get; set; } = null!;
    public DateTime DateOpen { get; set; }
    public DateTime? DateClosed { get; set; }
    public decimal? Cost { get; set; }
    public string DescribeProblem { get; set; } = null!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public virtual ICollection<CommentTicketingDto> CommentTicketings { get; set; }
    public virtual ICollection<PhotoTicketingDto> PhotoTicketings { get; set; }
}