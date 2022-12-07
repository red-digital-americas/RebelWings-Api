namespace api.rebel_wings.Models.CashRegisterShortage
{
    /// <summary>
    /// Modelo de Volado de efectivo
    /// </summary>
    public class CashRegisterShortageDto
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
        /// Comentario
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Fecha de envio de la alarma
        /// </summary>
        public DateTime? AlarmTime { get; set; }
        /// <summary>
        /// Fecha de envio de la alarma
        /// </summary>
        public string ElapsedAlarmTime { get; set; }
        /// <summary>
        /// Quien lo Creo
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Cuando se creo
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Quien fue el ultimo en actualizar
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Fecha ultima de actualización
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        /// <summary>
        /// Colección de Fotos
        /// </summary>
        public virtual ICollection<PhotoCashRegisterShortageDto> PhotoCashRegisterShortages { get; set; }
    }
}
