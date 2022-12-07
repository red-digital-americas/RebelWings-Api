using api.rebel_wings.Models.User;

namespace api.rebel_wings.Models.Ticketing;
/// <summary>
/// Model
/// </summary>
public class TicketingGetDto
{
    /// <summary>
    /// Constructor
    /// </summary>
    public TicketingGetDto()
    {
        CommentTicketings = new HashSet<CommentTicketingDto>();
        PhotoTicketings = new HashSet<PhotoTicketingDto>();
    }
    /// <summary>
    /// PK
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// FK
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Status
    /// </summary>
    public bool Status { get; set; }
    /// <summary>
    /// Where are you located
    /// </summary>
    public int WhereAreYouLocated { get; set; }
    /// <summary>
    /// Specific location
    /// </summary>
    public string SpecificLocation { get; set; } = null!;
    /// <summary>
    /// Category
    /// </summary>
    public int Category { get; set; }
    /// <summary>
    /// No. Ticket
    /// </summary>
    public string NoTicket { get; set; } = null!;
    /// <summary>
    /// Date Open
    /// </summary>
    public DateTime DateOpen { get; set; }
    /// <summary>
    /// Date Closed
    /// </summary>
    public DateTime? DateClosed { get; set; }
    /// <summary>
    /// Cost
    /// </summary>
    public decimal? Cost { get; set; }
    /// <summary>
    /// Description Problem
    /// </summary>
    public string DescribeProblem { get; set; } = null!;
    /// <summary>
    /// Created By
    /// </summary>
    public int CreatedBy { get; set; }
    /// <summary>
    /// Created Date
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Updated By
    /// </summary>
    public int? UpdatedBy { get; set; }
    /// <summary>
    /// Updated date
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
    
    public virtual CatTicketingDto CategoryNavigation { get; set; } = null!;
    public virtual UserDto CreatedByNavigation { get; set; } = null!;
    public virtual CatBranchLocateDto WhereAreYouLocatedNavigation { get; set; } = null!;
    public virtual ICollection<CommentTicketingDto> CommentTicketings { get; set; }
    public virtual ICollection<PhotoTicketingDto> PhotoTicketings { get; set; }
}