using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.LivingRoomBathroomCleaning;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Tip;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers
{
    /// <summary>
    /// Controlador de LIMPIEZA DE SALÓN Y BAÑOS
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LivingRoomBathroomCleaningController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly ILivingRoomBathroomCleaningRepository _livingRoomBathroomCleaningRepository;
        private readonly IPhotoLivingRoomBathroomCleaningRepository _photoLivingRoomBathroomCleaningRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="loggerManager"></param>
        /// <param name="livingRoomBathroomCleaningRepository"></param>
        /// <param name="photoLivingRoomBathroomCleaningRepository"></param>
        public LivingRoomBathroomCleaningController(IMapper mapper, ILoggerManager loggerManager,
            ILivingRoomBathroomCleaningRepository livingRoomBathroomCleaningRepository,
            IPhotoLivingRoomBathroomCleaningRepository photoLivingRoomBathroomCleaningRepository)
        {
            _mapper = mapper;
            _logger = loggerManager;
            _livingRoomBathroomCleaningRepository = livingRoomBathroomCleaningRepository;
            _photoLivingRoomBathroomCleaningRepository = photoLivingRoomBathroomCleaningRepository;
        }

        /// <summary>
        /// GET:
        /// Obtener por ID
        /// </summary>
        /// <param name="id">ID de </param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<LivingRoomBathroomCleaningDto>>> GetById(int id)
        {
            ApiResponse<LivingRoomBathroomCleaningDto> response = new ApiResponse<LivingRoomBathroomCleaningDto>();
            try
            {
                var alarm = _mapper.Map<LivingRoomBathroomCleaningDto>(_livingRoomBathroomCleaningRepository.GetAllIncluding(i => i.PhotoLivingRoomBathroomCleanings).FirstOrDefault(f => f.Id == id));
                response.Result = alarm;
                response.Success = true;
                response.Message = "Consult was success";
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
        /// POST:
        /// Agregar una nueva LIMPIEZA DE SALÓN Y BAÑOS
        /// </summary>
        /// <param name="livingRoomBathroomCleaningDto">Objeto de LIMPIEZA DE SALÓN Y BAÑOS</param>
        /// <returns></returns>
        /// <response code="201">Regresa el nuevo objeto creado</response>
        /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>        
        /// <response code="415">Si la foto no cumple con los requisitos para subirla</response>   
        [HttpPost]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<LivingRoomBathroomCleaningDto>>> Post([FromBody] LivingRoomBathroomCleaningDto livingRoomBathroomCleaningDto)
        {
            ApiResponse<LivingRoomBathroomCleaningDto> response = new ApiResponse<LivingRoomBathroomCleaningDto>();
            try
            {
                foreach (var item in livingRoomBathroomCleaningDto.PhotoLivingRoomBathroomCleanings)
                {
                    if (_livingRoomBathroomCleaningRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _livingRoomBathroomCleaningRepository.UploadImageBase64(item.Photo, "Files/Limpieza/", item.PhotoPath);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }
                }
                await _livingRoomBathroomCleaningRepository.AddAsyn(_mapper.Map<LivingRoomBathroomCleaning>(livingRoomBathroomCleaningDto));
                response.Result = livingRoomBathroomCleaningDto;
                response.Success = true;
                response.Message = "Record was added success";
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
        /// PUT:
        /// Actualizar LIMPIEZA DE SALÓN Y BAÑOS
        /// </summary>
        /// <param name="livingRoomBathroomCleaningDto">Objeto de LIMPIEZA DE SALÓN Y BAÑOS</param>
        /// <returns></returns>
        /// <response code="201">Regresa el nuevo objeto creado</response>
        /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>        
        /// <response code="415">Si la foto no cumple con los requisitos para subirla</response>   
        [HttpPut]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<LivingRoomBathroomCleaningDto>>> Put([FromBody] LivingRoomBathroomCleaningDto livingRoomBathroomCleaningDto)
        {
            ApiResponse<LivingRoomBathroomCleaningDto> response = new ApiResponse<LivingRoomBathroomCleaningDto>();
            try
            {
                foreach (var item in livingRoomBathroomCleaningDto.PhotoLivingRoomBathroomCleanings)
                {
                    if (item.Id == 0 && _photoLivingRoomBathroomCleaningRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _photoLivingRoomBathroomCleaningRepository.UploadImageBase64(item.Photo, "Files/Limpieza/", item.PhotoPath);
                        await _photoLivingRoomBathroomCleaningRepository.AddAsyn(_mapper.Map<PhotoLivingRoomBathroomCleaning>(item));
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
                response.Result = _mapper.Map<LivingRoomBathroomCleaningDto>(await _livingRoomBathroomCleaningRepository.UpdateAsync(_mapper.Map<LivingRoomBathroomCleaning>(livingRoomBathroomCleaningDto), livingRoomBathroomCleaningDto.Id));
                response.Result = livingRoomBathroomCleaningDto;
                response.Success = true;
                response.Message = "Record was added success";
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
        public async Task<ActionResult<ApiResponse<PhotoLivingRoomBathroomCleaningDto>>> Delete(int id)
        {
            var response = new ApiResponse<PhotoLivingRoomBathroomCleaningDto>();
            try
            {
                if (await _photoLivingRoomBathroomCleaningRepository.ExistsAsync(id))
                {
                    var photoToSetTable = await _photoLivingRoomBathroomCleaningRepository.GetAsync(id);
                    await _photoLivingRoomBathroomCleaningRepository.DeleteAsyn(photoToSetTable);
                    response.Success = true;
                    response.Result = _mapper.Map<PhotoLivingRoomBathroomCleaningDto>(photoToSetTable);
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
