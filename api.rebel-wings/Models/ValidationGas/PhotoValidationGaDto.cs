using System.ComponentModel;

namespace api.rebel_wings.Models.ValidationGas
{
    /// <summary>
    /// Modelo de Foto para Validación de Gas
    /// </summary>
    public class PhotoValidationGaDto
    {
        /// <summary>
        /// ID => PK
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Llave foranea de Validación de Gas
        /// </summary>
        public int ValidationGasId { get; set; }
        /// <summary>
        /// Foto en Base 64 para crear, se retorna el path de la imagen en el GET
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// Extensión de Foto
        /// </summary>
        [DefaultValue("")]
        public string PhotoPath { get; set; }
        /// <summary>
        /// Quien creo el registro
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Fecha de creación de registro
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Quien actualizo el registro
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
    }
}
