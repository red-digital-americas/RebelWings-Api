using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Dashboard;
using api.rebel_wings.Models.Mermas;
using api.rebel_wings.Models.RequestTransfer;
using AutoMapper;
using biz.bd2.Repository.Stock;
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
    private readonly IStockRepository _stockDB2Repository;
    private readonly biz.bd1.Repository.Stock.IStockRepository _stockDB1Repository;
    private readonly biz.bd1.Repository.Sucursal.ISucursalRepository _sucursalDB1Repository;
    private readonly biz.bd2.Repository.Sucursal.ISucursalRepository _sucursalDB2Repository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dashboardRepository"></param>
    /// <param name="iRhTrabRepository"></param>
    /// <param name="logger"></param>
    /// <param name="mapper"></param>
    public DashboardController(ILoggerManager logger,
        IMapper mapper, 
        IDashboardRepository dashboardRepository,
        IRHTrabRepository iRhTrabRepository,
        IStockRepository stockDB2Repository,
        biz.bd1.Repository.Stock.IStockRepository stockDB1Repository,
        biz.bd1.Repository.Sucursal.ISucursalRepository sucursalDB1Repository,
        biz.bd2.Repository.Sucursal.ISucursalRepository sucursalDB2Repository)
    {
        _dashboardRepository = dashboardRepository;
        _iRHTrabRepository = iRhTrabRepository;
        _logger = logger;
        _mapper = mapper;
        _stockDB2Repository = stockDB2Repository;
        _stockDB1Repository = stockDB1Repository;
        _sucursalDB1Repository = sucursalDB1Repository;
        _sucursalDB2Repository = sucursalDB2Repository;
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
    /// <param name="timeOne">Time frame 1</param>
    /// <param name="timeTwo">Time frame 2</param>
    /// <param name="isDone">is done</param>
    /// <param name="city">City</param>
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

    /// <summary>
    /// GET:
    /// Return Performance by regional
    /// </summary>
    /// <returns></returns>
    [HttpGet("performance-regional/{city:int}/{branch:int}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<MermasDto>>> GetPerformanceRegional(int city, int branch, [FromQuery] DateTime initDate, [FromQuery] DateTime endDate)
    {
        var response = new ApiResponse<List<MermasDto>>();
        try
        {
            switch (city)
            {
                case (1):
                    //DB2
                    response.Result = _mapper.Map<List<MermasDto>>(
                        _stockDB2Repository.GetMermas(branch, initDate.AbsoluteStart(), endDate.AbsoluteEnd()));
                    break;
                case (2):
                    //DB1
                    response.Result = _mapper.Map<List<MermasDto>>(
                        _stockDB1Repository.GetMermas(branch, initDate.AbsoluteStart(), endDate.AbsoluteEnd()));
                    break;
            }

            response.Success = true;
            response.Message = "Operation was success";
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
    /// Method GET:
    /// performance general
    /// </summary>
    /// <param name="city">City</param>
    /// <param name="regional">Regional</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Return a list with performance</returns>
    [HttpGet("performance-general/{city:int}/{regional:int}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<DashboardAdminPerformanceDto>> GetPerformanceGeneral(int city, int regional, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var response = new ApiResponse<DashboardAdminPerformanceDto>();
        try
        {
            var res = _mapper.Map<DashboardAdminPerformanceDto>(_dashboardRepository.GetAdminPerformance(city, regional, startDate, endDate));
            switch (city)
            {
                case 1:
                    foreach (var performance in res.Performances)
                    {
                        if (_sucursalDB2Repository.getSucursalById(performance.IdBranch))
                        {
                            performance.NameBranch = _sucursalDB2Repository
                                .Find(x => x.Idfront == performance.IdBranch).Titulo;
                        }
                        else
                        {
                            performance.NameBranch = "La sucursal no existe";
                        }
                    }
                    break;
                case 2:
                    foreach (var performance in res.Performances)
                    {
                        if (_sucursalDB1Repository.getSucursalById(performance.IdBranch))
                        {
                            performance.NameBranch = _sucursalDB1Repository
                                .Find(x => x.Idfront == performance.IdBranch).Titulo;
                        }
                        else
                        {
                            performance.NameBranch = "La sucursal no existe";
                        }
                    }
                    break;
                default:
                    break;
            }

            response.Result = res;
            response.Message = "Operation was success";
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
    /// Method GET:
    /// performance general supervisor
    /// </summary>
    /// <param name="city">City</param>
    /// <param name="regional">Regional</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Return a list with performance</returns>
    [HttpGet("performance-general-supervisor/{city:int}/{regional:int}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<DashboardAdminPerformanceDto>> GetPerformanceGeneralSupervisor(int city, int regional, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var response = new ApiResponse<DashboardAdminPerformanceDto>();
        try
        {
            var res = _mapper.Map<DashboardAdminPerformanceDto>(_dashboardRepository.GetAdminPerformanceSupervisor(city, regional, startDate, endDate));
            switch (city)
            {
                case 1:
                    foreach (var performance in res.Performances)
                    {
                        if (_sucursalDB2Repository.getSucursalById(performance.IdBranch))
                        {
                            performance.NameBranch = _sucursalDB2Repository
                                .Find(x => x.Idfront == performance.IdBranch).Titulo;
                        }
                        else
                        {
                            performance.NameBranch = "La sucursal no existe";
                        }
                    }
                    break;
                case 2:
                    foreach (var performance in res.Performances)
                    {
                        if (_sucursalDB1Repository.getSucursalById(performance.IdBranch))
                        {
                            performance.NameBranch = _sucursalDB1Repository
                                .Find(x => x.Idfront == performance.IdBranch).Titulo;
                        }
                        else
                        {
                            performance.NameBranch = "La sucursal no existe";
                        }
                    }
                    break;
                default:
                    break;
            }

            response.Result = res;
            response.Message = "Operation was success";
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

}