using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Station;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Station;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controlador de Estación
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StationController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IStationRepository _stationRepository;
    private readonly IPhotoStationRepository _photoStationRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="stationRepository"></param>
    /// <param name="photoStationRepositor"></param>
    public StationController(
        IMapper mapper,
        ILoggerManager loggerManager,
        IStationRepository stationRepository,
        IPhotoStationRepository photoStationRepositor)
    {
        _mapper = mapper;
        _loggerManager = loggerManager;
        _stationRepository = stationRepository;
        _photoStationRepository = photoStationRepositor;
    }
    /// <summary>
    /// GET:
    /// Obtener por ID de Station
    /// </summary>
    /// <param name="id">ID </param>
    /// <returns></returns>
    [HttpGet("By-Id/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<StationDto>>> GetById(int id)
    {
        ApiResponse<StationDto> response = new ApiResponse<StationDto>();
        try
        {
            response.Result = _mapper.Map<StationDto>(_stationRepository
                .GetAllIncluding(g => g.PhotoStations).FirstOrDefault(f => f.Id == id));
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
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<StationDto>> Get(int id, int user)
    {
        var response = new ApiResponse<StationDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _stationRepository.GetAllIncluding(i => i.PhotoStations)
                .FirstOrDefault(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<StationDto>(order);
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
        return StatusCode(201, response);
    }
    /// <summary>
    /// POST:
    /// </summary>
    /// <param name="stationDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<StationDto>>> Post([FromBody] StationDto stationDto)
    {
        var response = new ApiResponse<StationDto>();
        try
        {   
            foreach (var item in stationDto.PhotoStations)
            {
                if (_stationRepository.IsBase64(item.Photo))
                {
                    item.Photo = _stationRepository.UploadImageBase64(item.Photo, "Files/Station/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            
            var order = await _stationRepository.AddAsyn(_mapper.Map<Station>(stationDto));
            response.Result = _mapper.Map<StationDto>(order);
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
        return StatusCode(201, response);
    }
    /// <summary>
    /// PUT:
    /// </summary>
    /// <param name="stationDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<StationDto>>> Put([FromBody] StationDto stationDto)
    {
        var response = new ApiResponse<StationDto>();
        try
        {
            
            foreach (var item in stationDto.PhotoStations)
            {
                if (_stationRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.StationId = stationDto.Id;
                    item.Photo = _stationRepository.UploadImageBase64(item.Photo, "Files/Station/", item.PhotoPath);
                    await _photoStationRepository.AddAsyn(_mapper.Map<PhotoStation>(item));
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
            var generalCleaning = await _stationRepository.UpdateAsync(_mapper.Map<Station>(stationDto), stationDto.Id);

            response.Result = _mapper.Map<StationDto>(generalCleaning);
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
        return StatusCode(201, response);
    }
    
    /// <summary>
    /// DELETE:
    /// Remove Photo
    /// </summary>
    /// <param name="id"></param>
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
            response.Result = await _photoStationRepository.DeleteByAsync(d=>d.Id==id);
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
