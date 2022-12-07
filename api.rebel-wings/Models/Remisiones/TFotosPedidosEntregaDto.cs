namespace api.rebel_wings.Models.Remisiones
{
    public class TFotosPedidosEntregaDto
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public string Foto { get; set; }
        public string Tipo { get; set; }
    }
}