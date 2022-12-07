namespace api.rebel_wings.Models.User
{
    /// <summary>
    /// Modelo de Usuario
    /// </summary>
    public class UserDto
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
        public string Token { get; set; }
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
        public string MotherName { get; set; }
        /// <summary>
        /// Rol de usuario
        /// </summary>
        public int RoleId { get; set; }
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
    /// <summary>
    /// Modelo de Usuario de Restorno
    /// </summary>
    public class UserReturnDto
    {
        /// <summary>
        /// ID de Usuario
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Correo
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Contraseña
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Clave de Trabajador
        /// </summary>
        public int? ClabTrab { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Nombre
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Apellido Paterno
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Apellido Materno
        /// </summary>
        public string MotherName { get; set; }
        public int RoleId { get; set; }
        public int? StateId { get; set; }
        public int? SucursalId { get; set; }
        public string DataBase { get; set; }
        public string SucursalName { get; set; }
        /// <summary>
        /// Nombre de sucursal
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// ID de Sucursal
        /// </summary>
        public int Branch { get; set; }
        public int? BranchId { get; set; }
    }
}
