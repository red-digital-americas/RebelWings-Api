namespace api.rebel_wings.Models.ValidateAttendance
{
    /// <summary>
    /// Modelo de Catalogo de Validación de Asistencia
    /// </summary>
    public class CatStatusValidateAttendanceDto
    {
        /// <summary>
        /// ID => PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nombre del estatus
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Quien creo el registro
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Fecha de Creación 
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
    }
}
