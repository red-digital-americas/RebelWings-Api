using System.ComponentModel;

namespace api.rebel_wings.Models.Tip
{
    /// <summary>
    /// Modelo de Fotos de Propinas
    /// </summary>
    public class PhotoTipDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID de Propinas => FK
        /// </summary>
        public int TipId { get; set; }
        /// <summary>
        /// Foto Base64
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// Extensión de foto
        /// </summary>
        [DefaultValue("")]
        public string PhotoPath { get; set; }
        /// <summary>
        /// Fecha de creación
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

    }
}
