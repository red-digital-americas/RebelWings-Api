using System.ComponentModel;
namespace api.rebel_wings.Models.WaitlistTable
{
  /// <summary>
  /// Modelo de Validación de Mesas en espera
  /// </summary>
  public class WaitlistTableDto
    {
        /// <summary>
        /// WaitlistTable
        /// </summary>
        public WaitlistTableDto()
        {
            PhotoWaitlistTables = new HashSet<PhotoWaitlistTableDto>();
        }
        [DefaultValue(0)]
        /// <summary>
        /// ID identificador de registro
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID de Sucursal
        /// </summary>
        public int Branch { get; set; }
        /// <summary>
        /// ¿Hubo mesas en espera?
        /// </summary>
        public bool WaitlistTables { get; set; }
        /// <summary>
        /// ¿Cuántas mesas hubo en espera?
        /// </summary>
        public int? HowManyTables { get; set; }
        /// <summary>
        /// Número de personas en espera por mesa
        /// </summary>
        public int? NumberPeople { get; set; }
        /// <summary>
        /// ID de usuario que creo el registro
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Fecha de creación de registro
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// FID de usuario que actualizo el registro
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Fecha de actualización de registro
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
        /// <summary>
        /// Comentario
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Arreglo de fotos 
        /// </summary>
        public virtual ICollection<PhotoWaitlistTableDto> PhotoWaitlistTables { get; set; }
    }
}
