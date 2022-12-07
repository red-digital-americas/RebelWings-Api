using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.CashRegisterShortage;
using AutoMapper;
using biz.bd1.Repository.Tesoreria;
using biz.bd2.Repository.Tesoreria;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.CashRegisterShortage;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers
{
    /// <summary>
    /// Controlador de Volado de efectivo
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CashRegisterShortageController : ControllerBase
    {
        private readonly ICashRegisterShortageRepository _cashRegisterShortageRepository;
        private readonly IPhotoCashRegisterShortageRepository _photoCashRegisterShortageRepository;
        private readonly biz.bd1.Repository.Tesoreria.ITesoreriaRepository _tesoreriaDB1Repository;
        private readonly biz.bd2.Repository.Tesoreria.ITesoreriaRepository _tesoreriaDB2Repository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="cashRegisterShortageRepository"></param>
        /// <param name="photoCashRegisterShortageRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public CashRegisterShortageController(ICashRegisterShortageRepository cashRegisterShortageRepository,
            IPhotoCashRegisterShortageRepository photoCashRegisterShortageRepository,
            biz.bd1.Repository.Tesoreria.ITesoreriaRepository tesoreriaDB1Repository,
            biz.bd2.Repository.Tesoreria.ITesoreriaRepository tesoreriaDB2Repository,
            IMapper mapper,
            ILoggerManager logger)
        {
            _cashRegisterShortageRepository = cashRegisterShortageRepository;
            _photoCashRegisterShortageRepository = photoCashRegisterShortageRepository;
            _tesoreriaDB1Repository = tesoreriaDB1Repository;
            _tesoreriaDB2Repository = tesoreriaDB2Repository;
            _mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        /// GET:
        /// Obtiene por ID de Volado de efectivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<CashRegisterShortageDto>>> GetById(int id)
        {
            ApiResponse<CashRegisterShortageDto> response = new ApiResponse<CashRegisterShortageDto>();
            try
            {
                var alarm = _mapper.Map<CashRegisterShortageDto>(_cashRegisterShortageRepository.GetAllIncluding(i => i.PhotoCashRegisterShortages).FirstOrDefault(f => f.Id == id));
                TimeSpan Span = alarm.CreatedDate.Subtract(Convert.ToDateTime(alarm.AlarmTime));
                alarm.ElapsedAlarmTime = Span.ToString(); //DateTime.Parse(Span.Hours + ":" + Span.Minutes + ":" + Span.Seconds).ToString("hh:mm:ss");
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

        [HttpGet("GetCash", Name = "GetCash")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<CashRegisterShortageDto>>> GetCash(int id_sucursal, string dataBase)
        {
            ApiResponse<CashRegisterShortageDto> response = new ApiResponse<CashRegisterShortageDto>();
            try
            {
                DateTime lastCash = _cashRegisterShortageRepository.CashRegisterShortageCreate(id_sucursal);

                switch (dataBase)
                {
                    case "DB1":
                        if (_tesoreriaDB1Repository.GetVolado(id_sucursal, lastCash) >= 3000)
                        {
                            response.Success = true;
                            response.Message = _tesoreriaDB1Repository.GetVolado(id_sucursal, lastCash).ToString();
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = _tesoreriaDB1Repository.GetVolado(id_sucursal, lastCash).ToString();
                        }
                        break;
                    case "DB2":
                        if (_tesoreriaDB2Repository.GetVolado(id_sucursal, lastCash) >= 3000)
                        {
                            response.Success = true;
                            response.Message = _tesoreriaDB2Repository.GetVolado(id_sucursal, lastCash).ToString();
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = _tesoreriaDB2Repository.GetVolado(id_sucursal, lastCash).ToString();
                        }
                        break;
                    default:

                        break;
                }

                //var alarm = _mapper.Map<CashRegisterShortageDto>(_cashRegisterShortageRepository.GetAllIncluding(i => i.PhotoCashRegisterShortages).FirstOrDefault(f => f.Id == id));
                //response.Result = alarm;
               
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
        /// Agregar nuevo registro de Volado de efectivo
        /// </summary>
        /// <param name="cashRegisterShortage">Objeto de Volado de efectivo</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<CashRegisterShortageDto>>> Post([FromBody] CashRegisterShortageDto cashRegisterShortage)
        {
            ApiResponse<CashRegisterShortageDto> response = new ApiResponse<CashRegisterShortageDto>();
            try
            {
                foreach (var item in cashRegisterShortage.PhotoCashRegisterShortages)
                {
                    if (_cashRegisterShortageRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _cashRegisterShortageRepository.UploadImageBase64(item.Photo, "Files/Volados/", item.PhotoPath);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }
                }
                
                response.Result = _mapper.Map<CashRegisterShortageDto>(await _cashRegisterShortageRepository.AddAsyn(_mapper.Map<CashRegisterShortage>(cashRegisterShortage)));
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
        /// Actualizar un Volado de efectivo
        /// </summary>
        /// <param name="cashRegisterShortage">Objeto de Volado de efectivo</param>
        /// <returns></returns>
        [HttpPut]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<CashRegisterShortageDto>>> Put([FromBody] CashRegisterShortageDto cashRegisterShortage)
        {
            ApiResponse<CashRegisterShortageDto> response = new ApiResponse<CashRegisterShortageDto>();
            try
            {
                foreach (var item in cashRegisterShortage.PhotoCashRegisterShortages)
                {
                    if (item.Id == 0 && _photoCashRegisterShortageRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _photoCashRegisterShortageRepository.UploadImageBase64(item.Photo, "Files/Volados/", item.PhotoPath);
                        await _photoCashRegisterShortageRepository.AddAsyn(_mapper.Map<PhotoCashRegisterShortage>(item));
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
                response.Result = _mapper.Map<CashRegisterShortageDto>(await _cashRegisterShortageRepository.UpdateAsync(_mapper.Map<CashRegisterShortage>(cashRegisterShortage), cashRegisterShortage.Id));
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
        public async Task<ActionResult<ApiResponse<PhotoCashRegisterShortageDto>>> Delete(int id)
        {
            var response = new ApiResponse<PhotoCashRegisterShortageDto>();
            try
            {
                if (await _photoCashRegisterShortageRepository.ExistsAsync(id))
                {
                    var @async = await _photoCashRegisterShortageRepository.GetAsync(id);
                    await _photoCashRegisterShortageRepository.DeleteAsyn(@async);
                    response.Success = true;
                    response.Result = _mapper.Map<PhotoCashRegisterShortageDto>(@async);
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
