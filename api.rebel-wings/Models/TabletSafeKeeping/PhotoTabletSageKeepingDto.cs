using System.ComponentModel;

namespace api.rebel_wings.Models.TabletSafeKeeping
{
    public class PhotoTabletSageKeepingDto
    {
        public int Id { get; set; }
        public int TabletSafeKeepingId { get; set; }
        public string Photo { get; set; }

        [DefaultValue("")]
        public string PhotoPath { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
