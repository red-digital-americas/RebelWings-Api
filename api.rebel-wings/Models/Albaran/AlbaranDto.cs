namespace api.rebel_wings.Models.Albaran;
/// <summary>
/// Modelo de Albaran
/// </summary>
public class AlbaranDto
{
    /// <summary>
    /// ID => PK
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Sucursal
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// Fecha de Albaran viene de base de cliente, funciona como llave foranea para identificar el registro de su dato
    /// </summary>
    public DateTime AlbaranDate { get; set; }
    /// <summary>
    /// Hora de Albaran viene de base de cliente, funciona como llave foranea para identificar el registro de su dato
    /// </summary>
    public TimeSpan AlbaranTime { get; set; }
    /// <summary>
    /// Descripcion de Albaran 
    /// </summary>
    public string AlbaranDescription { get; set; }
    /// <summary>
    /// Numero de serie
    /// </summary>
    public string NumSerie { get; set; }
    /// <summary>
    /// Numero de Albaran
    /// </summary>
    public int NumAlbaran { get; set; }
    /// <summary>
    /// N => no idea
    /// </summary>
    public string N { get; set; }
    /// <summary>
    /// Estatus ID => FK
    /// </summary>
    public int StatusId { get; set; }
    /// <summary>
    /// Hora de arribo
    /// </summary>
    public TimeSpan TimeArrive { get; set; }
    /// <summary>
    /// Comentario
    /// </summary>
    public string Comment { get; set; }
    /// <summary>
    /// Creado Por
    /// </summary>
    public int CreatedBy { get; set; }
    /// <summary>
    /// Fecha de creación
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Actualizado Por
    /// </summary>
    public int? UpdatedBy { get; set; }
    /// <summary>
    /// Fecha de actualización
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
}