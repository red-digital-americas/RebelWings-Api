namespace api.rebel_wings.Models.Remisiones
{
    public class TEstatusPedidosEntregaDto
    {
        public int Id { get; set; }
        public string Estatus { get; set; }
        public string Tipo { get; set; }
        public bool Active { get; set; }
    }
}