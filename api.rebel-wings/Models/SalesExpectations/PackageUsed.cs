namespace api.rebel_wings.Models.SalesExpectations
{
    public class PackageUsed
    {
        /// <summary>
        /// ID de paquete
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Cantidad Usada
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// ID de usuario que esta agregando 
        /// </summary>
        public int UserId { get; set; } 

    }
}
