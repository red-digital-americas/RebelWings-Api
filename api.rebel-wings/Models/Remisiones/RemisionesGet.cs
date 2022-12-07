namespace api.rebel_wings.Models.Remisiones;

public class RemisionesGet
{
    public RemisionesGet()
    {
        RemisionesDetails = new HashSet<RemisionesDetailGet>();
    }
    public string NumSerie { get; set; }
    public int NumAlbaran { get; set; }
    public DateTime? Fecha { get; set; }
    public virtual ICollection<RemisionesDetailGet> RemisionesDetails { get; set; }
}

public class RemisionesDetailGet
{
    public string? Referencia { get; set; }
    public string? Descripcion { get; set; }
    public double? UnidadTotal { get; set; }
    public double? Precio { get; set; }
    public double? Dto { get; set; }
    public double? Total { get; set; }
}