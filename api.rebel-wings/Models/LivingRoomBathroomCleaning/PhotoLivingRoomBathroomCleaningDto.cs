using System.ComponentModel;

namespace api.rebel_wings.Models.LivingRoomBathroomCleaning
{
    /// <summary>
    /// Modelo de Fotos 
    /// </summary>
    public class PhotoLivingRoomBathroomCleaningDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID de Limpieza de salón y baños =>FK
        /// </summary>
        public int LivingRoomBathroomCleaningId { get; set; }
        /// <summary>
        /// Fotp
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 
        /// </summary>

        [DefaultValue("")]
        public string PhotoPath { get; set; }
        /// <summary>
        /// Quien creo
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// Cuando se creo
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Quien actualizo
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Cuando se actualizo
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

    }
}
