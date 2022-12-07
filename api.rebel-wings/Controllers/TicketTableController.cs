using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.TicketTable;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.TicketTable;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TicketTableController : ControllerBase
{
    private readonly ITicketTableRepository _ticketTableRepository;
    private readonly IPhotoTicketTableRepository _photoTicketTableRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ticketTableRepository"></param>
    /// <param name="photoTicketTableRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    public TicketTableController(
        ITicketTableRepository ticketTableRepository,
        IPhotoTicketTableRepository photoTicketTableRepository,
        IMapper mapper,
        ILoggerManager loggerManager
        )
    {
        _ticketTableRepository = ticketTableRepository;
        _photoTicketTableRepository = photoTicketTableRepository;
        _mapper = mapper;
        _loggerManager = loggerManager;
    }
    /// <summary>
    /// GET:
    /// Regresa Ticket y Mesa del día
    /// </summary>
    /// <param name="id">Sucursal</param>
    /// <param name="user">Sucursal</param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<TicketTableDto>> Get(int id, int user)
    {
        var response = new ApiResponse<TicketTableDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var queryable =
                _ticketTableRepository.GetAllIncluding(i => i.PhotoTicketTables);
            var @default = queryable.FirstOrDefault(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<TicketTableDto>(@default);
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
    /// Return By Id
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<TicketTableDto>> GetById(int id)
    {
        var response = new ApiResponse<TicketTableDto>();
        try
        {
            var queryable =
                _ticketTableRepository.GetAllIncluding(i => i.PhotoTicketTables);
            var @default = queryable.FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<TicketTableDto>(@default);
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
    /// Add new Ticket y mesa
    /// </summary>
    /// <param name="ticketTableDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<TicketTableDto>>> Post([FromBody] TicketTableDto ticketTableDto)
    {
        var response = new ApiResponse<TicketTableDto>();
        try
        {
            foreach (var item in ticketTableDto.PhotoTicketTables)
            {
                if (_photoTicketTableRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoTicketTableRepository.UploadImageBase64(item.Photo, "Files/TicketTable/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var order = await _ticketTableRepository.AddAsyn(_mapper.Map<TicketTable>(ticketTableDto));
            response.Result = _mapper.Map<TicketTableDto>(order);
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
    /// Update record Ticket Table
    /// </summary>
    /// <param name="ticketTableDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<TicketTableDto>>> Put([FromBody] TicketTableDto ticketTableDto)
    {
        var response = new ApiResponse<TicketTableDto>();
        try
        {
            foreach (var item in ticketTableDto.PhotoTicketTables)
            {
                if (_photoTicketTableRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.Photo = _photoTicketTableRepository.UploadImageBase64(item.Photo, "Files/TicketTable/", item.PhotoPath);
                    await _photoTicketTableRepository.AddAsyn(_mapper.Map<PhotoTicketTable>(item));
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

            var @async = await _ticketTableRepository.UpdateAsync(
                _mapper.Map<TicketTable>(ticketTableDto), ticketTableDto.Id);
            response.Result = _mapper.Map<TicketTableDto>(@async);
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
    /// Remove a Photo Ticket Mesa
    /// </summary>
    /// <param name="id">PK</param>
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
            response.Result = await _photoTicketTableRepository.DeleteByAsync(d => d.Id == id);
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
