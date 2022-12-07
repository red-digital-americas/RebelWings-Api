namespace biz.bd1.Models;

public class Remisiones
{
    public Remisiones()
    {
        RemisionesDetails = new HashSet<RemisionesDetail>();
    }
    public string NumSerie { get; set; }
    public int NumAlbaran { get; set; }
    public DateTime? Fecha { get; set; }
    public virtual ICollection<RemisionesDetail> RemisionesDetails { get; set; }
}

public class RemisionesDetail
{
    public string? Referencia { get; set; }
    public string? Descripcion { get; set; }
    public double? UnidadTotal { get; set; }
    public double? Precio { get; set; }
    public double? Dto { get; set; }
    public double? Total { get; set; }
}