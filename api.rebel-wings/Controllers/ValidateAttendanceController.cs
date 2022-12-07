using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.ValidateAttendance;
using AutoMapper;
using biz.fortia.Repository.RH;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.ValidateAttendance;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace api.rebel_wings.Controllers
{
    /// <summary>
    /// Controlador para Validación de Asistencia
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateAttendanceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IValidateAttendanceRepository _validateAttendanceRepository;
        private readonly IRHTrabRepository _iRHTrabRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="loggerManager"></param>
        /// <param name="validateAttendanceRepository"></param>
        /// <param name="rHTrabRepository"></param>
        public ValidateAttendanceController(IMapper mapper, ILoggerManager loggerManager,
            IValidateAttendanceRepository validateAttendanceRepository, IRHTrabRepository rHTrabRepository)
        {
            _mapper = mapper;
            _logger = loggerManager;
            _validateAttendanceRepository = validateAttendanceRepository;
            _iRHTrabRepository = rHTrabRepository;
        }
        /// <summary>
        /// Obtener por ID de validación de asistencia
        /// </summary>
        /// <param name="id">ID de Validación de asistencia en caso de que exista uno</param>
        /// <returns>Regresa objeto de validación de asistencia</returns>
        [HttpGet("{id}")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<ValidateAttendanceDto>>> GetById(int id)
        {
            ApiResponse<ValidateAttendanceDto> response = new ApiResponse<ValidateAttendanceDto>();
            try
            {
                response.Result = _mapper.Map<ValidateAttendanceDto>(_validateAttendanceRepository.GetAllIncluding(i => i.AttendenceNavigation).First(f => f.Id == id));
                response.Success = true;
                response.Message = "Consult was success.";
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
        /// Retorna todos los trabajadores de una sucursal
        /// </summary>
        /// <param name="id">ID de sucursal</param>
        /// <returns></returns>
        [HttpGet("All/{id}", Name ="All")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<List<ValidateAttendanceList>>>> GetAll(int id)
        {
            ApiResponse<List<ValidateAttendanceList>> response = new ApiResponse<List<ValidateAttendanceList>>();
            try
            {
                // List<ValidateAttendanceList> items = new List<ValidateAttendanceList>();
                // using (StreamReader r = new StreamReader(Path.GetFullPath("Files/json.json")))
                // {
                //     string json = r.ReadToEnd();
                //     items.AddRange(JsonConvert.DeserializeObject<List<ValidateAttendanceList>>(json));
                // }
                var list = _iRHTrabRepository.GetAttendanceLists(id);
                List<ValidateAttendanceList> ValidateAttendanceList = new List<ValidateAttendanceList>();
                foreach (var listItem in list)
                {
                    string output = "";
                    ValidateAttendanceList.Add(new ValidateAttendanceList()
                    {
                        JobTitle = listItem.JobTitle,
                        Name = ReplaceWhitespace(listItem.FullName, output),
                        AttendanceId = _validateAttendanceRepository.Exists(f=>f.ClabTrab == listItem.Id && f.CreatedDate == DateTime.Now) 
                            ? _validateAttendanceRepository.Find(f => f.ClabTrab == listItem.Id && f.CreatedDate == DateTime.Now).Id : 0,
                        Avatar = "",
                        ValidateAttendance = "",
                        TimeDelay = 0,
                        Workshift = listItem.Workshift.ToString(),
                        ClabTrab = listItem.Id
                    });
                }
                response.Result = ValidateAttendanceList;
                // Random rnd = new Random();
                // foreach (var item in items)
                // {
                //     item.AttendanceId = _validateAttendanceRepository.GetAll().Any(f => f.ClabTrab == item.ClabTrab && f.CreatedDate.Date == DateTime.Now.Date)
                //         ? _validateAttendanceRepository.GetAll().OrderByDescending(o=>o.Id).First(f => f.ClabTrab == item.ClabTrab && f.CreatedDate.Date == DateTime.Now.Date).Id : 0;
                //     item.Status = rnd.Next(1, 4);
                // }
                // response.Result = items;
                response.Success = true;
                response.Message = "Consulta was success.";
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
        /// Agregar un nuevo registro de VAlidación de asistencia
        /// </summary>
        /// <param name="validateAttendanceDto">Objeto de validadción de asistencia</param>
        /// <returns></returns>
        /// <response code="201">Regresa el nuevo objeto creado</response>
        /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>        
        /// <response code="415">Si la foto no cumple con los requisitos para subirla</response>
        [HttpPost]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<ValidateAttendanceDto>>> Post([FromBody] ValidateAttendanceDto validateAttendanceDto)
        {
            ApiResponse<ValidateAttendanceDto> response = new ApiResponse<ValidateAttendanceDto>();
            try
            {
                response.Result = _mapper.Map<ValidateAttendanceDto>(await _validateAttendanceRepository.AddAsyn(_mapper.Map<ValidateAttendance>(validateAttendanceDto)));
                response.Success = true;
                response.Message = "Record was added success.";
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

        [HttpPut]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<ApiResponse<ValidateAttendanceDto>>> Put([FromBody] ValidateAttendanceDto validateAttendanceDto)
        {
            ApiResponse<ValidateAttendanceDto> response = new ApiResponse<ValidateAttendanceDto>();
            try
            {
                response.Result = _mapper.Map<ValidateAttendanceDto>(await _validateAttendanceRepository.UpdateAsync(_mapper.Map<ValidateAttendance>(validateAttendanceDto), validateAttendanceDto.Id));
                response.Success = true;
                response.Message = "Record was updated success.";
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


        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            input = regex.Replace(input, " ");
            return sWhitespace.Replace(input, replacement);
        }


    }
}
