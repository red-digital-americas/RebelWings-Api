namespace api.rebel_wings.Models.SalesExpectations
{
    public class StockChickenGetDto
    {
        /// <summary>
        /// ID de Stock de Pollo
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Código de Producto
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Cantidad
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Estatus de Stock de Pollo
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Fecha de Creación 
        /// </summary>
        public DateTime Created { get; set; }
    }
}
