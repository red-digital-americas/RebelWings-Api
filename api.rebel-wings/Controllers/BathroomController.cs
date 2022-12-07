using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Bathroom;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Bathroom;
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
public class BathroomController : ControllerBase
{
    private readonly IBathroomRepository _bathroomRepository;
    private readonly IPhotoBathroomRepository _photoBathroomRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="bathroomRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="photoBathroomRepository"></param>
    public BathroomController(
        IBathroomRepository bathroomRepository,
        IMapper mapper,
        ILoggerManager loggerManager,
        IPhotoBathroomRepository photoBathroomRepository)
    {
        _bathroomRepository = bathroomRepository;
        _mapper = mapper;
        _loggerManager = loggerManager;
        _photoBathroomRepository = photoBathroomRepository;
    }
    /// <summary>
    /// GET:
    /// Obtener por ID de Salon 
    /// </summary>
    /// <param name="id">ID </param>
    /// <returns></returns>
    [HttpGet("By-Id/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<BathroomDto>>> GetById(int id)
    {
        ApiResponse<BathroomDto> response = new ApiResponse<BathroomDto>();
        try
        {
            response.Result = _mapper.Map<BathroomDto>(_bathroomRepository.GetAllIncluding(g=>g.PhotoBathrooms).FirstOrDefault(f => f.Id == id));
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
    /// Return a Baños
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<BathroomDto>>> Get(int id, int user)
    {
        var response = new ApiResponse<BathroomDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _bathroomRepository.GetAllIncluding(i => i.PhotoBathrooms);
            var @default = await order.FirstOrDefaultAsync(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<BathroomDto>(@default);
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
    /// Add new Baños
    /// </summary>
    /// <param name="bathroomDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<BathroomDto>>> Post([FromBody] BathroomDto bathroomDto)
    {
        var response = new ApiResponse<BathroomDto>();
        try
        {
            foreach (var item in bathroomDto.PhotoBathrooms)
            {
                if (_photoBathroomRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoBathroomRepository.UploadImageBase64(item.Photo, "Files/Bathroom/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var bathroom = await _bathroomRepository.AddAsyn(_mapper.Map<Bathroom>(bathroomDto));
            response.Result = _mapper.Map<BathroomDto>(bathroom);
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
    /// Update a Baños
    /// </summary>
    /// <param name="bathroomDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<BathroomDto>>> Put([FromBody] BathroomDto bathroomDto)
    {
        var response = new ApiResponse<BathroomDto>();
        try
        {
            foreach (var item in bathroomDto.PhotoBathrooms)
            {
                if (_photoBathroomRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.BathroomId = bathroomDto.Id;
                    item.Photo = _photoBathroomRepository.UploadImageBase64(item.Photo, "Files/Bathroom/", item.PhotoPath);
                    await _photoBathroomRepository.AddAsyn(_mapper.Map<PhotoBathroom>(item));
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
            var update = await _bathroomRepository.UpdateAsync(_mapper.Map<Bathroom>(bathroomDto),
                bathroomDto.Id);
            response.Result = _mapper.Map<BathroomDto>(update);
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
            response.Result = await _photoBathroomRepository.DeleteByAsync(d=>d.Id==id);
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
