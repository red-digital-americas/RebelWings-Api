using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.BanosMatutino;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.BanosMatutino;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers
{
  /// <summary>
  /// Controlador para Baños Matutinos
  /// </summary>
  [Produces("application/json")]
  [ApiController]
  [Route("api/[controller]")]
  public class BanosMatutinoController : ControllerBase
  {

    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly IBanosMatutinoRepository _banosMatutinoRepository;
    private readonly IPhotoBanosMatutinoRepository _photoBanosMatutinoRepository;
    /// <summary>
    /// Metodo Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="banosMatutinoRepository"></param>
    public BanosMatutinoController(IMapper mapper, ILoggerManager logger, IBanosMatutinoRepository banosMatutinoRepository, IPhotoBanosMatutinoRepository photoBanosMatutinoRepository)
    {
      _mapper = mapper;
      _logger = logger;
      _banosMatutinoRepository = banosMatutinoRepository;
      _photoBanosMatutinoRepository = photoBanosMatutinoRepository;
    }
    /// <summary>
    /// Retorna el Baños Matutino por ID
    /// </summary>
    /// <param name="id">ID Baños</param>
    /// <returns></returns>
    [HttpGet]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [Route("{id}")]
    public ActionResult<Models.ApiResponse.ApiResponse<BanosMatutinoDto>> GetById(int id)
    {
      Models.ApiResponse.ApiResponse<BanosMatutinoDto> response = new Models.ApiResponse.ApiResponse<BanosMatutinoDto>();
      try
      {
        var res = _mapper.Map<BanosMatutinoDto>(_banosMatutinoRepository.GetAllIncluding(i => i.PhotoBanosMatutinos).FirstOrDefault(f => f.Id == id));
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
    /// Agregar un nuevo Baños Matutino
    /// </summary>
    /// <remarks> 
    /// EXAMPLE :
    /// POST /BanosMatutino
    /// {
    ///     "id": 0,
    ///     "branch": 0,
    ///     "photo": "string",
    ///     "comment": "string",
    ///     "createdBy": 0,
    ///     "createdDate": "2021-10-22T17:42:10.356Z",
    ///     "updatedBy": 0,
    ///     "updatedDate": "2021-10-22T17:42:10.356Z",
    ///     "photoBanosMatutinos": [
    ///     {
    ///          "id": 0,
    ///          "banosMatutinoId": 0, // ID del Salón Montado 
    ///          "photoPath": "string", // Extensión de Foto
    ///          "photo": "string", // Base64 de la Foto
    ///          "createdBy": 0, 
    ///          "createdDate": "2021-10-22T18:25:53.959Z",
    ///          "updatedBy": 0, // Quien actualiza el registro 
    ///          "updatedDate": "2021-10-22T18:25:53.959Z" // Fecha de quien actualizo el registro,
    ///        }
    ///      ]
    ///     }
    /// </remarks>
    /// <param name="banosMatutino">Objeto de Salón Montado</param>
    /// <returns>Regresa respuesta exitosa caso contrario manda error 500 o 400 dependiendo del caso de error </returns>
    /// <response code="201">Regresa el nuevo objeto creado</response>
    /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>        
    /// <response code="415">Si la foto no cumple con los requisitos para subirla</response>        
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<BanosMatutinoDto>>> Post([FromBody] BanosMatutinoDto banosMatutino)
    {
      Models.ApiResponse.ApiResponse<BanosMatutinoDto> response = new Models.ApiResponse.ApiResponse<BanosMatutinoDto>();
      try
      {
        foreach (var item in banosMatutino.PhotoBanosMatutinos)
        {
          if (_photoBanosMatutinoRepository.IsBase64(item.Photo))
          {
            item.Photo = _photoBanosMatutinoRepository.UploadImageBase64(item.Photo, "Files/BanosMatutino/", item.PhotoPath);
          }
          else
          {
            response.Success = false;
            response.Message = "Photo is not Base64";
            return StatusCode(415, response);
          }
        }

        response.Result = _mapper.Map<BanosMatutinoDto>(await _banosMatutinoRepository.AddAsyn(_mapper.Map<BanosMatutino>(banosMatutino)));
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
    /// Actualiza un Baños Matutino
    /// </summary>
    /// <remarks> 
    /// EXAMPLE :
    /// 
    /// PUT /BanosMatutino
    /// {
    ///     "id": 0,
    ///     "branch": 0,
    ///     "comment": "string",
    ///     "createdBy": 0, // No Se Modifica se retorna tal cual se mando en el GET
    ///     "createdDate": "2021-10-22T17:42:10.356Z", // No Se Modifica se retorna tal cual se mando en el GET
    ///     "updatedBy": 0, // Quien actualiza el registro 
    ///     "updatedDate": "2021-10-22T17:42:10.356Z" // Fecha de quien actualizo el registro,
    ///     "photoBanosMatutinos": [
    ///     {
    ///          "id": 0,
    ///          "banosMatutinoId": 0, // ID del Salón Montado 
    ///          "photoPath": "string", // Extensión de Foto
    ///          "photo": "string", // Base64 de la Foto
    ///          "createdBy": 0, 
    ///          "createdDate": "2021-10-22T18:25:53.959Z",
    ///          "updatedBy": 0, // Quien actualiza el registro 
    ///          "updatedDate": "2021-10-22T18:25:53.959Z" // Fecha de quien actualizo el registro,
    ///        }
    ///      ]
    ///     }
    /// </remarks>
    /// <param name="banosMatutino">Objeto de Salón Montado</param>
    /// <returns>Regresa respuesta exitosa caso contrario manda error 500 o 400 dependiendo del caso de error </returns>
    /// <response code="200">Regresa el nuevo objeto creado</response>
    /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>        
    /// <response code="415">Si la foto no cumple con los requisitos para subirla</response>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<BanosMatutinoDto>>> Put([FromBody] BanosMatutinoDto banosMatutino)
    {
      Models.ApiResponse.ApiResponse<BanosMatutinoDto> response = new Models.ApiResponse.ApiResponse<BanosMatutinoDto>();
      try
      {
        foreach (var item in banosMatutino.PhotoBanosMatutinos)
        {
          if (item.Id == 0 && _photoBanosMatutinoRepository.IsBase64(item.Photo))
          {
            item.Photo = _photoBanosMatutinoRepository.UploadImageBase64(item.Photo, "Files/BanosMatutino/", item.PhotoPath);
            _photoBanosMatutinoRepository.Add(_mapper.Map<PhotoBanosMatutino>(item));
          }
          else if (item.Id != 0 && item.Photo.Length < 251)
          {
            continue;
          }
          else if (item.Id == 0 && !_photoBanosMatutinoRepository.IsBase64(item.Photo))
          {
            response.Success = false;
            response.Message = "Photo is not Base64";
            return StatusCode(415, response);
          }

        }
        response.Result = _mapper.Map<BanosMatutinoDto>(await _banosMatutinoRepository.UpdateAsync(_mapper.Map<BanosMatutino>(banosMatutino), banosMatutino.Id));
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
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<PhotoBanosMatutinoDto>>> Delete(int id)
    {
      var response = new Models.ApiResponse.ApiResponse<PhotoBanosMatutinoDto>();
      try
      {
        if (await _photoBanosMatutinoRepository.ExistsAsync(id))
        {
          var photoBanosMatutino = await _photoBanosMatutinoRepository.GetAsync(id);
          await _photoBanosMatutinoRepository.DeleteAsyn(photoBanosMatutino);
          response.Success = true;
          response.Result = _mapper.Map<PhotoBanosMatutinoDto>(photoBanosMatutino);
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
