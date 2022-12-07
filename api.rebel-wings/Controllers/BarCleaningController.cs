using api.rebel_wings.ActionFilter;
using api.rebel_wings.Extensions;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.BarCleaning;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.BarCleaning;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controller Bar Cleaning
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class BarCleaningController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IBarCleaningRepository _barCleaningRepository;
    private readonly IPhotoBarCleaningRepository _photoBarCleaningRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="barCleaningRepository"></param>
    /// <param name="photoBarCleaningRepository"></param>
    public BarCleaningController(IMapper mapper, ILoggerManager loggerManager, IBarCleaningRepository barCleaningRepository, IPhotoBarCleaningRepository photoBarCleaningRepository)
    {
        _mapper = mapper;
        _loggerManager = loggerManager;
        _barCleaningRepository = barCleaningRepository;
        _photoBarCleaningRepository = photoBarCleaningRepository;
    }
    /// <summary>
    /// GET:
    /// Return a Bar Cleaning By Branch
    /// </summary>
    /// <param name="id">ID => Branch </param>
    /// <param name="user">ID => Branch </param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<BarCleaningDto>> Get(int id, int user)
    {
        var response = new ApiResponse<BarCleaningDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _barCleaningRepository.GetAllIncluding(i => i.PhotoBarCleanings)
                .FirstOrDefault(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<BarCleaningDto>(order);
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
    
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<BarCleaningDto>> GetById(int id)
    {
        var response = new ApiResponse<BarCleaningDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd();
            var order = _barCleaningRepository.GetAllIncluding(i => i.PhotoBarCleanings)
                .FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<BarCleaningDto>(order);
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
    /// Add new Bar Cleaning
    /// </summary>
    /// <param name="barCleaningDto">Object</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<BarCleaningDto>>> Post([FromBody] BarCleaningDto barCleaningDto)
    {
        var response = new ApiResponse<BarCleaningDto>();
        try
        {
            foreach (var item in barCleaningDto.PhotoBarCleanings)
            {
                if (_photoBarCleaningRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoBarCleaningRepository.UploadImageBase64(item.Photo, "Files/BarCleaning/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }

            var barCleaning = await _barCleaningRepository.AddAsyn(_mapper.Map<BarCleaning>(barCleaningDto));
            response.Result = _mapper.Map<BarCleaningDto>(barCleaning);
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
    /// Update a Bar Cleaning
    /// </summary>
    /// <param name="barCleaningDto">Object</param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<BarCleaningDto>>> Put([FromBody] BarCleaningDto barCleaningDto)
    {
        var response = new ApiResponse<BarCleaningDto>();
        try
        {
            foreach (var item in barCleaningDto.PhotoBarCleanings)
            {
                if (_photoBarCleaningRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.BarCleaningId = barCleaningDto.Id;
                    item.Photo = _photoBarCleaningRepository.UploadImageBase64(item.Photo, "Files/BarCleaning/", item.PhotoPath);
                    await _photoBarCleaningRepository.AddAsyn(_mapper.Map<PhotoBarCleaning>(item));
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
                
            var updateAsync = await _barCleaningRepository.UpdateAsync(_mapper.Map<BarCleaning>(barCleaningDto), barCleaningDto.Id);

            response.Result = _mapper.Map<BarCleaningDto>(updateAsync);
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
            response.Result = await _photoBarCleaningRepository.DeleteByAsync(d=>d.Id==id);
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
