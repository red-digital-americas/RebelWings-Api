using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.fortia.Models
{
    public class AttendanceList
    {
        /// <summary>
        /// Clabe de trabajador
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Turno
        /// </summary>
        public string Workshift { get; set; }
        /// <summary>
        /// Nombre Completo
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Foto de trabajador
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// Puesto
        /// </summary>
        public string JobTitle { get; set; }

    }
}
