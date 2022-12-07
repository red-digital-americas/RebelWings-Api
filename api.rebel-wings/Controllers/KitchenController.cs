using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Chicken;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Kitchen;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.rebel_wings.Controllers;
/// <summary>
/// COntroller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class KitchenController : ControllerBase
{
    private readonly IKitchenRepository _kitchenRepository;
    private readonly IPhotoKitchenRepository _photoKitchenRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="kitchenRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="photoKitchenRepository"></param>
    public KitchenController(
        IKitchenRepository kitchenRepository,
        IMapper mapper,
        ILoggerManager loggerManager,
        IPhotoKitchenRepository photoKitchenRepository)
    {
        _kitchenRepository = kitchenRepository;
        _mapper = mapper;
        _loggerManager = loggerManager;
        _photoKitchenRepository = photoKitchenRepository;
    }
    /// <summary>
    /// GET:
    /// Obtener por ID de albaran 
    /// </summary>
    /// <param name="id">ID de albaran a consultar</param>
    /// <returns></returns>
    [HttpGet("By-Id/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<KitchenDto>>> GetById(int id)
    {
        ApiResponse<KitchenDto> response = new ApiResponse<KitchenDto>();
        try
        {
            response.Result = _mapper.Map<KitchenDto>(_kitchenRepository.GetAllIncluding(g=>g.PhotoKitchens).FirstOrDefault(f => f.Id == id));
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
    
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<KitchenDto>>> Get(int id, int user)
    {
        var response = new ApiResponse<KitchenDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var collection = _kitchenRepository.GetAllIncluding(i=>i.PhotoKitchens);
            var @default = await collection.FirstOrDefaultAsync(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<KitchenDto>(@default);
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
    
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<KitchenDto>>> Post([FromBody] KitchenDto kitchenDto)
    {
        var response = new ApiResponse<KitchenDto>();
        try
        {
            foreach (var item in kitchenDto.PhotoKitchens)
            {
                if (_photoKitchenRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoKitchenRepository.UploadImageBase64(item.Photo, "Files/Kitchen/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var addAsyn = await _kitchenRepository.AddAsyn(_mapper.Map<Kitchen>(kitchenDto));
            response.Result = _mapper.Map<KitchenDto>(addAsyn);
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
    
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<KitchenDto>>> Put([FromBody] KitchenDto kitchenDto)
    {
        var response = new ApiResponse<KitchenDto>();
        try
        {
            foreach (var item in kitchenDto.PhotoKitchens)
            {
                if (_photoKitchenRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.KitchenId = kitchenDto.Id;
                    item.Photo = _photoKitchenRepository.UploadImageBase64(item.Photo, "Files/Kitchen/", item.PhotoPath);
                    await _photoKitchenRepository.AddAsyn(_mapper.Map<PhotoKitchen>(item));
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
            var update = await _kitchenRepository.UpdateAsync(_mapper.Map<Kitchen>(kitchenDto),
                kitchenDto.Id);
            response.Result = _mapper.Map<KitchenDto>(update);
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
            response.Result = await _photoKitchenRepository.DeleteByAsync(d=>d.Id==id);
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
