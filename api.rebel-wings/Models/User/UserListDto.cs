namespace api.rebel_wings.Models.User
{
    public class UserListDto
    {
        /// <summary>
        /// ID de usuario
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Correo electronico
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Contraseña 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Clabe de trbajador que viene de FORTIA
        /// </summary>
        public int? ClabTrab { get; set; }
        /// <summary>
        /// TOKEN
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// Nombre 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Apellido
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Apellido
        /// </summary>
        public string? MotherName { get; set; }
        /// <summary>
        /// Rol de usuario
        /// </summary>
        public int? RoleId { get; set; }
        public int? StateId { get; set; }
        public int? SucursalId { get; set; }
        public int? BranchId { get; set; }
        /// <summary>
        /// Quien creo el registro
        /// </summary>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// Fecha de creación 
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Quien actualizo el registro
        /// </summary>
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Fecha de actualización de registro
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
    }
}
