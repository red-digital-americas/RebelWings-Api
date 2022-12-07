using System.ComponentModel;

namespace api.rebel_wings.Models.CashRegisterShortage
{
    /// <summary>
    /// Modelo de fotos para Volado de efectivo
    /// </summary>
    public class PhotoCashRegisterShortageDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Llave foranea de Volado de efectivo
        /// </summary>
        public int CashRegisterShortageId { get; set; }
        /// <summary>
        /// Foto en Base64
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// Extensión de foto
        /// </summary>
        [DefaultValue("")]
        public string PhotoPath { get; set; }
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

    }
}
