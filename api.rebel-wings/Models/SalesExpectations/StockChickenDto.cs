using System.ComponentModel;

namespace api.rebel_wings.Models.SalesExpectations
{
    public class StockChickenDto
    {
        [DefaultValue(0)]
        public int Id { get; set; }
        /// <summary>
        /// Sucursal
        /// </summary>
        public int? Branch { get; set; }
        /// <summary>
        /// Código de Producto
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Cantidad de Kilos
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Status de Producto
        /// </summary>
        [DefaultValue(1)]
        public int StatusId { get; set; }
        /// <summary>
        /// Quien Creo el registro
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Ultimo usuario que actualizo el registro
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Fecha ultima de actualización
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
    }
}
