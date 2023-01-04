using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Dashboard;
using api.rebel_wings.Models.RequestTransfer;
using AutoMapper;
using biz.fortia.Models;
using biz.fortia.Repository.RH;
using biz.rebel_wings.Models.Transfer;
using biz.rebel_wings.Repository.Dashboard;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controller to Dashboard
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IDashboardRepository _dashboardRepository;
    private readonly IRHTrabRepository _iRHTrabRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dashboardRepository"></param>
    /// <param name="iRhTrabRepository"></param>
    /// <param name="logger"></param>
    /// <param name="mapper"></param>
    public DashboardController(ILoggerManager logger, IMapper mapper, IDashboardRepository dashboardRepository,
        IRHTrabRepository iRhTrabRepository)
    {
        _dashboardRepository = dashboardRepository;
        _iRHTrabRepository = iRhTrabRepository;
        _logger = logger;
        _mapper = mapper;
    }
    /// <summary>
    /// GET:
    /// Return info to admin page
    /// </summary>
    /// <returns></returns>
    [HttpGet("Admin")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<DashboardAdmin>> GetAdmin([FromQuery] DateTime dateTime)
    {
        var response = new ApiResponse<DashboardAdmin>();
        try
        {
            var list = _mapper.Map<List<TransfersListDto>>(_iRHTrabRepository.GetBranchList());

            response.Result = _mapper.Map<DashboardAdmin>(_dashboardRepository.GetAdmin(dateTime.AbsoluteStart(),
                dateTime.AbsoluteEnd(), _mapper.Map<List<TransfersList>>(list)));
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
    /// GET:
    /// Supervisor
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <param name="dateTime">Time frame 1</param>
    /// <param name="dateTime">Time frame 2</param>
    /// <param name="int">If task is done or not</param>
    /// <param name="city">City ID</param>
    /// <returns></returns>
    [HttpGet("{id}/Supervisor")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<DashboardSupervisor>> GetSupervisor(int id, [FromQuery] DateTime timeOne,
        [FromQuery] DateTime timeTwo, [FromQuery] int isDone, [FromQuery] int city)
    {
        var response = new ApiResponse<DashboardSupervisor>();
        try
        {
            response.Result =
                _mapper.Map<DashboardSupervisor>(
                    _dashboardRepository.GetSupervisorsV2(
                        id, timeOne.AbsoluteStart(), timeTwo.AbsoluteEnd(), isDone, city)
                    );
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
    /// GET:
    /// Regional
    /// </summary>
    /// <param name="id">Branch</param>
    /// <param name="dateTime">Time frame 1</param>
    /// <param name="dateTime">Time frame 2</param>
    /// <returns></returns>
    [HttpGet("{id}/Regional")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<DashboardRegional>> GetRegionals(int id, [FromQuery] DateTime timeOne, 
        [FromQuery] DateTime timeTwo, [FromQuery] int isDone, [FromQuery] int city)
    {
        var response = new ApiResponse<DashboardRegional>();
        try
        {
            response.Result = _mapper.Map<DashboardRegional>(_dashboardRepository.GetRegionalV2
                (id, timeOne.AbsoluteStart(), timeTwo.AbsoluteEnd(), isDone, city));
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
    /// GET:
    /// Asistencias
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    [HttpGet("{id}/Assistance")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<DashboardAssistanceV2>> GetAssistance(int id,  [FromQuery] DateTime timeOne, 
        [FromQuery] DateTime timeTwo)
    {
        var response = new ApiResponse<DashboardAssistanceV2>();
        try
        {
            response.Result = _mapper.Map<DashboardAssistanceV2>(_dashboardRepository.GetAssistance(id, timeOne, timeTwo));
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


}