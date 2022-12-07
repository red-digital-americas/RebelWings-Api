namespace biz.bd2.Models;

public class Albaranes
{
    public string NumSerie { get; set; }
    public int NumAlbaran { get; set; }
    public string N { get; set; }
    public string NombreCliente { get; set; }
    public string NombreComercial { get; set; }
    public string Descripcion { get; set; }
    public DateTime AlbaranDate { get; set; }
    public TimeSpan AlbaranTime { get; set; }
    public string Status { get; set; }
    public int? Id { get; set; }
}