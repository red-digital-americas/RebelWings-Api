using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Spotlight;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Spotlight;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controlador de Focos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SpotlightController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly ISpotlightRepository _spotlightRepository;
    private readonly IPhotoSpotlightRepository _photoSpotlightRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="spotlightRepository"></param>
    public SpotlightController(IMapper mapper, ILoggerManager logger, ISpotlightRepository spotlightRepository, IPhotoSpotlightRepository photoSpotlightRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _spotlightRepository = spotlightRepository;
        _photoSpotlightRepository = photoSpotlightRepository;
    }
    /// <summary>
    /// GET:
    /// Return a list of Spotlights
    /// </summary>
    /// <param name="id">ID ==> Sucursal ID</param>
    /// <param name="user">ID ==> Sucursal ID</param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<SpotlightDto>>> Get(int id, int user)
    {
        var response = new ApiResponse<SpotlightDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _spotlightRepository.GetAllIncluding(i => i.PhotoSpotlights);
            var @default = await order.FirstOrDefaultAsync(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<SpotlightDto>(@default);
            response.Message = "Consult was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(200, response);
    }
    /// <summary>
    /// GET:
    /// Return By Id
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<SpotlightDto>>> GetById(int id)
    {
      ApiResponse<SpotlightDto> response = new ApiResponse<SpotlightDto>();
        try
        {
            response.Result = _mapper.Map<SpotlightDto>(_spotlightRepository.GetAllIncluding(g=>g.PhotoSpotlights).FirstOrDefault(f => f.Id == id));
            response.Message = "Operation was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.Message;
            return StatusCode(500, response);
        }
        return StatusCode(200, response);
    }
    
    /// <summary>
    /// POST:
    /// Un nuevo Foco
    /// </summary>
    /// <param name="spotlightDto">Foco a agregar</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<SpotlightDto>>> Post([FromBody] SpotlightDto spotlightDto)
    {
        var response = new ApiResponse<SpotlightDto>();
        try
        {
            foreach (var item in spotlightDto.PhotoSpotlights)
            {
                if (_photoSpotlightRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoSpotlightRepository.UploadImageBase64(item.Photo, "Files/Spotlight/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var bathroom = await _spotlightRepository.AddAsyn(_mapper.Map<Spotlight>(spotlightDto));
            response.Result = _mapper.Map<SpotlightDto>(bathroom);
            response.Message = "Add was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(201, response);
    }
    /// <summary>
    /// PUT:
    /// Update or Add new Spotlight
    /// </summary>
    /// <param name="spotlightDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<SpotlightDto>>> Put([FromBody] SpotlightDto spotlightDto)
    {
        var response = new ApiResponse<SpotlightDto>();
        try
        {
            foreach (var item in spotlightDto.PhotoSpotlights)
            {
                if (_photoSpotlightRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.SpotlightId = spotlightDto.Id;
                    item.Photo = _photoSpotlightRepository.UploadImageBase64(item.Photo, "Files/Spotlight/", item.PhotoPath);
                    await _photoSpotlightRepository.AddAsyn(_mapper.Map<PhotoSpotlight>(item));
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
            var update = await _spotlightRepository.UpdateAsync(_mapper.Map<Spotlight>(spotlightDto),
                spotlightDto.Id);
            response.Result = _mapper.Map<SpotlightDto>(update);
            response.Message = "Updated was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
            response.Result = await _photoSpotlightRepository.DeleteByAsync(d=>d.Id==id);
            response.Message = "Removed was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(200, response);
    }
}
