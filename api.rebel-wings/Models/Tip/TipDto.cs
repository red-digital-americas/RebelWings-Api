namespace api.rebel_wings.Models.Tip
{
    /// <summary>
    /// Clase de Resguardo de Propina
    /// </summary>
    public class TipDto
    {
        /// <summary>
        /// ID 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID de Sucursal
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// Monto
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Comentarios
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Quien creo el registro
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Quien actualizo el registro
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Fecha de actualización
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        /// <summary>
        /// Colección de fotos de resguardo de propinas
        /// </summary>
        public virtual ICollection<PhotoTipDto> PhotoTips { get; set; }
    }
}
