using System.ComponentModel;

namespace api.rebel_wings.Models.ControlCenter
{
    /// <summary>
    /// Modelo de Centro de Control
    /// </summary>
    public class ControlCenter
    {
        /// <summary>
        /// NOMBRE
        /// </summary>
        [DefaultValue("")]
        public string Name { get; set; }
        /// <summary>
        /// DESCRIPCIÓN
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// ¿Es Porcentaje o Completado?
        /// Completado es ==> FALSE
        /// Porcentaje es ==> TRUE
        /// </summary>
        public bool IsPercentageOrComplete { get; set; }
        /// <summary>
        /// ¿Esta completado?
        /// </summary>
        public bool IsComplete { get; set; }
        /// <summary>
        /// Porcentaje
        /// </summary>
        public decimal Percentage { get; set; }
        /// <summary>
        /// Color de estatus
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// ID de Task si es que existe
        /// </summary>
        [DefaultValue(0)]
        public int? Id { get; set; }
    }
}
