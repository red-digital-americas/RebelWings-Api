using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.EntriesChargedAsDeliveryNote;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.EntriesChargedAsDeliveryNote;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controller de Entradas cargadas como albarán 
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EntriesChargedAsDeliveryNoteController : ControllerBase
{
    private readonly IEntriesChargedAsDeliveryNoteRepository _entriesChargedAsDeliveryNoteRepository;
    private readonly IPhotoEntriesChargedAsDeliveryNoteRepository _photoEntriesChargedAsDeliveryNoteRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="entriesChargedAsDeliveryNoteRepository"></param>
    /// <param name="photoEntriesChargedAsDeliveryNoteRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    public EntriesChargedAsDeliveryNoteController(
        IEntriesChargedAsDeliveryNoteRepository entriesChargedAsDeliveryNoteRepository,
        IPhotoEntriesChargedAsDeliveryNoteRepository photoEntriesChargedAsDeliveryNoteRepository,
        IMapper mapper,
        ILoggerManager loggerManager
    )
    {
        _entriesChargedAsDeliveryNoteRepository = entriesChargedAsDeliveryNoteRepository;
        _photoEntriesChargedAsDeliveryNoteRepository = photoEntriesChargedAsDeliveryNoteRepository;
        _mapper = mapper;
        _loggerManager = loggerManager;
    }
    /// <summary>
    /// GET:
    /// Regresa Entradas cargadas como albarán del día
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<EntriesChargedAsDeliveryNoteDto>> Get(int id)
    {
        var response = new ApiResponse<EntriesChargedAsDeliveryNoteDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd();
            var queryable =
                _entriesChargedAsDeliveryNoteRepository.GetAllIncluding(i => i.PhotoEntriesChargedAsDeliveryNotes);
            var @default = queryable.FirstOrDefault(f => f.BranchId == id && f.CreatedDate > today);
            response.Result = _mapper.Map<EntriesChargedAsDeliveryNoteDto>(@default);
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
    /// GET:
    /// Obtener por ID de Entradas cargadas
    /// </summary>
    /// <param name="id">ID de Entradas cargadas</param>
    /// <returns></returns>
    [HttpGet("By-Id/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<EntriesChargedAsDeliveryNoteDto>>> GetById(int id)
    {
        var response = new ApiResponse<EntriesChargedAsDeliveryNoteDto>();
        try
        {
            var @default = _entriesChargedAsDeliveryNoteRepository.GetAllIncluding(f => f.PhotoEntriesChargedAsDeliveryNotes)
                .FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<EntriesChargedAsDeliveryNoteDto>(@default);
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
    /// POST:
    /// Add new Entradas cargadas como albarán del día
    /// </summary>
    /// <param name="entriesChargedAsDeliveryNoteDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<EntriesChargedAsDeliveryNoteDto>>> Post([FromBody] EntriesChargedAsDeliveryNoteDto entriesChargedAsDeliveryNoteDto)
    {
        var response = new ApiResponse<EntriesChargedAsDeliveryNoteDto>();
        try
        {
            foreach (var item in entriesChargedAsDeliveryNoteDto.PhotoEntriesChargedAsDeliveryNotes)
            {
                if (_photoEntriesChargedAsDeliveryNoteRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoEntriesChargedAsDeliveryNoteRepository.UploadImageBase64(item.Photo, "Files/EntriesChargedAsDeliveryNote/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var order = await _entriesChargedAsDeliveryNoteRepository.AddAsyn(_mapper.Map<EntriesChargedAsDeliveryNote>(entriesChargedAsDeliveryNoteDto));
            response.Result = _mapper.Map<EntriesChargedAsDeliveryNoteDto>(order);
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
    /// Update Entradas cargadas como albarán del día
    /// </summary>
    /// <param name="entriesChargedAsDeliveryNoteDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<EntriesChargedAsDeliveryNoteDto>>> Put([FromBody] EntriesChargedAsDeliveryNoteDto entriesChargedAsDeliveryNoteDto)
    {
        var response = new ApiResponse<EntriesChargedAsDeliveryNoteDto>();
        try
        {
            foreach (var item in entriesChargedAsDeliveryNoteDto.PhotoEntriesChargedAsDeliveryNotes)
            {
                if (_photoEntriesChargedAsDeliveryNoteRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.Photo = _photoEntriesChargedAsDeliveryNoteRepository.UploadImageBase64(item.Photo, "Files/EntriesChargedAsDeliveryNote/", item.PhotoPath);
                    await _photoEntriesChargedAsDeliveryNoteRepository.AddAsyn(_mapper.Map<PhotoEntriesChargedAsDeliveryNote>(item));
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

            var @async = await _entriesChargedAsDeliveryNoteRepository.UpdateAsync(
                _mapper.Map<EntriesChargedAsDeliveryNote>(entriesChargedAsDeliveryNoteDto), entriesChargedAsDeliveryNoteDto.Id);
            response.Result = _mapper.Map<EntriesChargedAsDeliveryNoteDto>(@async);
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
    /// Remove a PHOTO Entradas cargadas como albarán del día
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Photo")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
    {
        var response = new ApiResponse<int>();
        try
        {
            response.Result = await _photoEntriesChargedAsDeliveryNoteRepository.DeleteByAsync(d => d.Id == id);
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
        return StatusCode(202, response);
    }
    
}