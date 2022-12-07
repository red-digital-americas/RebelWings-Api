namespace api.rebel_wings.Models.Remisiones
{
    public class TPedidoEntregaDto
    {
        public int Id { get; set; }
        public int IdProveedor { get; set; }
        public string ProveedorName { get; set; }
        public DateTime? FechaProgPedido { get; set; }
        public DateTime? FechaPedidoReal { get; set; }
        public DateTime? FechaProgEntrega { get; set; }
        public DateTime? FechaEntregaReal { get; set; }
        public string ComentariosPedido { get; set; }
        public string ComentariosEntrega { get; set; }
        public int EstatusEntrega { get; set; }
        public string EstatusEntregaName { get; set; }
        public int EstatusPedido { get; set; }
        public string EstatusPedidoName { get; set; }
        public int IdSucursal { get; set; }
        public virtual ICollection<TFotosPedidosEntregaDto> TFotosPedidosEntregas { get; set; }
    }

    public class TPedidoEntregaUpadteDto
    {
        public int Id { get; set; }
        public int IdProveedor { get; set; }
        public DateTime? FechaProgPedido { get; set; }
        public DateTime? FechaPedidoReal { get; set; }
        public DateTime? FechaProgEntrega { get; set; }
        public DateTime? FechaEntregaReal { get; set; }
        public string ComentariosPedido { get; set; }
        public string ComentariosEntrega { get; set; }
        public int EstatusEntrega { get; set; }
        public int EstatusPedido { get; set; }
        public int IdSucursal { get; set; }
    }
}
