namespace api.rebel_wings.Models.Alarm
{
    public class AlarmDto
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Comment { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<PhotoAlarmDto> PhotoAlarms { get; set; }
    }
}
