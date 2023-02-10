namespace api.rebel_wings.Models.Remisiones;

public class TPedidoEntregaCalendarDto
{
    public int Id { get; set; }
    public int IdProvedor { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string title { get; set; }
    public DateTime? FechaProg { get; set; }
    public DateTime? FechaReal { get; set; }
    public string Comment { get; set; }
    public int Estatus { get; set; }
    public string EstatusName { get; set; }
    public int IdSucursal { get; set; }
    public string Nombre { get; set; }
    public ColorDto Color { get; set; }
    public bool Draggable { get; set; } 
    public virtual ICollection<TFotosPedidosEntregaDto> TFotosPedidosEntregas { get; set; }
}

public class ColorDto
{
    public string Primary { get; set; }
    public string Secondary { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }
}