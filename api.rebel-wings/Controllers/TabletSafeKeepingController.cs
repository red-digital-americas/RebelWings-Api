using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.LivingRoomBathroomCleaning;
using api.rebel_wings.Models.TabletSafeKeeping;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Alarm;
using biz.rebel_wings.Repository.TabletSafeKeeping;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers
{
    /// <summary>
    /// Controlador de Resguardo de Tableta
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TabletSafeKeepingController : ControllerBase
    {
        private readonly ITabletSafeKeepingRepository _tabletSafeKeepingRepository;
        private readonly IPhotoTabletSafeKeepingRepository _photoTabletSafeKeepingRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tabletSafeKeepingRepository"></param>
        /// <param name="photoTabletSafeKeepingRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public TabletSafeKeepingController(ITabletSafeKeepingRepository tabletSafeKeepingRepository,
            IPhotoTabletSafeKeepingRepository photoTabletSafeKeepingRepository,
            IMapper mapper,
            ILoggerManager logger)
        {
            _tabletSafeKeepingRepository = tabletSafeKeepingRepository;
            _photoTabletSafeKeepingRepository = photoTabletSafeKeepingRepository;
            _mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        /// GET:
        /// Obtener por ID
        /// </summary>
        /// <param name="id">ID de </param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<TabletSafeKeepingDto>>> GetById(int id)
        {
            ApiResponse<TabletSafeKeepingDto> response = new ApiResponse<TabletSafeKeepingDto>();
            try
            {
                var alarm = _mapper.Map<TabletSafeKeepingDto>(_tabletSafeKeepingRepository.GetAllIncluding(i => i.PhotoTabletSageKeepings).FirstOrDefault(f => f.Id == id));
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
        /// Agregar una nueva Resguardo de Tableta
        /// </summary>
        /// <param name="tabletSafeKeeping">Objeto de Resguardo de Tableta</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<TabletSafeKeepingDto>>> Post([FromBody] TabletSafeKeepingDto tabletSafeKeeping)
        {
            ApiResponse<TabletSafeKeepingDto> response = new ApiResponse<TabletSafeKeepingDto>();
            try
            {
                foreach (var item in tabletSafeKeeping.PhotoTabletSageKeepings)
                {
                    if (_tabletSafeKeepingRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _tabletSafeKeepingRepository.UploadImageBase64(item.Photo, "Files/ResguardoTableta/", item.PhotoPath);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }
                }
                await _tabletSafeKeepingRepository.AddAsyn(_mapper.Map<TabletSafeKeeping>(tabletSafeKeeping));
                response.Result = tabletSafeKeeping;
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
        /// Actualizar la Resguardo de Tableta
        /// </summary>
        /// <param name="tabletSafeKeeping">Objeto de Resguardo de Tableta</param>
        /// <returns></returns>
        [HttpPut]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<TabletSafeKeepingDto>>> Put([FromBody] TabletSafeKeepingDto tabletSafeKeeping)
        {
            ApiResponse<TabletSafeKeepingDto> response = new ApiResponse<TabletSafeKeepingDto>();
            try
            {
                foreach (var item in tabletSafeKeeping.PhotoTabletSageKeepings)
                {
                    if (item.Id == 0 && _tabletSafeKeepingRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _tabletSafeKeepingRepository.UploadImageBase64(item.Photo, "Files/ResguardoTableta/", item.PhotoPath);
                        await _photoTabletSafeKeepingRepository.AddAsyn(_mapper.Map<PhotoTabletSageKeeping>(item));
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
                response.Result = _mapper.Map<TabletSafeKeepingDto>(await _tabletSafeKeepingRepository.UpdateAsync(_mapper.Map<TabletSafeKeeping>(tabletSafeKeeping), tabletSafeKeeping.Id));
                response.Result = tabletSafeKeeping;
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
        public async Task<ActionResult<ApiResponse<PhotoTabletSageKeepingDto>>> Delete(int id)
        {
            var response = new ApiResponse<PhotoTabletSageKeepingDto>();
            try
            {
                if (await _photoTabletSafeKeepingRepository.ExistsAsync(id))
                {
                    var @async = await _photoTabletSafeKeepingRepository.GetAsync(id);
                    await _photoTabletSafeKeepingRepository.DeleteAsyn(@async);
                    response.Success = true;
                    response.Result = _mapper.Map<PhotoTabletSageKeepingDto>(@async);
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
