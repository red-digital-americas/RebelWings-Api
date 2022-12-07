using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.WaitlistTable;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.WaitlistTable;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers
{
  /// <summary>
  /// Mesas en Espera Controlador
  /// </summary>
  [Produces("application/json")]
  [ApiController]
  [Route("api/[controller]")]
  public class WaitListTableController : ControllerBase
  {

    private readonly IWaitlistTableRepository _waitlistTableRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly IPhotoWaitlistTableRepository _photoWaitlistTableRepository;
    /// <summary>
    /// Contructor
    /// </summary>
    /// <param name="waitlistTableRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="photoWaitlistTableRepository"></param>
    public WaitListTableController(IWaitlistTableRepository waitlistTableRepository, IMapper mapper, ILoggerManager logger, IPhotoWaitlistTableRepository photoWaitlistTableRepository)
    {
      _waitlistTableRepository = waitlistTableRepository;
      _mapper = mapper;
      _logger = logger;
      _photoWaitlistTableRepository = photoWaitlistTableRepository;
    }



    /// <summary>
    /// POST para agregar un nuevo Mesas en Espera
    /// </summary>
    /// <remarks>
    /// Sample Request:
    /// {
    /// "id": 0,
    /// "branch": 1,
    ///  "waitlistTables": true,
    ///  "howManyTables": 2,
    ///  "numberPeople": 5,
    ///  "createdBy": 1,
    ///  "createdDate": "2021-10-26T17:54:55.450Z",
    ///  "updatedBy": null,
    ///  "updatedDate": null
    /// }
    /// </remarks>
    /// <param name="waitlistTable"></param>
    /// <returns></returns>
    /// <response code="200">Regresa respuesta exitosa de objeto agregado</response>
    /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>
    /// <response code="500">Error Interno</response>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ActionFilter.ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<WaitlistTableDto>>> Post(WaitlistTableDto waitlistTable)
    {
      Models.ApiResponse.ApiResponse<WaitlistTableDto> response = new Models.ApiResponse.ApiResponse<WaitlistTableDto>();
      try
      {
        foreach (var photoValidation in waitlistTable.PhotoWaitlistTables)
        {
          if (_waitlistTableRepository.IsBase64(photoValidation.Photo))
          {
            photoValidation.Photo = _waitlistTableRepository.UploadImageBase64(photoValidation.Photo, "Files/WaitlistTables/", photoValidation.PhotoPath);
          }
          else
          {
            response.Success = false;
            response.Message = "Photo is not Base64";
            return StatusCode(415, response);
          }
        }


        response.Result = _mapper.Map<WaitlistTableDto>(await _waitlistTableRepository.AddAsyn(_mapper.Map<WaitlistTable>(waitlistTable)));
        response.Success = true;
        response.Message = "Record was added succesfully.";
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        response.Success = false;
        response.Message = ex.Message;
        return StatusCode(500, response);
      }
      return StatusCode(201, response);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <remarks> 
    /// EXAMPLE :
    /// 
    /// PUT /WaitListTable
    /// {
    ///     "id": 0,
    ///     "branch": 0,
    ///     "waitlistTables": true,
    ///     "howManyTables": 0,
    ///     "numberPeople": 0,
    ///     "createdBy": 0,
    ///     "createdDate": "2022-02-08T20:55:05.850Z",
    ///     "updatedBy": 0,
    ///     "updatedDate":
    ///     "updatedDate": "2021-10-22T19:51:42.190Z"
    /// }
    /// </remarks>
    /// <param name="waitListTable">Object to send in From body</param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ActionFilter.ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<WaitlistTableDto>>> Put(WaitlistTableDto waitListTable)
    {
      Models.ApiResponse.ApiResponse<WaitlistTableDto> response = new Models.ApiResponse.ApiResponse<WaitlistTableDto>();
      try
      {
        foreach (PhotoWaitlistTableDto photoWaitlistTa in waitListTable.PhotoWaitlistTables)
        {
          if (photoWaitlistTa.Id == 0 && _photoWaitlistTableRepository.IsBase64(photoWaitlistTa.Photo))
          {
            photoWaitlistTa.Photo = _photoWaitlistTableRepository.UploadImageBase64(photoWaitlistTa.Photo, "Files/WaitlistTables/", photoWaitlistTa.PhotoPath);
            _photoWaitlistTableRepository.Add(_mapper.Map<PhotoWaitlistTable>(photoWaitlistTa));
          }
          else if (photoWaitlistTa.Id != 0 && photoWaitlistTa.Photo.Length < 251)
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

        response.Result = _mapper.Map<WaitlistTableDto>(await _waitlistTableRepository.UpdateAsync(_mapper.Map<WaitlistTable>(waitListTable), waitListTable.Id));
        response.Success = true;
        response.Message = "Record was updated succesfully.";
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        response.Success = false;
        response.Message = ex.Message;
        return StatusCode(500, response);
      }
      return StatusCode(200, response);
    }

    /// <summary>
    /// GET:
    /// Regresa lista de lista de esperas por sucursal
    /// </summary>
    /// <returns>Regresa lista de estatus</returns>
    /// <response code="200">Regresa el nuevo objeto creado</response>
    /// <response code="404">No existe</response>        
    /// <response code="500">Error interno de servidor</response> 
    [HttpGet("{id}/Branch")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Models.ApiResponse.ApiResponse<List<WaitlistTableDto>>> GetByBranch(int id)
    {
      var response = new Models.ApiResponse.ApiResponse<List<WaitlistTableDto>>();
      try
      {
        DateTime date = DateTime.Now;
        var waitlistTableDtos = _mapper.Map<List<WaitlistTableDto>>(_waitlistTableRepository.GetAll());
        response.Result = waitlistTableDtos.Where(x =>
                x.Branch == id && x.CreatedDate >= date.AbsoluteStart() && x.CreatedDate <= date.AbsoluteEnd())
            .ToList();
        response.Success = true;
        response.Message = "Consult was success";
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        response.Success = false;
        response.Message = ex.ToString();
        return StatusCode(500, response);
      }
      return StatusCode(200, response);
    }

    /// <summary>
    /// Retorna la validacion de mesas en espera por ID
    /// </summary>
    /// <param name="id">ID del salón montado</param>
    /// <returns></returns>
    [HttpGet]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [Route("{id}")]
    public ActionResult<Models.ApiResponse.ApiResponse<WaitlistTableDto>> GetById(int id)
    {
      Models.ApiResponse.ApiResponse<WaitlistTableDto> response = new Models.ApiResponse.ApiResponse<WaitlistTableDto>();
      try
      {
        var res = _mapper.Map<WaitlistTableDto>(_waitlistTableRepository.GetAllIncluding(i => i.PhotoWaitlistTables).First(f => f.Id == id));
        response.Success = true;
        response.Message = "Consult was successfully";
        response.Result = res;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        response.Success = false;
        response.Message = ex.Message;
        return StatusCode(500, response);
      }
      return StatusCode(200, response);
    }

    /// <summary>
    /// Delete:
    /// Remove photo by ID
    /// </summary>
    /// <param name="id">ID </param>
    /// <returns></returns>
    [HttpDelete("{id}/Photo")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<PhotoWaitlistTableDto>>> Delete(int id)
    {
      var response = new Models.ApiResponse.ApiResponse<PhotoWaitlistTableDto>();
      try
      {
        if (await _photoWaitlistTableRepository.ExistsAsync(id))
        {
          var photoToSetTable = await _photoWaitlistTableRepository.GetAsync(id);
          await _photoWaitlistTableRepository.DeleteAsyn(photoToSetTable);
          response.Success = true;
          response.Result = _mapper.Map<PhotoWaitlistTableDto>(photoToSetTable);
          response.Message = "Record was deleted successfully.";
        }
        else
        {
          _logger.LogError("Not Exist Record");
          response.Success = false;
          response.Message = "Record not exist.";
          return StatusCode(404, response);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        response.Success = false;
        response.Message = ex.Message;
        return StatusCode(500, response);
      }
      return StatusCode(200, response);
    }

  }
}
