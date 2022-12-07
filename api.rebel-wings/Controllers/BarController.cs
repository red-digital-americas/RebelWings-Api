using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Bar;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Bar;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controller
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class BarController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IBarRepository _barRepository;
    private readonly IPhotoBarRepository _photoBarRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="barRepository"></param>
    /// <param name="photoBarRepository"></param>
    public BarController(IMapper mapper, ILoggerManager loggerManager,
        IBarRepository barRepository,
        IPhotoBarRepository photoBarRepository)
    {
        _mapper = mapper;
        _loggerManager = loggerManager;
        _barRepository = barRepository;
        _photoBarRepository = photoBarRepository;
    }
    
    /// <summary>
    /// GET:
    /// Obtener por ID de Barra 
    /// </summary>
    /// <param name="id">ID </param>
    /// <returns></returns>
    [HttpGet("By-Id/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<BarDto>>> GetById(int id)
    {
        ApiResponse<BarDto> response = new ApiResponse<BarDto>();
        try
        {
            response.Result = _mapper.Map<BarDto>(_barRepository.GetAllIncluding(g=>g.PhotoBars).FirstOrDefault(f => f.Id == id));
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
    /// Return a Barra
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<BarDto>>> Get(int id, int user)
    {
        var response = new ApiResponse<BarDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _barRepository.GetAllIncluding(i=>i.PhotoBars);
            response.Result = _mapper.Map<BarDto>(await order.FirstOrDefaultAsync(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user));
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
    /// Add new Barra
    /// </summary>
    /// <param name="barDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<BarDto>>> Post([FromBody] BarDto barDto)
    {
        var response = new ApiResponse<BarDto>();
        try
        {   
            foreach (var item in barDto.PhotoBars)
            {
                if (_photoBarRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoBarRepository.UploadImageBase64(item.Photo, "Files/Bar/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var bar = await _barRepository.AddAsyn(_mapper.Map<Bar>(barDto));
            response.Result = _mapper.Map<BarDto>(bar);
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
    /// Update a Barra
    /// </summary>
    /// <param name="barDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<BarDto>>> Put([FromBody] BarDto barDto)
    {
        var response = new ApiResponse<BarDto>();
        try
        {
            
            foreach (var item in barDto.PhotoBars)
            {
                if (_photoBarRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.BarId = barDto.Id;
                    item.Photo = _photoBarRepository.UploadImageBase64(item.Photo, "Files/Bar/", item.PhotoPath);
                    await _photoBarRepository.AddAsyn(_mapper.Map<PhotoBar>(item));
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
            
            
            var @async = await _barRepository.UpdateAsync(_mapper.Map<Bar>(barDto), barDto.Id);

            response.Result = _mapper.Map<BarDto>(@async);
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
            response.Result = await _photoBarRepository.DeleteByAsync(d=>d.Id==id);
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
