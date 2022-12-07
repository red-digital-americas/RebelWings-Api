using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Salon;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Salon;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SalonController : ControllerBase
{
    private readonly ISalonRepository _salonRepository;
    private readonly IPhotoSalonRepository _photoSalonRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="salonRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="photoSalonRepository"></param>
    public SalonController(
        ISalonRepository salonRepository,
        IMapper mapper,
        ILoggerManager loggerManager,
        IPhotoSalonRepository photoSalonRepository)
    {
        _salonRepository = salonRepository;
        _mapper = mapper;
        _loggerManager = loggerManager;
        _photoSalonRepository = photoSalonRepository;
    }
    /// <summary>
    /// GET:
    /// Obtener por ID de Salon 
    /// </summary>
    /// <param name="id">ID </param>
    /// <returns></returns>
    [HttpGet("By-Id/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<SalonDto>>> GetById(int id)
    {
        ApiResponse<SalonDto> response = new ApiResponse<SalonDto>();
        try
        {
            response.Result = _mapper.Map<SalonDto>(_salonRepository.GetAllIncluding(g=>g.PhotoSalons).FirstOrDefault(f => f.Id == id));
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
    /// Return a Salon
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<SalonDto>>> Get(int id)
    {
        var response = new ApiResponse<SalonDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd();
            var collection = _salonRepository.GetAllIncluding(i=>i.PhotoSalons);
            var @default = await collection.FirstOrDefaultAsync(f => f.BranchId == id && f.CreatedDate > today);
            response.Result = _mapper.Map<SalonDto>(@default);
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
    /// Add new Salon
    /// </summary>
    /// <param name="salonDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<SalonDto>>> Post([FromBody] SalonDto salonDto)
    {
        var response = new ApiResponse<SalonDto>();
        try
        {
            foreach (var item in salonDto.PhotoSalons)
            {
                if (_photoSalonRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoSalonRepository.UploadImageBase64(item.Photo, "Files/Salon/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var salon = await _salonRepository.AddAsyn(_mapper.Map<Salon>(salonDto));
            response.Result = _mapper.Map<SalonDto>(salon);
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
    /// Update a Salon
    /// </summary>
    /// <param name="salonDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<SalonDto>>> Put([FromBody] SalonDto salonDto)
    {
        var response = new ApiResponse<SalonDto>();
        try
        {
            foreach (var item in salonDto.PhotoSalons)
            {
                if (_photoSalonRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.SalonId = salonDto.Id;
                    item.Photo = _photoSalonRepository.UploadImageBase64(item.Photo, "Files/Salon/", item.PhotoPath);
                    await _photoSalonRepository.AddAsyn(_mapper.Map<PhotoSalon>(item));
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
            var update = await _salonRepository.UpdateAsync(_mapper.Map<Salon>(salonDto),
                salonDto.Id);
            response.Result = _mapper.Map<SalonDto>(update);
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
            response.Result = await _photoSalonRepository.DeleteByAsync(d=>d.Id==id);
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