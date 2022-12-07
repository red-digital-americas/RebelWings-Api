using System.ComponentModel;

namespace api.rebel_wings.Models.ValidateAttendance
{
    /// <summary>
    /// Modelo para Arreglo de lista de trabajadores por Sucursal
    /// </summary>
    public class ValidateAttendanceList
    {
        /// <summary>
        /// ID de validación de asistencia de día en caso de que exista
        /// </summary>
        [DefaultValue(0)]
        public int AttendanceId { get; set; }
        /// <summary>
        /// Imagen de Trabajador
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// Nombre de trabajador
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Puesto Laboral
        /// </summary>
        public string JobTitle { get; set; }
        /// <summary>
        /// Turno 
        /// </summary>
        public string Workshift { get; set; }
        /// <summary>
        /// Retardos
        /// </summary>
        public int TimeDelay { get; set; }
        /// <summary>
        /// Estatus de validación de asistencia INFO viene de FORTIA
        /// </summary>
        public string ValidateAttendance { get; set; }
        /// <summary>
        /// Clabe de trabajador
        /// </summary>
        public int ClabTrab { get; set; }
        /// <summary>
        /// Estatus de Si llegó o No llegó
        /// id |	status
        /// ---------------
        /// 1  |	Llegó
        /// 2  |	No Llegó
        /// 3  |	Retardo
        /// </summary>
        public int Status { get; set; }

    }
}
