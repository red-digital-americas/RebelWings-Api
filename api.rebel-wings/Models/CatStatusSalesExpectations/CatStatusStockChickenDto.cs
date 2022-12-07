namespace api.rebel_wings.Models.CatStatusSalesExpectations
{
    public class CatStatusStockChickenDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Nombre del estatus
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Quien creo el estatus
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Fecha de Creación
        /// </summary>
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
