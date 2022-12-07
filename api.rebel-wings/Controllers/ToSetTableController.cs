using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ToSetTable;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.ToSetTable;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace api.rebel_wings.Controllers
{
    /// <summary>
    /// Controlador para Salón Montado
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class ToSetTableController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IToSetTableRepository _toSetTableRepository;
        private readonly IPhotoToSetTableRepository _photoToSetTableRepository;
        /// <summary>
        /// Metodo Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <param name="toSetTableRepository"></param>
        public ToSetTableController(IMapper mapper, ILoggerManager logger, IToSetTableRepository toSetTableRepository, IPhotoToSetTableRepository photoToSetTableRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _toSetTableRepository = toSetTableRepository;
            _photoToSetTableRepository = photoToSetTableRepository;
        }

    /// <summary>
    /// GET:
    /// Obtener por ID de Station
    /// </summary>
    /// <param name="id">ID </param>
    /// <returns></returns>
    [HttpGet("By-Id/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<ToSetTableDto>>> GetById(int id)
    {
      Models.ApiResponse.ApiResponse<ToSetTableDto> response = new Models.ApiResponse.ApiResponse<ToSetTableDto>();
        try
        {
            response.Result = _mapper.Map<ToSetTableDto>(_toSetTableRepository.GetAllIncluding(g=>g.PhotoToSetTables).FirstOrDefault(f => f.Id == id));
            response.Message = "Operation was success";
            response.Success = true;
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
    /// Regresa Audio & Video objeto
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<ToSetTableDto>>> Get(int id, int user)
    {
        var response = new Models.ApiResponse.ApiResponse<ToSetTableDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd();
            var order = _toSetTableRepository.GetAllIncluding(i => i.PhotoToSetTables);
            var @default = await order.FirstOrDefaultAsync(f => f.Branch == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<ToSetTableDto>(@default);
            response.Message = "Consult was success";
            response.Success = true;
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
        /// Agregar un nuevo Salón Montado
        /// </summary>
        /// <remarks> 
        /// EXAMPLE :
        /// POST /ToSetTable
        /// {
        ///     "id": 0,
        ///     "branch": 0,
        ///     "photo": "string",
        ///     "comment": "string",
        ///     "createdBy": 0,
        ///     "createdDate": "2021-10-22T17:42:10.356Z",
        ///     "updatedBy": 0,
        ///     "updatedDate": "2021-10-22T17:42:10.356Z",
        ///     "photoToSetTables": [
        ///     {
        ///          "id": 0,
        ///          "toSetTableId": 0, // ID del Salón Montado 
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
        /// <param name="toSetTable">Objeto de Salón Montado</param>
        /// <returns>Regresa respuesta exitosa caso contrario manda error 500 o 400 dependiendo del caso de error </returns>
        /// <response code="201">Regresa el nuevo objeto creado</response>
        /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>        
        /// <response code="415">Si la foto no cumple con los requisitos para subirla</response>        
        [HttpPost]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<Models.ApiResponse.ApiResponse<ToSetTableDto>>> Post([FromBody] ToSetTableDto toSetTable)
        {
            Models.ApiResponse.ApiResponse<ToSetTableDto> response = new Models.ApiResponse.ApiResponse<ToSetTableDto>();
            try 
            {
                foreach (var item in toSetTable.PhotoToSetTables)
                {
                    if (_photoToSetTableRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _photoToSetTableRepository.UploadImageBase64(item.Photo, "Files/SalonMontado/", item.PhotoPath);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }
                }
                
                response.Result = _mapper.Map<ToSetTableDto>(await _toSetTableRepository.AddAsyn(_mapper.Map<ToSetTable>(toSetTable)));
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
        /// Actualiza un Salón Montado
        /// </summary>
        /// <remarks> 
        /// EXAMPLE :
        /// 
        /// PUT /ToSetTable
        /// {
        ///     "id": 0,
        ///     "branch": 0,
        ///     "comment": "string",
        ///     "createdBy": 0, // No Se Modifica se retorna tal cual se mando en el GET
        ///     "createdDate": "2021-10-22T17:42:10.356Z", // No Se Modifica se retorna tal cual se mando en el GET
        ///     "updatedBy": 0, // Quien actualiza el registro 
        ///     "updatedDate": "2021-10-22T17:42:10.356Z" // Fecha de quien actualizo el registro,
        ///     "photoToSetTables": [
        ///     {
        ///          "id": 0,
        ///          "toSetTableId": 0, // ID del Salón Montado 
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
        /// <param name="toSetTable">Objeto de Salón Montado</param>
        /// <returns>Regresa respuesta exitosa caso contrario manda error 500 o 400 dependiendo del caso de error </returns>
        /// <response code="200">Regresa el nuevo objeto creado</response>
        /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>        
        /// <response code="415">Si la foto no cumple con los requisitos para subirla</response>
        [HttpPut]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<Models.ApiResponse.ApiResponse<ToSetTableDto>>> Put([FromBody] ToSetTableDto toSetTable)
        {
            Models.ApiResponse.ApiResponse<ToSetTableDto> response = new Models.ApiResponse.ApiResponse<ToSetTableDto>();
            try
            {
                foreach (var item in toSetTable.PhotoToSetTables)
                {
                    if (item.Id == 0 && _photoToSetTableRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _photoToSetTableRepository.UploadImageBase64(item.Photo, "Files/SalonMontado/", item.PhotoPath);
                        _photoToSetTableRepository.Add(_mapper.Map<PhotoToSetTable>(item));
                    }
                    else if(item.Id != 0 && item.Photo.Length < 251)
                    { 
                        continue; 
                    }
                    else if (item.Id == 0 && !_photoToSetTableRepository.IsBase64(item.Photo))
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }

                }
                response.Result = _mapper.Map<ToSetTableDto>(await _toSetTableRepository.UpdateAsync(_mapper.Map<ToSetTable>(toSetTable), toSetTable.Id));
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
        public async Task<ActionResult<Models.ApiResponse.ApiResponse<PhotoToSetTableDto>>> Delete(int id)
        {
            var response = new Models.ApiResponse.ApiResponse<PhotoToSetTableDto>();
            try
            {
                if (await _photoToSetTableRepository.ExistsAsync(id))
                {
                    var photoToSetTable = await _photoToSetTableRepository.GetAsync(id);
                    await _photoToSetTableRepository.DeleteAsyn(photoToSetTable);
                    response.Success = true;
                    response.Result = _mapper.Map<PhotoToSetTableDto>(photoToSetTable);
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
