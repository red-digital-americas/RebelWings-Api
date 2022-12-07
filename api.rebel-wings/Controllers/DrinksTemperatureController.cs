using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.DrinksTemperature;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.DrinksTemperature;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controlador de temperatura de bebidas
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class DrinksTemperatureController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IDrinksTemperatureRepository _drinksTemperatureRepository;
    private readonly IPhotoDrinksTemperatureRepository _photoDrinksTemperatureRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="drinksTemperatureRepository"></param>
    /// <param name="photoDrinksTemperatureRepository"></param>
    public DrinksTemperatureController(IMapper mapper, ILoggerManager loggerManager,
        IDrinksTemperatureRepository drinksTemperatureRepository,
        IPhotoDrinksTemperatureRepository photoDrinksTemperatureRepository)
    {
        _mapper = mapper;
        _loggerManager = loggerManager;
        _drinksTemperatureRepository = drinksTemperatureRepository;
        _photoDrinksTemperatureRepository = photoDrinksTemperatureRepository;
    }
    /// <summary>
    /// GET:
    /// Regresa lista de Temperatura de bebidas
    /// </summary>
    /// <param name="id">ID de sucursal</param>
    /// <param name="user">ID de sucursal</param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<DrinksTemperatureDto>>> Get(int id, int user)
    {
        var response = new ApiResponse<List<DrinksTemperatureDto>>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _drinksTemperatureRepository.GetAllIncluding(i => i.PhotoDrinksTemperatures)
                .Where(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user).ToList();
            response.Result = _mapper.Map<List<DrinksTemperatureDto>>(order);
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
    /// GET:
    /// By ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<DrinksTemperatureDto>> GetById(int id)
    {
        var response = new ApiResponse<DrinksTemperatureDto>();
        try
        {
            var order = _drinksTemperatureRepository.GetAllIncluding(i => i.PhotoDrinksTemperatures)
                .FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<DrinksTemperatureDto>(order);
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
    /// Agregar una lista de bebidas de temperartura 
    /// </summary>
    /// <param name="drinksTemperatureDtos"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<DrinksTemperatureDto>>>> Post([FromBody] List<DrinksTemperatureDto> drinksTemperatureDtos)
    {
        var response = new ApiResponse<List<DrinksTemperatureDto>>();
        try
        {
            var temperatureDtos = new List<DrinksTemperatureDto>();
            foreach (var drinksTemperatureDto in drinksTemperatureDtos)
            {
                if (drinksTemperatureDto.Id != 0)
                {
                    foreach (var item in drinksTemperatureDto.PhotoDrinksTemperatures)
                    {
                        if (_photoDrinksTemperatureRepository.IsBase64(item.Photo))
                        {
                            item.DrinkTemperatureId = drinksTemperatureDto.Id;
                            item.Photo = _photoDrinksTemperatureRepository.UploadImageBase64(item.Photo, "Files/DrinksTemperature/", item.PhotoPath);
                            await _photoDrinksTemperatureRepository.AddAsyn(_mapper.Map<PhotoDrinksTemperature>(item));
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Photo is not Base64";
                            return StatusCode(415, response);
                        }
                    }
                    var drinksTemperature = await _drinksTemperatureRepository.UpdateAsync(_mapper.Map<DrinksTemperature>(drinksTemperatureDto), drinksTemperatureDto.Id);
                    temperatureDtos.Add(_mapper.Map<DrinksTemperatureDto>(drinksTemperature));
                }
                else
                {
                    foreach (var item in drinksTemperatureDto.PhotoDrinksTemperatures)
                    {
                        if (_photoDrinksTemperatureRepository.IsBase64(item.Photo))
                        {
                            item.Photo = _photoDrinksTemperatureRepository.UploadImageBase64(item.Photo, "Files/DrinksTemperature/", item.PhotoPath);
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Photo is not Base64";
                            return StatusCode(415, response);
                        }
                    }
                    var drinksTemperature = await _drinksTemperatureRepository.AddAsyn(_mapper.Map<DrinksTemperature>(drinksTemperatureDto));
                    temperatureDtos.Add(_mapper.Map<DrinksTemperatureDto>(drinksTemperature));
                }
                
            }
           
            response.Result = temperatureDtos;
            response.Message = "added was success";
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
    /// Actualiza una lista de  Temperatura de bebidas
    /// </summary>
    /// <param name="drinksTemperatureDtos"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<DrinksTemperatureDto>>>> Put([FromBody] List<DrinksTemperatureDto> drinksTemperatureDtos)
    {
        var response = new ApiResponse<List<DrinksTemperatureDto>>();
        try
        {
            var dtos = new List<DrinksTemperatureDto>();
            foreach (var drinksTemperatureDto in drinksTemperatureDtos)
            {
                if (drinksTemperatureDto.Id != 0)
                {
                    foreach (var item in drinksTemperatureDto.PhotoDrinksTemperatures)
                    {
                        if (_drinksTemperatureRepository.IsBase64(item.Photo) && item.Id == 0)
                        {
                            item.DrinkTemperatureId = drinksTemperatureDto.Id;
                            item.Photo = _drinksTemperatureRepository.UploadImageBase64(item.Photo, "Files/DrinksTemperature/", item.PhotoPath);
                            await _photoDrinksTemperatureRepository.AddAsyn(_mapper.Map<PhotoDrinksTemperature>(item));
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
                
                    var updateAsync = await _drinksTemperatureRepository.UpdateAsync(_mapper.Map<DrinksTemperature>(drinksTemperatureDto), drinksTemperatureDto.Id);
                    dtos.Add(_mapper.Map<DrinksTemperatureDto>(updateAsync));
                }
                else
                {
                    foreach (var item in drinksTemperatureDto.PhotoDrinksTemperatures)
                    {
                        if (_drinksTemperatureRepository.IsBase64(item.Photo))
                        {
                            item.Photo = _drinksTemperatureRepository.UploadImageBase64(item.Photo, "Files/DrinksTemperature/", item.PhotoPath);
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Photo is not Base64";
                            return StatusCode(415, response);
                        }
                    }
                    var addAsyn = await _drinksTemperatureRepository.AddAsyn(_mapper.Map<DrinksTemperature>(drinksTemperatureDto));
                    dtos.Add(_mapper.Map<DrinksTemperatureDto>(addAsyn));
                }
            }
            
            
            response.Result = dtos;
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
            response.Result = await _photoDrinksTemperatureRepository.DeleteByAsync(d=>d.Id==id);
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
