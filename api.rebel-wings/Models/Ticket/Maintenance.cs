namespace api.rebel_wings.Models.Ticket;

public class Maintenance
{
    /// <summary>
    /// Nombre de Sucursal
    /// </summary>
    public string BranchName { get; set; }
    /// <summary>
    /// Id de Sucursal
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Nombre de Usuario
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// ¿En qué parte de la sucursal te encuentras?
    /// </summary>
    public string PartBranch { get; set; }
    /// <summary>
    /// Lugar específico
    /// </summary>
    public string SpecificSection { get; set; }
    /// <summary>
    /// Nombre de Estatus
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// Ticket ID
    /// </summary>
    public int Id { get; set; }
}