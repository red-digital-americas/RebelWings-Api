using System.ComponentModel;

namespace api.rebel_wings.Models.ValidateAttendance
{
    /// <summary>
    /// Modelo de Validación de Asistencia
    /// </summary>
    public class ValidateAttendanceDto
    {
        public ValidateAttendanceDto()
        {
            PhotoValidateAttendances = new HashSet<PhotoValidateAttendanceDto>();
        }       
        /// <summary>
        /// ID => PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID de Sucursal
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// ID de estatus de Validación de asistencia
        /// </summary>
        public int Attendence { get; set; }
        /// <summary>
        /// Clave de trabajador
        /// </summary>
        public int ClabTrab { get; set; }
        /// <summary>
        /// Fecha y Hora de Entrada
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// Comentario
        /// </summary>
        [DefaultValue("")]
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
        /// Fecha de ultima actualización
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<PhotoValidateAttendanceDto> PhotoValidateAttendances { get; set; }
    }
}
