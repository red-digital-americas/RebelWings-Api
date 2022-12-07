namespace api.rebel_wings.Models.TabletSafeKeeping
{
    public class TabletSafeKeepingDto
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
        /// <summary>
        /// Colección de Fotos
        /// </summary>
        public virtual ICollection<PhotoTabletSageKeepingDto> PhotoTabletSageKeepings { get; set; }
    }
}
