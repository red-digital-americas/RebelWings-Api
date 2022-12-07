using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ValidationGas;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.ValidationGas;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers
{
  /// <summary>
  /// Contralador de Validación de Gas
  /// </summary>
  [Produces("application/json")]
  [ApiController]
  [Route("api/[controller]")]
  public class ValidationGasController : Controller
  {
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly IValidationGasRepository _validationGasRepository;
    private readonly IPhotoValidationGasRepository _photoValidationGasRepository;
    /// <summary>
    /// Contructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="validationGasRepository"></param>
    /// <param name="photoValidationGasRepository"></param>
    public ValidationGasController(IMapper mapper, ILoggerManager logger, IValidationGasRepository validationGasRepository, IPhotoValidationGasRepository photoValidationGasRepository)
    {
      _logger = logger;
      _mapper = mapper;
      _validationGasRepository = validationGasRepository;
      _photoValidationGasRepository = photoValidationGasRepository;
    }
    /// <summary>
    /// Retorna el validadción de gas por ID
    /// </summary>
    /// <param name="id">ID del salón montado</param>
    /// <returns></returns>
    [HttpGet]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [Route("{id}")]
    public ActionResult<Models.ApiResponse.ApiResponse<ValidationGaDto>> GetById(int id)
    {
      Models.ApiResponse.ApiResponse<ValidationGaDto> response = new Models.ApiResponse.ApiResponse<ValidationGaDto>();
      try
      {
        var res = _mapper.Map<ValidationGaDto>(_validationGasRepository.GetAllIncluding(i => i.PhotoValidationGas).First(f => f.Id == id));
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
    /// Agrega Validación de Gas
    /// </summary>
    /// <remarks> 
    /// EXAMPLE :
    /// POST /ValidationGas
    /// {
    /// "id": 0,
    /// "branch": 0,
    /// "amount": 0,
    /// "photo": "string",
    /// "photoPath": "string",
    /// "comment": "string",
    /// "createdBy": 0,
    ///  "createdDate": "2021-10-22T19:51:42.190Z",
    ///  "updatedBy": 0,
    ///  "updatedDate": "2021-10-22T19:51:42.190Z"
    /// }
    /// </remarks>
    /// <param name="validationGa">Objeto de Validación Gas</param>
    /// <returns>Regresa respuesta exitosa caso contrario manda error 500 o 400 dependiendo del caso de error </returns>
    /// <response code="201">Regresa el nuevo objeto creado</response>
    /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>        
    /// <response code="415">Si la foto no cumple con los requisitos para subirla</response>        
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<ValidationGaDto>>> Post([FromBody] ValidationGaDto validationGa)
    {
      Models.ApiResponse.ApiResponse<ValidationGaDto> response = new Models.ApiResponse.ApiResponse<ValidationGaDto>();
      try
      {
        foreach (var photoValidation in validationGa.PhotoValidationGas)
        {
          if (_validationGasRepository.IsBase64(photoValidation.Photo))
          {
            photoValidation.Photo = _validationGasRepository.UploadImageBase64(photoValidation.Photo, "Files/ValidacionGas/", photoValidation.PhotoPath);
          }
          else
          {
            response.Success = false;
            response.Message = "Photo is not Base64";
            return StatusCode(415, response);
          }
        }


        response.Result = _mapper.Map<ValidationGaDto>(await _validationGasRepository.AddAsyn(_mapper.Map<ValidationGa>(validationGa)));
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
    /// Actualiza Validación de Gas
    /// </summary>
    /// <remarks> 
    /// EXAMPLE :
    /// 
    /// PUT /ValidationGas
    /// {
    /// "id": 0,
    /// "branch": 0,
    /// "amount": 0,
    /// "photo": "string",
    /// "photoPath": "string",
    /// "comment": "string",
    /// "createdBy": 0,
    ///  "createdDate": "2021-10-22T19:51:42.190Z",
    ///  "updatedBy": 0,
    ///  "updatedDate": "2021-10-22T19:51:42.190Z"
    /// }
    /// </remarks>
    /// <param name="validationGa">Objeto de Validación de Gas</param>
    /// <returns>Regresa respuesta exitosa caso contrario manda error 500 o 400 dependiendo del caso de error </returns>
    /// <response code="200">Regresa el nuevo objeto creado</response>
    /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>        
    /// <response code="415">Si la foto no cumple con los requisitos para subirla</response>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<ValidationGaDto>>> Put([FromBody] ValidationGaDto validationGa)
    {
      Models.ApiResponse.ApiResponse<ValidationGaDto> response = new Models.ApiResponse.ApiResponse<ValidationGaDto>();
      try
      {
        foreach (PhotoValidationGaDto photoValidationGa in validationGa.PhotoValidationGas)
        {
          if (photoValidationGa.Id == 0 && _photoValidationGasRepository.IsBase64(photoValidationGa.Photo))
          {
            photoValidationGa.Photo = _photoValidationGasRepository.UploadImageBase64(photoValidationGa.Photo, "Files/SalonMontado/", photoValidationGa.PhotoPath);
            _photoValidationGasRepository.Add(_mapper.Map<PhotoValidationGa>(photoValidationGa));
          }
          else if (photoValidationGa.Id != 0 && photoValidationGa.Photo.Length < 251)
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

        response.Result = _mapper.Map<ValidationGaDto>(await _validationGasRepository.UpdateAsync(_mapper.Map<ValidationGa>(validationGa), validationGa.Id));
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
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<PhotoValidationGaDto>>> Delete(int id)
    {
      var response = new Models.ApiResponse.ApiResponse<PhotoValidationGaDto>();
      try
      {
        if (await _photoValidationGasRepository.ExistsAsync(id))
        {
          var photoToSetTable = await _photoValidationGasRepository.GetAsync(id);
          await _photoValidationGasRepository.DeleteAsyn(photoToSetTable);
          response.Success = true;
          response.Result = _mapper.Map<PhotoValidationGaDto>(photoToSetTable);
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
