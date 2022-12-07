using System.ComponentModel;

namespace api.rebel_wings.Models.ToSetTable
{
    public class ToSetTableDto
    {
        public ToSetTableDto()
        {
          PhotoToSetTables = new HashSet<PhotoToSetTableDto>();
        }
        public int Id { get; set; }
        /// <summary>
        /// Sucursal
        /// </summary>
        [Description("Sucursal")]
        public int? Branch { get; set; }

        public string CommentSalon { get; set; } 
        public string CommentCocina { get; set; } 
        public string CommentMeet { get; set; } 
        public int Cajeros { get; set; }
        public decimal Vendedores { get; set; }
        public decimal Cocineros { get; set; }
        public decimal CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<PhotoToSetTableDto> PhotoToSetTables { get; set; }

    }
}
