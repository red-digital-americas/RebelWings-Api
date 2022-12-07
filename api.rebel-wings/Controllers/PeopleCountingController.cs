using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.PeopleCounting;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.PeopleCounting;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;

  /// <summary>
  /// Conteo de Personas
  /// </summary>
  [ApiController]
  [Route("api/[controller]")]
  [Produces("application/json")]
  public class PeopleCountingController : ControllerBase
  {
    private readonly IPeopleCountingRepository _peopleCountingRepository;
    private readonly IPhotoPeopleCountingRepository _photopPeopleCountingRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="peopleCountingRepository"></param>
    /// <param name="photopPeopleCountingRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>

    public PeopleCountingController(IPeopleCountingRepository peopleCountingRepository, IMapper mapper, ILoggerManager loggerManager, IPhotoPeopleCountingRepository photopPeopleCountingRepository)
    {
      _peopleCountingRepository = peopleCountingRepository;
      _mapper = mapper;
      _loggerManager = loggerManager;
      _photopPeopleCountingRepository = photopPeopleCountingRepository;
    }


    /// <summary>
    /// GET:
    /// Return a list of Orders
    /// </summary>
    /// <param name="id">ID ==> Sucursal ID</param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Models.ApiResponse.ApiResponse<PeopleCountingDto>> Get(int id, int user)
    {
      Models.ApiResponse.ApiResponse<PeopleCountingDto> response = new Models.ApiResponse.ApiResponse<PeopleCountingDto>();
      try
      {
        var today = DateTime.Now.AddDays(-1);
        today = today.AbsoluteEnd().ToUniversalTime();
        var order = _peopleCountingRepository.GetAllIncluding(i => i.PhotoPeoplesCountings)
            .FirstOrDefault(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
        response.Result = _mapper.Map<PeopleCountingDto>(order);
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
    /// Return by ID
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Models.ApiResponse.ApiResponse<PeopleCountingDto>> GetById(int id)
    {
      Models.ApiResponse.ApiResponse<PeopleCountingDto> response = new Models.ApiResponse.ApiResponse<PeopleCountingDto>();
      try
      {
        var today = DateTime.Now.AddDays(-1);
        today = today.AbsoluteEnd();
        var order = _peopleCountingRepository.GetAllIncluding(i => i.PhotoPeoplesCountings)
            .FirstOrDefault(f => f.Id == id);
        response.Result = _mapper.Map<PeopleCountingDto>(order);
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
    /// <param name="peopleCountingDto">Object</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<PeopleCountingDto>>> Post(PeopleCountingDto peopleCountingDto)
    {
      Models.ApiResponse.ApiResponse<PeopleCountingDto> response = new Models.ApiResponse.ApiResponse<PeopleCountingDto>();
      try
      {
        foreach (var item in peopleCountingDto.PhotoPeoplesCountings)
        {
          if (_photopPeopleCountingRepository.IsBase64(item.Photo))
          {
            item.Photo = _photopPeopleCountingRepository.UploadImageBase64(item.Photo, "Files/PeopleCounting/", item.PhotoPath);
          }
          else
          {
            response.Success = false;
            response.Message = "Photo is not Base64";
            return StatusCode(415, response);
          }
        }

        //var peopleCounting = await _peopleCountingRepository.AddAsyn(_mapper.Map<PeopleCounting>(peopleCountingDto));
        //response.Result = _mapper.Map<PeopleCountingDto>(peopleCounting);
        response.Result = _mapper.Map<PeopleCountingDto>(await _peopleCountingRepository.AddAsyn(_mapper.Map<PeopleCounting>(peopleCountingDto)));
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

    ///// <summary>
    ///// UPDATE:
    ///// Update a People Counting Record
    ///// </summary>
    ///// <param name="satisfactionSurveyDto"></param>
    ///// <returns></returns>
    //[HttpPut]
    //[ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    //public async Task<ActionResult<ApiResponse<PeopleCountingDto>>> Put([FromBody] PeopleCountingDto satisfactionSurveyDto)
    //{
    //    var response = new ApiResponse<PeopleCountingDto>();
    //    try
    //    {
    //        var update = await _peopleCountingRepository.UpdateAsync(_mapper.Map<PeopleCounting>(satisfactionSurveyDto),
    //            satisfactionSurveyDto.Id);
    //        response.Result = _mapper.Map<PeopleCountingDto>(update);
    //        response.Message = "Updated was success";
    //        response.Success = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        _loggerManager.LogError(ex.Message);
    //        response.Success = false;
    //        response.Message = ex.ToString();
    //        return StatusCode(500, response);
    //    }
    //    return StatusCode(201, response);
    //}

    /// <summary>
    /// PUT:
    /// Update or Add new Order
    /// </summary>
    /// <param name="peopleDtos"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<PeopleCountingDto>>> Put([FromBody] PeopleCountingDto peopleDtos)
    {
      Models.ApiResponse.ApiResponse<PeopleCountingDto> response = new Models.ApiResponse.ApiResponse<PeopleCountingDto>();
      try
      {
        foreach (var item in peopleDtos.PhotoPeoplesCountings)
        {
          if (_photopPeopleCountingRepository.IsBase64(item.Photo) && item.Id == 0)
          {
            
            item.Photo = _photopPeopleCountingRepository.UploadImageBase64(item.Photo, "Files/PeopleCounting/", item.PhotoPath);
            await _photopPeopleCountingRepository.AddAsyn(_mapper.Map<PhotoPeopleCounting>(item));
          }
          else if (item.Id != 0 && item.Photo.Length < 251)
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

        var updateAsync = await _peopleCountingRepository.UpdateAsync(_mapper.Map<PeopleCounting>(peopleDtos), peopleDtos.Id);

        response.Result = _mapper.Map<PeopleCountingDto>(updateAsync);
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<int>>> Delete(int id)
    {
      var response = new Models.ApiResponse.ApiResponse<int>();
      try
      {
        response.Result = await _photopPeopleCountingRepository.DeleteByAsync(d => d.Id == id);
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
      return StatusCode(201, response);
    }

  }

