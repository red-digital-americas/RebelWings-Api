using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.WashBasinWithSoapPaper;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.BathRoomsOverallStatus;
using biz.rebel_wings.Repository.WashBasinWithSoapPaper;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controlador
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WashBasinWithSoapPaperController : ControllerBase
{
    private readonly IWashBasinWithSoapPaperRepository _washBasinWithSoapPaperRepository;
    private readonly IPhotoWashBasinWithSoapPaperRepository _photoWashBasinWithSoapPaperRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="washBasinWithSoapPaperRepository"></param>
    /// <param name="photoWashBasinWithSoapPaperRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    public WashBasinWithSoapPaperController(
        IWashBasinWithSoapPaperRepository washBasinWithSoapPaperRepository,
        IPhotoWashBasinWithSoapPaperRepository photoWashBasinWithSoapPaperRepository,
        IMapper mapper,
        ILoggerManager loggerManager)
    {
        _mapper = mapper;
        _loggerManager = loggerManager;
        _washBasinWithSoapPaperRepository = washBasinWithSoapPaperRepository;
        _photoWashBasinWithSoapPaperRepository = photoWashBasinWithSoapPaperRepository;
    }


    /// <summary>
    /// GET:
    /// Obtener por ID de Lavabos con jabón y papel 
    /// </summary>
    /// <param name="id">ID </param>
    /// <returns></returns>
    [HttpGet("By-Id/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<WashBasinWithSoapPaperDto>>> GetById(int id)
    {
        ApiResponse<WashBasinWithSoapPaperDto> response = new ApiResponse<WashBasinWithSoapPaperDto>();
        try
        {
            response.Result = _mapper.Map<WashBasinWithSoapPaperDto>(_washBasinWithSoapPaperRepository
                .GetAllIncluding(g => g.PhotoWashBasinWithSoapPapers).FirstOrDefault(f => f.Id == id));
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
    /// Regrasa registro del día
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<WashBasinWithSoapPaperDto>> Get(int id, int user)
    {
        var response = new ApiResponse<WashBasinWithSoapPaperDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _washBasinWithSoapPaperRepository.GetAllIncluding(i => i.PhotoWashBasinWithSoapPapers);
            var @default = order.FirstOrDefault(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<WashBasinWithSoapPaperDto>(@default);
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
    /// Add new Lavabos con jabón y papel
    /// </summary>
    /// <param name="washBasinWithSoapPaperDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<WashBasinWithSoapPaperDto>>> Post([FromBody] WashBasinWithSoapPaperDto washBasinWithSoapPaperDto)
    {
        var response = new ApiResponse<WashBasinWithSoapPaperDto>();
        try
        {
            foreach (var item in washBasinWithSoapPaperDto.PhotoWashBasinWithSoapPapers)
            {
                if (_photoWashBasinWithSoapPaperRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoWashBasinWithSoapPaperRepository.UploadImageBase64(item.Photo, "Files/WashBasinWithSoapPaper/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var order = await _washBasinWithSoapPaperRepository.AddAsyn(_mapper.Map<WashBasinWithSoapPaper>(washBasinWithSoapPaperDto));
            response.Result = _mapper.Map<WashBasinWithSoapPaperDto>(order);
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
    /// UPDATE:
    /// Update Lavabos con jabón y papel
    /// </summary>
    /// <param name="washBasinWithSoapPaperDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<WashBasinWithSoapPaperDto>>> Put([FromBody] WashBasinWithSoapPaperDto washBasinWithSoapPaperDto)
    {
        var response = new ApiResponse<WashBasinWithSoapPaperDto>();
        try
        {
            foreach (var item in washBasinWithSoapPaperDto.PhotoWashBasinWithSoapPapers)
            {
                if (_photoWashBasinWithSoapPaperRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.Photo = _photoWashBasinWithSoapPaperRepository.UploadImageBase64(item.Photo, "Files/WashBasinWithSoapPaper/", item.PhotoPath);
                    await _photoWashBasinWithSoapPaperRepository.AddAsyn(_mapper.Map<PhotoWashBasinWithSoapPaper>(item));
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

            var @async = await _washBasinWithSoapPaperRepository.UpdateAsync(
                _mapper.Map<WashBasinWithSoapPaper>(washBasinWithSoapPaperDto), washBasinWithSoapPaperDto.Id);
            response.Result = _mapper.Map<WashBasinWithSoapPaperDto>(@async);
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
    /// Remove a Photo Lavabos con jabón y papel
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    [HttpDelete("{id}/Photo")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
    {
        var response = new ApiResponse<int>();
        try
        {
            response.Result = await _photoWashBasinWithSoapPaperRepository.DeleteByAsync(d => d.Id == id);
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
        return StatusCode(201, response);
    }
    
}
