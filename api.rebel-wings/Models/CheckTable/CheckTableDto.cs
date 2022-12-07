namespace api.rebel_wings.Models.CheckTable;
/// <summary>
/// Model
/// </summary>
public class CheckTableDto
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// FK
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Num Table
    /// </summary>
    public int NumTable { get; set; }
    /// <summary>
    /// One
    /// </summary>
    public bool ProductOne { get; set; }
    /// <summary>
    /// Comment One
    /// </summary>
    public string? CommentProductOne { get; set; }
    /// <summary>
    /// Two
    /// </summary>
    public bool ProductTwo { get; set; }
    /// <summary>
    /// Comment Two
    /// </summary>
    public string? CommentProductTwo { get; set; }
    /// <summary>
    /// Three
    /// </summary>
    public bool ProductThree { get; set; }
    /// <summary>
    /// Comment Three
    /// </summary>
    public string? CommentProductThree { get; set; }
    /// <summary>
    /// Four
    /// </summary>
    public bool ProductFour { get; set; }
    /// <summary>
    /// Comment Four
    /// </summary>
    public string? CommentProductFour { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}