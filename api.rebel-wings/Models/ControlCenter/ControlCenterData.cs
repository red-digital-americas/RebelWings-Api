namespace api.rebel_wings.Models.ControlCenter;

public class ControlCenterData
{
    public decimal Progress { get; set; }
    public virtual ICollection<ControlCenter> ControlCenters { get; set; }
}