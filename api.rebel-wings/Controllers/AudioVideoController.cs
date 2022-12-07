using api.rebel_wings.ActionFilter;
using api.rebel_wings.Extensions;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.AudioVideo;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.AudioVideo;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controlador de Audio & Video
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class AudioVideoController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IAudioVideoRepository _audioVideoRepository;
    private readonly IPhotoAudioVideoRepository _photoAudioVideoRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="audioVideoRepository"></param>
    public AudioVideoController(IMapper mapper, ILoggerManager loggerManager,
        IAudioVideoRepository audioVideoRepository, IPhotoAudioVideoRepository photoAudioVideoRepository)
    {
        _mapper = mapper;
        _loggerManager = loggerManager;
        _audioVideoRepository = audioVideoRepository;
        _photoAudioVideoRepository = photoAudioVideoRepository;
    }
    /// <summary>
    /// GET:
    /// Obtener por ID de Station
    /// </summary>
    /// <param name="id">ID </param>
    /// <returns></returns>
    [HttpGet("By-Id/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<AudioVideoDto>>> GetById(int id)
    {
        ApiResponse<AudioVideoDto> response = new ApiResponse<AudioVideoDto>();
        try
        {
            response.Result = _mapper.Map<AudioVideoDto>(_audioVideoRepository.GetAllIncluding(g=>g.PhotoAudioVideos).FirstOrDefault(f => f.Id == id));
            response.Message = "Operation was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.Message;
            return StatusCode(500, response);
        }
        return StatusCode(200, response);
    }
    /// <summary>
    /// GET:
    /// Regresa Audio & Video objeto
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<AudioVideoDto>>> Get(int id, int user)
  {
        var response = new ApiResponse<AudioVideoDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _audioVideoRepository.GetAllIncluding(i => i.PhotoAudioVideos);
            var @default = await order.FirstOrDefaultAsync(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<AudioVideoDto>(@default);
            response.Message = "Consult was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(200, response);
    }
    /// <summary>
    /// POST:
    /// Agregar un audio & video 
    /// </summary>
    /// <param name="audioVideoDto">Objeto a agregar</param>
    /// <returns>Regresa objeto agregado</returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<AudioVideoDto>>> Post([FromBody] AudioVideoDto audioVideoDto)
    {
        var response = new ApiResponse<AudioVideoDto>();
        try
        {   
            foreach (var item in audioVideoDto.PhotoAudioVideos)
            {
                if (_photoAudioVideoRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoAudioVideoRepository.UploadImageBase64(item.Photo, "Files/Audio/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var av = await _audioVideoRepository.AddAsyn(_mapper.Map<AudioVideo>(audioVideoDto));
            response.Result = _mapper.Map<AudioVideoDto>(av);
            response.Message = "Add was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(201, response);
    }
    /// <summary>
    /// PUT:
    /// Actualizar Audio & Video 
    /// </summary>
    /// <param name="audioVideoDto">Objeto a actualizar</param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<AudioVideoDto>>> Put([FromBody] AudioVideoDto audioVideoDto)
    {
        var response = new ApiResponse<AudioVideoDto>();
        try
        {
            foreach (var item in audioVideoDto.PhotoAudioVideos)
            {
                if (_photoAudioVideoRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.AudioVideoId = audioVideoDto.Id;
                    item.Photo = _photoAudioVideoRepository.UploadImageBase64(item.Photo, "Files/Audio/", item.PhotoPath);
                    await _photoAudioVideoRepository.AddAsyn(_mapper.Map<PhotoAudioVideo>(item));
                }
                else if(item.Id != 0 && item.Photo.Length < 251)
                {
                    continue;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var update = await _audioVideoRepository.UpdateAsync(_mapper.Map<AudioVideo>(audioVideoDto),
                audioVideoDto.Id);
            response.Result = _mapper.Map<AudioVideoDto>(update);
            response.Message = "Updated was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(200, response);
    }

    /// <summary>
    /// DELETE:
    /// Remove a Photo
    /// </summary>
    /// <param name="id">ID ==> Photo</param>
    /// <returns></returns>
    [HttpDelete("{id}/Photo")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
    {
        var response = new ApiResponse<int>();
        try
        {
            response.Result = await _photoAudioVideoRepository.DeleteByAsync(d=>d.Id==id);
            response.Message = "Removed was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(200, response);
    }
    
}
