using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.GeneralCleaning;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.GeneralCleaning;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// General Cleaning Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GeneralCleaningController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IGeneralCleaningRepository _generalCleaningRepository;
    private readonly IPhotoGeneralCleaningRepository _photoGeneralCleaningRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="generalCleaningRepository"></param>
    /// <param name="photoGeneralCleaningRepository"></param>
    public GeneralCleaningController(
        IMapper mapper,
        ILoggerManager loggerManager,
        IGeneralCleaningRepository generalCleaningRepository,
        IPhotoGeneralCleaningRepository photoGeneralCleaningRepository)
    {
        _mapper = mapper;
        _loggerManager = loggerManager;
        _generalCleaningRepository = generalCleaningRepository;
        _photoGeneralCleaningRepository = photoGeneralCleaningRepository;
    }
    /// <summary>
    /// GET
    /// </summary>
    /// <param name="id">ID de Sucursal</param>
    /// <param name="user">ID de Sucursal</param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<GeneralCleaningDto>> Get(int id, int user)
    {
        var response = new ApiResponse<GeneralCleaningDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _generalCleaningRepository.GetAllIncluding(i => i.PhotoGeneralCleanings)
                .FirstOrDefault(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<GeneralCleaningDto>(order);
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
    /// Return By Id
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<GeneralCleaningDto>> GetById(int id)
    {
        var response = new ApiResponse<GeneralCleaningDto>();
        try
        {
            var order = _generalCleaningRepository.GetAllIncluding(i => i.PhotoGeneralCleanings)
                .FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<GeneralCleaningDto>(order);
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
    
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<GeneralCleaningDto>>> Post([FromBody] GeneralCleaningDto generalCleaningDto)
    {
        var response = new ApiResponse<GeneralCleaningDto>();
        try
        {   
            foreach (var item in generalCleaningDto.PhotoGeneralCleanings)
            {
                if (_generalCleaningRepository.IsBase64(item.Photo))
                {
                    item.Photo = _generalCleaningRepository.UploadImageBase64(item.Photo, "Files/GeneralCleaning/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            
            var order = await _generalCleaningRepository.AddAsyn(_mapper.Map<GeneralCleaning>(generalCleaningDto));
            response.Result = _mapper.Map<GeneralCleaningDto>(order);
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
    
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<GeneralCleaningDto>>> Put([FromBody] GeneralCleaningDto generalCleaningDto)
    {
        var response = new ApiResponse<GeneralCleaningDto>();
        try
        {
            
            foreach (var item in generalCleaningDto.PhotoGeneralCleanings)
            {
                if (_generalCleaningRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.Photo = _generalCleaningRepository.UploadImageBase64(item.Photo, "Files/GeneralCleaning/", item.PhotoPath);
                    await _photoGeneralCleaningRepository.AddAsyn(_mapper.Map<PhotoGeneralCleaning>(item));
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
            var generalCleaning = await _generalCleaningRepository.UpdateAsync(_mapper.Map<GeneralCleaning>(generalCleaningDto), generalCleaningDto.Id);

            response.Result = _mapper.Map<GeneralCleaningDto>(generalCleaning);
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
            response.Result = await _photoGeneralCleaningRepository.DeleteByAsync(d=>d.Id==id);
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
