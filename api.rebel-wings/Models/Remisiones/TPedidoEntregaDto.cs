namespace api.rebel_wings.Models.Remisiones
{
    public class TPedidoEntregaDto
    {
        public int Id { get; set; }
        public string? ProveedorName { get; set; }
        public int IdProveedor { get; set; }
        public DateTime? FechaProg { get; set; }
        public DateTime? FechaReal { get; set; }
        public string Comentarios { get; set; }
        public int Estatus { get; set; }
        public string? EstatusName { get; set; }
        public string? EstatusType { get; set; }
        public int IdSucursal { get; set; }
        public virtual ICollection<TFotosPedidosEntregaDto> TFotosPedidosEntregas { get; set; }
    }

    public class TPedidoEntregaUpadteDto
    {
        public int Id { get; set; }
        public int IdProveedor { get; set; }
        public DateTime? FechaProg { get; set; }
        public DateTime? FechaReal { get; set; }
        public string Comentarios { get; set; }
        public int Estatus { get; set; }
        public int IdSucursal { get; set; }
        public virtual ICollection<TFotosPedidosEntregaUpdateUpdateDto> TFotosPedidosEntregas { get; set; }

    }
    
    public class TFotosPedidosEntregaUpdateUpdateDto
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public string Foto { get; set; }
        public string Tipo { get; set; }
    }
}
