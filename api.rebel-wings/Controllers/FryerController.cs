using api.rebel_wings.ActionFilter;
using api.rebel_wings.Extensions;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.FryerCleaning;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Fryer;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controlador de Freidoras
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FryerController : ControllerBase
{
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;
    private readonly IFryerCleaningRepository _fryerCleaningRepository;
    private readonly IPhotoFryerCleaningRepository _photoFryerCleaningRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="loggerManager"></param>
    /// <param name="mapper"></param>
    /// <param name="fryerCleaningRepository"></param>
    /// <param name="photoFryerCleaningRepository"></param>
    public FryerController(ILoggerManager loggerManager,
        IMapper mapper,
        IFryerCleaningRepository fryerCleaningRepository,
        IPhotoFryerCleaningRepository photoFryerCleaningRepository
        )
    {
        _loggerManager = loggerManager;
        _mapper = mapper;
        _fryerCleaningRepository = fryerCleaningRepository;
        _photoFryerCleaningRepository = photoFryerCleaningRepository;
    }
    
    /// <summary>
    /// GET:
    /// Return a Fryer By ID 
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<FryerCleaningDto>> GetById(int id)
    {
        var response = new ApiResponse<FryerCleaningDto>();
        try
        {
            var @default = _fryerCleaningRepository.GetAllIncluding(i => i.PhotoFryerCleanings)
                .FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<FryerCleaningDto>(@default);
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
    /// Return a list fryer cleaning By Branch
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<FryerCleaningDto>>> Get(int id, int user)
    {
        var response = new ApiResponse<List<FryerCleaningDto>>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _fryerCleaningRepository.GetAllIncluding(i => i.PhotoFryerCleanings)
                .Where(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user).ToList();
            response.Result = _mapper.Map<List<FryerCleaningDto>>(order);
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
    public async Task<ActionResult<ApiResponse<List<FryerCleaningDto>>>> Post([FromBody] List<FryerCleaningDto> fryerCleaningDtos)
    {
        var response = new ApiResponse<List<FryerCleaningDto>>();
        try
        {
            var orders = new List<FryerCleaningDto>();
            foreach (var fryerCleaningDto in fryerCleaningDtos)
            {
                foreach (var item in fryerCleaningDto.PhotoFryerCleanings)
                {
                    if (_fryerCleaningRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _fryerCleaningRepository.UploadImageBase64(item.Photo, "Files/Fryer/", item.PhotoPath);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }
                }
                var order = await _fryerCleaningRepository.AddAsyn(_mapper.Map<FryerCleaning>(fryerCleaningDto));
                orders.Add(_mapper.Map<FryerCleaningDto>(order));
            }
           
            response.Result = orders;
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
    /// PUT:
    /// Update a List of fryer cleaning 
    /// </summary>
    /// <param name="fryerCleaningDtos"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<FryerCleaningDto>>>> Put([FromBody] List<FryerCleaningDto> fryerCleaningDtos)
    {
        var response = new ApiResponse<List<FryerCleaningDto>>();
        try
        {
            var dtos = new List<FryerCleaningDto>();
            foreach (var fryerCleaningDto in fryerCleaningDtos)
            {
                if (fryerCleaningDto.Id != 0)
                {
                    foreach (var item in fryerCleaningDto.PhotoFryerCleanings)
                    {
                        if (_fryerCleaningRepository.IsBase64(item.Photo) && item.Id == 0)
                        {
                            item.FryerCleaningId = fryerCleaningDto.Id;
                            item.Photo = _fryerCleaningRepository.UploadImageBase64(item.Photo, "Files/Fryer/", item.PhotoPath);
                            await _photoFryerCleaningRepository.AddAsyn(_mapper.Map<PhotoFryerCleaning>(item));
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
                
                    var fryerCleaning = await _fryerCleaningRepository.UpdateAsync(_mapper.Map<FryerCleaning>(fryerCleaningDto), fryerCleaningDto.Id);
                    dtos.Add(_mapper.Map<FryerCleaningDto>(fryerCleaning));
                }
                else
                {
                    foreach (var item in fryerCleaningDto.PhotoFryerCleanings)
                    {
                        if (_fryerCleaningRepository.IsBase64(item.Photo))
                        {
                            item.Photo = _fryerCleaningRepository.UploadImageBase64(item.Photo, "Files/Fryer/", item.PhotoPath);
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Photo is not Base64";
                            return StatusCode(415, response);
                        }
                    }
                    var fryerCleaning = await _fryerCleaningRepository.AddAsyn(_mapper.Map<FryerCleaning>(fryerCleaningDto));
                    dtos.Add(_mapper.Map<FryerCleaningDto>(fryerCleaning));
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
            response.Result = await _photoFryerCleaningRepository.DeleteByAsync(d=>d.Id==id);
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
