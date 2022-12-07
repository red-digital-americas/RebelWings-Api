using System.ComponentModel;

namespace api.rebel_wings.Models.AudioVideo;
/// <summary>
/// Modelo de Audio & VIdeo
/// </summary>
public class AudioVideoDto
{
    public AudioVideoDto()
    {
        PhotoAudioVideos = new HashSet<PhotoAudioVideoDto>();
    }
    public int Id { get; set; }
    /// <summary>
    /// ID de sucursal
    /// </summary>
    public int BranchId { get; set; }
    /// <summary>
    /// ¿Funcionan de manera correcta todas las TVs en piso?
    /// </summary>
    public bool TvWorksProperly { get; set; }
    /// <summary>
    /// Comentario
    /// </summary>
    public string CommentTvWorksProperly { get; set; }
    /// <summary>
    /// ¿Funcionan de manera correcta todas las bocinas en piso?
    /// </summary>
    public bool SpeakersWorkProperly { get; set; }
    /// <summary>
    /// Comentario
    /// </summary>
    public string CommentSpeakersWorkProperly { get; set; }
    /// <summary>
    /// ¿Funcionan de manera correcta todas las TVs en terraza?
    /// </summary>
    public bool TerraceTvWorksProperly { get; set; }
    /// <summary>
    /// Comentario
    /// </summary>
    public string CommentTerraceTvWorksProperly { get; set; }
    /// <summary>
    /// ¿Funcionan de manera correcta todas las bocinas en terraza?
    /// </summary>
    public bool TerraceSpeakersWorkProperly { get; set; }
    /// <summary>
    /// Comentario
    /// </summary>
    public string CommentTerraceSpeakersWorkProperly { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public virtual ICollection<PhotoAudioVideoDto> PhotoAudioVideos { get; set; }
}
