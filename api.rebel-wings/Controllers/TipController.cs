using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Tip;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Tip;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers
{
    /// <summary>
    /// Controlador de RESGUARDO DE PROPINA
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TipController : ControllerBase
    {
        private readonly ITipRepository _tipRepository;
        private readonly IPhotoTipRepository _photoTipRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tipRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public TipController(ITipRepository tipRepository,
            IPhotoTipRepository photoTipRepository,
            IMapper mapper,
            ILoggerManager logger)
        {
            _tipRepository = tipRepository;
            _mapper = mapper;
            _logger = logger;
            _photoTipRepository = photoTipRepository;
        }
        /// <summary>
        /// GET:
        /// Obtener por ID
        /// </summary>
        /// <param name="id">ID de </param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<TipDto>>> GetById(int id)
        {
            ApiResponse<TipDto> response = new ApiResponse<TipDto>();
            try
            {
                var alarm = _mapper.Map<TipDto>(_tipRepository.GetAllIncluding(i => i.PhotoTips).FirstOrDefault(f => f.Id == id));
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
        /// Agregar una nueva RESGUARDO DE PROPINA
        /// </summary>
        /// <param name="tip">Objeto de RESGUARDO DE PROPINA</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<TipDto>>> Post([FromBody] TipDto tip)
        {
            ApiResponse<TipDto> response = new ApiResponse<TipDto>();
            try
            {
                foreach (var item in tip.PhotoTips)
                {
                    if (_tipRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _tipRepository.UploadImageBase64(item.Photo, "Files/ResguardoPropina/", item.PhotoPath);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }
                }
                await _tipRepository.AddAsyn(_mapper.Map<Tip>(tip));
                response.Result = tip;
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
        /// Actualizar la RESGUARDO DE PROPINA
        /// </summary>
        /// <param name="tip">Objeto de RESGUARDO DE PROPINA</param>
        /// <returns></returns>
        [HttpPut]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<TipDto>>> Put([FromBody] TipDto tip)
        {
            ApiResponse<TipDto> response = new ApiResponse<TipDto>();
            try
            {
                foreach (var item in tip.PhotoTips)
                {
                    if (item.Id == 0 && _photoTipRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _photoTipRepository.UploadImageBase64(item.Photo, "Files/ResguardoPropina/", item.PhotoPath);
                        await _photoTipRepository.AddAsyn(_mapper.Map<PhotoTip>(item));
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
                response.Result = _mapper.Map<TipDto>(await _tipRepository.UpdateAsync(_mapper.Map<Tip>(tip), tip.Id));
                response.Result = tip;
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
        public async Task<ActionResult<Models.ApiResponse.ApiResponse<PhotoTipDto>>> Delete(int id)
        {
            var response = new Models.ApiResponse.ApiResponse<PhotoTipDto>();
            try
            {
                if (await _photoTipRepository.ExistsAsync(id))
                {
                    var photoToSetTable = await _photoTipRepository.GetAsync(id);
                    await _photoTipRepository.DeleteAsyn(photoToSetTable);
                    response.Success = true;
                    response.Result = _mapper.Map<PhotoTipDto>(photoToSetTable);
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
