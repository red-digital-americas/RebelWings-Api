using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.SatisfactionSurvey;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.SatisfactionSurvey;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.rebel_wings.Controllers;
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SatisfactionSurveyController : ControllerBase
{
    private readonly ISatisfactionSurveyRepository _satisfactionSurveyRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
  private readonly IPhotoSatisfactionSurveyRepository _photoSatisfactionSurveyRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="satisfactionSurveyRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    public SatisfactionSurveyController(
        ISatisfactionSurveyRepository satisfactionSurveyRepository,
        IPhotoSatisfactionSurveyRepository photoSatisfactionSurveyRepository,
        IMapper mapper,
        ILoggerManager loggerManager)
    {
        _satisfactionSurveyRepository = satisfactionSurveyRepository;
        _mapper = mapper;
        _loggerManager = loggerManager;
        _photoSatisfactionSurveyRepository = photoSatisfactionSurveyRepository;
    }
    
    /// <summary>
    /// GET:
    /// Return a Satisfaction Survey By ID 
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<SatisfactionSurveyDto>> GetById(int id)
    {
        var response = new ApiResponse<SatisfactionSurveyDto>();
        try
        {
            response.Result = _mapper.Map<SatisfactionSurveyDto>(_satisfactionSurveyRepository.GetAllIncluding(g=>g.PhotoSatisfactionSurveys).FirstOrDefault(f => f.Id == id));
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
        return StatusCode(201, response);
    }
    
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<SatisfactionSurveyDto>>> Get(int id, int user)
    {
        var response = new ApiResponse<SatisfactionSurveyDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _satisfactionSurveyRepository.GetAllIncluding(i => i.PhotoSatisfactionSurveys);
            var @default = await order.FirstOrDefaultAsync(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<SatisfactionSurveyDto>(@default);
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
    public async Task<ActionResult<ApiResponse<SatisfactionSurveyDto>>> Post([FromBody] SatisfactionSurveyDto satisfactionSurveyDto)
    {
        var response = new ApiResponse<SatisfactionSurveyDto>();
        try
        {
            foreach (var item in satisfactionSurveyDto.PhotoSatisfactionSurveys)
            {
                if (_photoSatisfactionSurveyRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoSatisfactionSurveyRepository.UploadImageBase64(item.Photo, "Files/Encuesta/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var encuesta = await _satisfactionSurveyRepository.AddAsyn(_mapper.Map<SatisfactionSurvey>(satisfactionSurveyDto));
            response.Result = _mapper.Map<SatisfactionSurveyDto>(encuesta);
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<SatisfactionSurveyDto>>> Put([FromBody] SatisfactionSurveyDto satisfactionSurveyDto)
    {
        var response = new ApiResponse<SatisfactionSurveyDto>();
        try
        {
            foreach (var item in satisfactionSurveyDto.PhotoSatisfactionSurveys)
            {
                if (_photoSatisfactionSurveyRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.SatisfactionSurveyId = satisfactionSurveyDto.Id;
                    item.Photo = _photoSatisfactionSurveyRepository.UploadImageBase64(item.Photo, "Files/Encuesta/", item.PhotoPath);
                    await _photoSatisfactionSurveyRepository.AddAsyn(_mapper.Map<PhotoSatisfactionSurvey>(item));
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
            var update = await _satisfactionSurveyRepository.UpdateAsync(_mapper.Map<SatisfactionSurvey>(satisfactionSurveyDto),
                satisfactionSurveyDto.Id);
            response.Result = _mapper.Map<SatisfactionSurveyDto>(update);
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
      response.Result = await _photoSatisfactionSurveyRepository.DeleteByAsync(d => d.Id == id);
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
