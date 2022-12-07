namespace api.rebel_wings.Models.SalesExpectations;
/// <summary>
/// Model
/// </summary>
public class SalesExpectationGet
{
    /// <summary>
    /// Sales Expectation Total
    /// </summary>
    public decimal? SalesExpectationTotal { get; set; }
    /// <summary>
    /// Total
    /// </summary>
    public decimal? AmountTotal { get; set; }
    /// <summary>
    /// Complete 
    /// </summary>
    public decimal? CompletePercentage { get; set; }
}