using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.Alarm;
using api.rebel_wings.Models.ApiResponse;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Alarm;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers
{
    /// <summary>
    /// Controlador de Alarma
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmController : ControllerBase
    {

        private readonly IAlarmRepository _alarmRepository;
        private readonly IPhotoAlarmRepository _photoAlarmRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="alarmRepository"></param>
        /// <param name="photoAlarmRepository"></param>
        /// <param name="loggerManager"></param>
        /// <param name="mapper"></param>
        public AlarmController(IAlarmRepository alarmRepository,
            IPhotoAlarmRepository photoAlarmRepository,
            ILoggerManager loggerManager,
            IMapper mapper)
        {
            _alarmRepository = alarmRepository;
            _photoAlarmRepository = photoAlarmRepository;
            _logger = loggerManager;
            _mapper = mapper;
        }
        /// <summary>
        /// GET:
        /// Obtiene por ID de Alarma
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<AlarmDto>>> GetById(int id)
        {
            ApiResponse<AlarmDto> response = new ApiResponse<AlarmDto>();
            try
            {
                var alarm = _mapper.Map<AlarmDto>(_alarmRepository.GetAllIncluding(i=>i.PhotoAlarms).FirstOrDefault(f=>f.Id == id));
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
        /// Agregar nuevo registro
        /// </summary>
        /// <param name="alarm">Objeto de alarma</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<AlarmDto>>> Post([FromBody] AlarmDto alarm)
        {
            ApiResponse<AlarmDto> response = new ApiResponse<AlarmDto>();
            try
            {
                foreach (var item in alarm.PhotoAlarms)
                {
                    if (_alarmRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _alarmRepository.UploadImageBase64(item.Photo, "Files/Alarmas/", item.PhotoPath);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }
                }
                
                response.Result = _mapper.Map<AlarmDto>(await _alarmRepository.AddAsyn(_mapper.Map<Alarm>(alarm)));
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
        /// Actualizar 
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        [HttpPut]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<AlarmDto>>> Put([FromBody] AlarmDto alarm)
        {
            ApiResponse<AlarmDto> response = new ApiResponse<AlarmDto>();
            try
            {
                foreach (var item in alarm.PhotoAlarms)
                {
                    if (item.Id == 0 && _photoAlarmRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _photoAlarmRepository.UploadImageBase64(item.Photo, "Files/Alarmas/", item.PhotoPath);
                        await _photoAlarmRepository.AddAsyn(_mapper.Map<PhotoAlarm>(item));
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
                response.Result = _mapper.Map<AlarmDto>(await _alarmRepository.UpdateAsync(_mapper.Map<Alarm>(alarm), alarm.Id));
                response.Result = alarm;
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
        public async Task<ActionResult<ApiResponse<PhotoAlarmDto>>> Delete(int id)
        {
            var response = new ApiResponse<PhotoAlarmDto>();
            try
            {
                if (await _photoAlarmRepository.ExistsAsync(id))
                {
                    var @async = await _photoAlarmRepository.GetAsync(id);
                    await _photoAlarmRepository.DeleteAsyn(@async);
                    response.Success = true;
                    response.Result = _mapper.Map<PhotoAlarmDto>(@async);
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
