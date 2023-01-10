using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.BathRoomsOverallStatus;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.BathRoomsOverallStatus;
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
public class BathRoomsOverallStatusController : ControllerBase
{
    private readonly IBathRoomsOverallStatusRepository _bathRoomsOverallStatusRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IPhotoBathRoomsOverallStatusRepository _photoBathRoomsOverallStatusRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="bathRoomsOverallStatusRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    public BathRoomsOverallStatusController(
        IBathRoomsOverallStatusRepository bathRoomsOverallStatusRepository,
        IMapper mapper,
        ILoggerManager loggerManager,
        IPhotoBathRoomsOverallStatusRepository photoBathRoomsOverallStatusRepository)
    {
        _bathRoomsOverallStatusRepository = bathRoomsOverallStatusRepository;
        _mapper = mapper;
        _loggerManager = loggerManager;
        _photoBathRoomsOverallStatusRepository  = photoBathRoomsOverallStatusRepository;
    }
    
    /// <summary>
    /// GET:
    /// </summary>
    /// <param name="id">ID de Sucursal</param>
    /// <param name="user">ID de Sucursal</param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<BathRoomsOverallStatusDto>>> Get(int id, int user)
    {
        var response = new ApiResponse<BathRoomsOverallStatusDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _bathRoomsOverallStatusRepository.GetAllIncluding(i => i.PhotoBathRoomsOverallStatuss);
            var @default = await order.FirstOrDefaultAsync(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<BathRoomsOverallStatusDto>(@default);
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

    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<BathRoomsOverallStatusDto>>> GetById(int id)
    {
        var response = new ApiResponse<BathRoomsOverallStatusDto>();
        try
        {
            response.Result = _mapper.Map<BathRoomsOverallStatusDto>(_bathRoomsOverallStatusRepository.GetAllIncluding(g=>g.PhotoBathRoomsOverallStatuss).FirstOrDefault(f => f.Id == id));
            response.Message = "Operation was success";
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
    /// </summary>
    /// <param name="bathRoomsOverallStatusDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<BathRoomsOverallStatusDto>>> Post([FromBody] BathRoomsOverallStatusDto bathRoomsOverallStatusDto)
    {
        var response = new ApiResponse<BathRoomsOverallStatusDto>();
        try
        {
            foreach (var item in bathRoomsOverallStatusDto.PhotoBathRoomsOverallStatuss)
            {
                if (_photoBathRoomsOverallStatusRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoBathRoomsOverallStatusRepository.UploadImageBase64(item.Photo, "Files/General/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var banio = await _bathRoomsOverallStatusRepository.AddAsyn(_mapper.Map<BathRoomsOverallStatus>(bathRoomsOverallStatusDto));
            response.Result = _mapper.Map<BathRoomsOverallStatusDto>(banio);
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
    /// Update a bathRooms Overall Status
    /// </summary>
    /// <param name="bathRoomsOverallStatusDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<BathRoomsOverallStatusDto>>> Put([FromBody] BathRoomsOverallStatusDto bathRoomsOverallStatusDto)
    {
        var response = new ApiResponse<BathRoomsOverallStatusDto>();
        try
        {
            foreach (var item in bathRoomsOverallStatusDto.PhotoBathRoomsOverallStatuss)
            {
                if (_photoBathRoomsOverallStatusRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.BathRoomsOverallStatusId = bathRoomsOverallStatusDto.Id;
                    item.Photo = _photoBathRoomsOverallStatusRepository.UploadImageBase64(item.Photo, "Files/General/", item.PhotoPath);
                    await _photoBathRoomsOverallStatusRepository.AddAsyn(_mapper.Map<PhotoBathRoomsOverallStatus>(item));
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
            var update = await _bathRoomsOverallStatusRepository.UpdateAsync(_mapper.Map<BathRoomsOverallStatus>(bathRoomsOverallStatusDto),
                bathRoomsOverallStatusDto.Id);
            response.Result = _mapper.Map<BathRoomsOverallStatusDto>(update);
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
            response.Result = await _photoBathRoomsOverallStatusRepository.DeleteByAsync(d=>d.Id==id);
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
