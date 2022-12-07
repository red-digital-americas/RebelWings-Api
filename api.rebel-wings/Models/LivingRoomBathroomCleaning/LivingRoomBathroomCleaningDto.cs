namespace api.rebel_wings.Models.LivingRoomBathroomCleaning
{
    /// <summary>
    /// Modelo de Limpieza de Baños y salón
    /// </summary>
    public class LivingRoomBathroomCleaningDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID de Sucursal 
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// Comentario
        /// </summary>
        public string Comment { get; set; }
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
        /// <summary>
        /// Colleción de fotos
        /// </summary>
        public virtual ICollection<PhotoLivingRoomBathroomCleaningDto> PhotoLivingRoomBathroomCleanings { get; set; }
    }
}
