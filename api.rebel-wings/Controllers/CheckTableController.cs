using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.CheckTable;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.CheckTable;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controller 
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CheckTableController : ControllerBase
{
    private readonly ICheckTableRepository _checkTableRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="checkTableRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    public CheckTableController(
        ICheckTableRepository checkTableRepository,
        IMapper mapper,
        ILoggerManager loggerManager)
    {
        _checkTableRepository = checkTableRepository;
        _mapper = mapper;
        _loggerManager = loggerManager;
    }
    /// <summary>
    /// GET:
    /// Return a Revisión de Mesas
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<CheckTableDto>>> Get(int id, int user)
    {
        var response = new ApiResponse<CheckTableDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = await _checkTableRepository.GetAllAsyn();
            var @default = order.FirstOrDefault(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
            response.Result = _mapper.Map<CheckTableDto>(@default);
            response.Message = "Consult was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(200, response);
    }
    /// <summary>
    /// GET:
    /// Obtener por ID de Entradas cargadas
    /// </summary>
    /// <param name="id">ID de Entradas cargadas</param>
    /// <returns></returns>
    [HttpGet("By-Id/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<CheckTableDto>>> GetById(int id)
    {
        var response = new ApiResponse<CheckTableDto>();
        try
        {
            var @default = _checkTableRepository.Find(f => f.Id == id);
            response.Result = _mapper.Map<CheckTableDto>(@default);
            response.Message = "Operation was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.Message;
            return StatusCode(500, response);
        }
        return StatusCode(200, response);
    }
    /// <summary>
    /// POST:
    /// Add new Revisión de Mesas
    /// </summary>
    /// <param name="checkTableDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<CheckTableDto>>> Post([FromBody] CheckTableDto checkTableDto)
    {
        var response = new ApiResponse<CheckTableDto>();
        try
        {
            var checkTable = await _checkTableRepository.AddAsyn(_mapper.Map<CheckTable>(checkTableDto));
            response.Result = _mapper.Map<CheckTableDto>(checkTable);
            response.Message = "Add was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(201, response);
    }
    /// <summary>
    /// PUT:
    /// Update a Revisión de Mesas
    /// </summary>
    /// <param name="checkTableDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<CheckTableDto>>> Put([FromBody] CheckTableDto checkTableDto)
    {
        var response = new ApiResponse<CheckTableDto>();
        try
        {
            var update = await _checkTableRepository.UpdateAsync(_mapper.Map<CheckTable>(checkTableDto),
                checkTableDto.Id);
            response.Result = _mapper.Map<CheckTableDto>(update);
            response.Message = "Updated was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(200, response);
    }


}
