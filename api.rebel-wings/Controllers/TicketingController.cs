using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.RequestTransfer;
using api.rebel_wings.Models.Ticketing;
using AutoMapper;
using biz.fortia.Repository.RH;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Ticketing;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controller 
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TicketingController : ControllerBase
{
    private readonly ITicketingRepository _ticketingRepository;
    private readonly IPhotoTicketingRepository _photoTicketingRepository;
    private readonly ICommentTicketingRepository _commentTicketingRepository;
    private readonly ICatTicketingRepository _catTicketingRepository;
    private readonly ICatBranchLocateRepository _catBranchLocateRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IRHTrabRepository _rhTrabRepository;
    private readonly biz.bd1.Repository.Sucursal.ISucursalRepository _sucursalDB1Repository;
    private readonly biz.bd2.Repository.Sucursal.ISucursalRepository _sucursalDB2Repository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ticketingRepository"></param>
    /// <param name="photoTicketingRepository"></param>
    /// <param name="commentTicketingRepository"></param>
    /// <param name="catTicketingRepository"></param>
    /// <param name="catBranchLocateRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="rhTrabRepository"></param>
    public TicketingController(ITicketingRepository ticketingRepository,
        IPhotoTicketingRepository photoTicketingRepository,
        ICommentTicketingRepository commentTicketingRepository,
        ICatTicketingRepository catTicketingRepository,
        ICatBranchLocateRepository catBranchLocateRepository,
        IMapper mapper,
        ILoggerManager loggerManager,
        IRHTrabRepository rhTrabRepository,
        biz.bd1.Repository.Sucursal.ISucursalRepository sucursalDB1Repository,
        biz.bd2.Repository.Sucursal.ISucursalRepository sucursalDB2Repository)
    {
        _ticketingRepository = ticketingRepository;
        _photoTicketingRepository = photoTicketingRepository;
        _commentTicketingRepository = commentTicketingRepository;
        _catTicketingRepository = catTicketingRepository;
        _catBranchLocateRepository = catBranchLocateRepository;
        _mapper = mapper;
        _loggerManager = loggerManager;
        _rhTrabRepository = rhTrabRepository;
        _sucursalDB1Repository = sucursalDB1Repository;
        _sucursalDB2Repository = sucursalDB2Repository;
    }
    /// <summary>
    /// POST:
    /// Add new Ticket
    /// </summary>
    /// <param name="ticketingDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<TicketingDto>>> Post([FromBody] TicketingDto ticketingDto)
    {
        var response = new ApiResponse<TicketingDto>();
        try
        {
            foreach (var item in ticketingDto.PhotoTicketings)
            {
                if (_photoTicketingRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoTicketingRepository.UploadImageBase64(item.Photo, "Files/Ticketing/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }

            var number = await _ticketingRepository.CountByAsync(c => c.BranchId == ticketingDto.BranchId);
            ticketingDto.NoTicket = (number + 1).ToString();
            var order = await _ticketingRepository.AddAsyn(_mapper.Map<Ticketing>(ticketingDto));
            response.Result = _mapper.Map<TicketingDto>(order);
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
    /// Update record
    /// </summary>
    /// <param name="ticketingDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<TicketingDto>>> Put([FromBody] TicketingDto ticketingDto)
    {
        var response = new ApiResponse<TicketingDto>();
        try
        {
            foreach (var item in ticketingDto.PhotoTicketings)
            {
                if (_photoTicketingRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.Photo = _photoTicketingRepository.UploadImageBase64(item.Photo, "Files/Ticketing/", item.PhotoPath);
                    await _photoTicketingRepository.AddAsyn(_mapper.Map<PhotoTicketing>(item));
                }
                else if(item.Id != 0 && item.Photo.Length < 251)
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

            var @async = await _ticketingRepository.UpdateAsync(
                _mapper.Map<Ticketing>(ticketingDto), ticketingDto.Id);
            response.Result = _mapper.Map<TicketingDto>(@async);
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
        return StatusCode(201, response);
    }
    /// <summary>
    /// DELETE:
    /// remove photo
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Photo")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
    {
        var response = new ApiResponse<int>();
        try
        {
            response.Result = await _photoTicketingRepository.DeleteByAsync(d => d.Id == id);
            response.Message = "Removed was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(202, response);
    }
    /// <summary>
    /// POST:
    /// Add new comment 
    /// </summary>
    /// <param name="comment">String</param>
    /// <param name="id">ID => Ticket</param>
    /// <param name="user">ID => from user</param>
    /// <returns></returns>
    [HttpPost("{id}/{user}/Comment")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<CommentTicketingDto>>> AddComment([FromBody] string comment, int id, int user)
    {
        var response = new ApiResponse<CommentTicketingDto>();
        try
        {
            var commentTicketing = await _commentTicketingRepository.AddAsyn(new CommentTicketing()
            {
                Id = 0,
                Comment = comment,
                TicketingId = id,
                CreatedBy = user,
                CreatedDate = DateTime.Now
            });
            response.Result = _mapper.Map<CommentTicketingDto>(commentTicketing);
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
    /// UPDATE:
    /// update status
    /// </summary>
    /// <param name="ticketingStatus"></param>
    /// <param name="id">ID => Ticket</param>
    /// <returns></returns>
    [HttpPut("{id}/Status")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TicketingDto>>> UpdateStatus([FromBody] TicketingStatus ticketingStatus, int id)
    {
        var response = new ApiResponse<TicketingDto>();
        try
        {
            if (await _ticketingRepository.ExistsAsync(e=>e.Id==id))
            {
                var @async = await _ticketingRepository.FindAsync(f => f.Id == id);
                @async.Status = ticketingStatus.Status;
                @async.DateClosed = ticketingStatus.ClosedDate;
                @async.UpdatedBy = ticketingStatus.UserId;
                @async.UpdatedDate=DateTime.Now;
                @async.Cost = ticketingStatus.Cost;
                @async = await _ticketingRepository.UpdateAsync(@async, @async.Id);
                response.Result = _mapper.Map<TicketingDto>(@async);
                response.Message = "Update was success";
                response.Success = true;
            }
            else
            {
                response.Message = "Register does not exist";
                response.Success = false;
                return StatusCode(404, response);
            }
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
    /// GET:
    /// 
    /// </summary>
    /// <param name="id">ID de Ticket</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<TicketingGetDto>> Get(int id)
    {
        var response = new ApiResponse<TicketingGetDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var queryable =
                _ticketingRepository.GetAllIncluding(i => i.PhotoTicketings, c => c.CommentTicketings,
                    p => p.CategoryNavigation, w => w.WhereAreYouLocatedNavigation, u=>u.CreatedByNavigation);
            var @default = queryable.FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<TicketingGetDto>(@default);
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
    /// </summary>
    /// <param name="ids">ID's Sucursales</param>
    /// <returns></returns>
    [HttpGet()]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<TicketingGet>>> GetHistory([FromQuery] int[] ids)
    {
        var response = new ApiResponse<List<TicketingGet>>();
        try
        {
            var list = _mapper.Map<List<TransferDto>>(_sucursalDB1Repository.GetBranchList());
            //var list = _mapper.Map<List<TransfersListDto>>(_rhTrabRepository.GetBranchList());
            
            var queryable =
                _ticketingRepository.GetAllIncluding(u => u.CreatedByNavigation, c => c.CategoryNavigation)
                    .Where(x => !ids.Any() || ids.Contains(x.BranchId))
                    .Select(s => new TicketingGet()
                    {
                        Id = s.Id,
                        Status = s.Status,
                        NoTicketing = s.NoTicket,
                        Closed = s.DateClosed,
                        Opened = s.CreatedDate,
                        Regional = s.CreatedByNavigation.Name,
                        Category = s.CategoryNavigation.Category,
                        BranchId = s.BranchId
                    }).ToList();

            foreach (var ticketingGet in queryable)
            {
                ticketingGet.Branch = list.FirstOrDefault(f => f.BranchId == ticketingGet.BranchId)?.Name;
            }
            response.Result = queryable;
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
    /// Return History by Date and Branches 
    /// </summary>
    /// <param name="ids">ID's Sucursales</param>
    /// <param name="startDate">Date start</param>
    /// <param name="endDate">Date end</param>
    /// <returns></returns>
    [HttpGet("History")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<TicketingGet>>> GetHistories([FromQuery] int[] ids, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var response = new ApiResponse<List<TicketingGet>>();
        try
        {
            //var list = _mapper.Map<List<TransfersListDto>>(_rhTrabRepository.GetBranchList());
            var list = _mapper.Map<List<TransferDto>>(_sucursalDB1Repository.GetBranchList());

            var queryable =
                _ticketingRepository.GetAllIncluding(u => u.CreatedByNavigation, c => c.CategoryNavigation)
                    .Where(x => (!ids.Any() || ids.Contains(x.BranchId)) &&
                                x.CreatedDate >= startDate.AbsoluteStart() && x.CreatedDate <= endDate.AbsoluteEnd())
                    .Select(s => new TicketingGet()
                    {
                        Id = s.Id,
                        Status = s.Status,
                        NoTicketing = s.NoTicket,
                        Closed = s.DateClosed,
                        Opened = s.CreatedDate,
                        Regional = s.CreatedByNavigation.Name,
                        Category = s.CategoryNavigation.Category,
                        BranchId = s.BranchId
                    }).ToList();

            foreach (var ticketingGet in queryable)
            {
                ticketingGet.Branch = list.FirstOrDefault(f => f.BranchId == ticketingGet.BranchId)?.Name;
            }
            response.Result = queryable;
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
    /// Catalogue of Categories
    /// </summary>
    /// <returns></returns>
    [HttpGet("Catalogue/Category")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<CatTicketingDto>>> GetCategories()
    {
        var response = new ApiResponse<List<CatTicketingDto>>();
        try
        {
            var queryable = _catTicketingRepository.GetAll();
            response.Result = _mapper.Map<List<CatTicketingDto>>(queryable);
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
    /// Catalogue of Branch locater
    /// </summary>
    /// <returns></returns>
    [HttpGet("Catalogue/BranchLocate")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<CatBranchLocateDto>>> GetBranchLocate()
    {
        var response = new ApiResponse<List<CatBranchLocateDto>>();
        try
        {
            var queryable = _catBranchLocateRepository.GetAll();
            response.Result = _mapper.Map<List<CatBranchLocateDto>>(queryable);
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
    
}
