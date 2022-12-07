using System.ComponentModel;

namespace api.rebel_wings.Models.ValidationGas
{
    /// <summary>
    /// Modelo de Validación de Gas
    /// </summary>
    public class ValidationGaDto
    {
        /// <summary>
        /// ValidationGaDto
        /// </summary>
        public ValidationGaDto()
        {
            PhotoValidationGas = new HashSet<PhotoValidationGaDto>();
        }
        [DefaultValue(0)]
        public int Id { get; set; }
        /// <summary>
        /// Sucursal
        /// </summary>
        public int? Branch { get; set; }
        /// <summary>
        /// Cantidad de Litros
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Comentario
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Quien creo el registro
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Fecha de creación de regitro
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Quien actualizo el registro
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Fecha de actualización de registro
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        /// <summary>
        /// Arreglo de fotos 
        /// </summary>
        public virtual ICollection<PhotoValidationGaDto> PhotoValidationGas { get; set; }
    }
}
