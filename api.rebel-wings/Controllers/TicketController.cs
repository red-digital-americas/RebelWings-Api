using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.RequestTransfer;
using api.rebel_wings.Models.Ticket;
using AutoMapper;
using biz.fortia.Repository.RH;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Ticket;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controlador para Ticket
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IPhotoTicketRepository _photoTicketRepository;
    private readonly IRHTrabRepository _rhTrabRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public TicketController(ITicketRepository ticketRepository,
        IPhotoTicketRepository photoTicketRepository,
        IRHTrabRepository rhTrabRepository,
        IMapper mapper,
        ILoggerManager logger)
    {
        _logger = logger;
        _mapper = mapper;
        _ticketRepository = ticketRepository;
        _photoTicketRepository = photoTicketRepository;
        _rhTrabRepository = rhTrabRepository;
    }
    /// <summary>
    /// GET:
    /// Regresa Ticket por ID
    /// </summary>
    /// <param name="id">ID de Ticket</param>
    /// <returns>Rergresa objeto de Ticket</returns>
    [HttpGet("{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public ActionResult<ApiResponse<TicketGetDto>> GetById(int id)
    {
        ApiResponse<TicketGetDto> response = new ApiResponse<TicketGetDto>();
        try
        {
            var res = _mapper.Map<TicketGetDto>(_ticketRepository
                .GetAllIncluding(i => i.PhotoTickets, t => t.PartBranch, o => o.SpecificSection, s => s.Status)
                .FirstOrDefault(f => f.Id == id));
            response.Success = true;
            response.Message = "Consult was successfully";
            response.Result = res;
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
    /// Agregar un nuevo Ticket
    /// </summary>
    /// <param name="ticketDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<TicketDto>>> Post([FromBody] TicketDto ticketDto)
    {
        ApiResponse<TicketDto> response = new ApiResponse<TicketDto>();
        try 
        {
            foreach (var item in ticketDto.PhotoTickets)
            {
                if (_photoTicketRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoTicketRepository.UploadImageBase64(item.Photo, "Files/Ticket/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
                
            response.Result = _mapper.Map<TicketDto>(await _ticketRepository.AddAsyn(_mapper.Map<Ticket>(ticketDto)));
            response.Success = true;
            response.Message = "Record was added successfully.";
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
    /// Actualiza registro de Ticket
    /// </summary>
    /// <param name="ticketDto">Objeto de Ticket a actualizar</param>
    /// <returns>Regreas objeto actualizado</returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<TicketDto>>> Put([FromBody] TicketDto ticketDto)
    {
        ApiResponse<TicketDto> response = new ApiResponse<TicketDto>();
        try
        {
            foreach (var item in ticketDto.PhotoTickets)
            {
                if (item.Id == 0 && _photoTicketRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoTicketRepository.UploadImageBase64(item.Photo, "Files/Ticket/", item.PhotoPath);
                    await _photoTicketRepository.AddAsyn(_mapper.Map<PhotoTicket>(item));
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
            response.Result = _mapper.Map<TicketDto>(await _ticketRepository.UpdateAsync(_mapper.Map<Ticket>(ticketDto), ticketDto.Id));
            response.Success = true;
            response.Message = "Record was updated successfully.";
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
    /// PUT:
    /// Actualiza estatus de un Ticket
    /// </summary>
    /// <param name="id">ID de Ticket</param>
    /// <param name="status">ID de Estatus</param>
    /// <param name="user">ID de Usuario que realiza la acción</param>
    /// <returns>Regresa objeto actualizado</returns>
    [HttpPut("{id}/{status}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TicketDto>>> PutAdmin(int id, int status, int user)
    {
        ApiResponse<TicketDto> response = new ApiResponse<TicketDto>();
        try
        {
            if (await _ticketRepository.ExistsAsync(e=>e.Id==id))
            {
                var ticketDto = await _ticketRepository.FindAsync(f => f.Id == id);
                ticketDto.StatusId = status;
                ticketDto.UpdatedBy = user;
                ticketDto.UpdatedDate=DateTime.Now;
                response.Result = _mapper.Map<TicketDto>(await _ticketRepository.UpdateAsync(_mapper.Map<Ticket>(ticketDto), ticketDto.Id));
                response.Success = true;
                response.Message = "Record was updated successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Record Not Found";
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
    /// <summary>
    /// GET:
    /// Regresa lista para Mantenimiento en Modulo de Administración
    /// </summary>
    /// <returns>Regresa una lista de Maintenance</returns>
    [HttpGet("Maintenance")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<List<Maintenance>>>> GetMaintenances()
    {
        ApiResponse<List<Maintenance>> response = new ApiResponse<List<Maintenance>>();
        try
        {
            var ticketGetDtos = _mapper.Map<List<TicketGetDto>>(_ticketRepository
                .GetAllIncluding(t => t.PartBranch, o => o.SpecificSection, s => s.Status, u => u.CreatedByNavigation));
            
            var list = _mapper.Map<List<TransfersListDto>>(_rhTrabRepository.GetBranchList());
            
            List<Maintenance> maintenances = new List<Maintenance>();
            foreach (var item in ticketGetDtos)
            {
                maintenances.Add(new Maintenance()
                {
                    Id = item.Id,
                    Status = item.Status.Status,
                    BranchId = item.BranchId,
                    BranchName = list.FirstOrDefault(f=>f.BranchId==item.BranchId).Name,
                    PartBranch = item.PartBranch.PartBranch,
                    SpecificSection = item.SpecificSection.SpecificSection,
                    UserName = item.CreatedByNavigation.Name
                });
            }
            response.Success = true;
            response.Message = "Consult was successfully";
            response.Result = maintenances;
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
    /// GET:
    /// Catalogo de ¿En qué parte de la sucursal te encuentras?
    /// </summary>
    /// <returns>Regresa lista de ¿En qué parte de la sucursal te encuentras?</returns>
    [HttpGet("Catalogue/PartBranch")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<List<CatPartBranchDto>>>> GetPartBranch()
    {
        ApiResponse<List<CatPartBranchDto>> response = new ApiResponse<List<CatPartBranchDto>>();
        try
        {
            var catPartBranchDtos = _mapper.Map<List<CatPartBranchDto>>(_ticketRepository.getPartBranches());
            response.Success = true;
            response.Message = "Consult was successfully";
            response.Result = catPartBranchDtos;
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
    /// GET:
    /// Catalogo de lugar específico
    /// </summary>
    /// <param name="id">ID de que parte de la Sucursal</param>
    /// <returns>Regresa lista de que parte de la Sucursal</returns>
    [HttpGet("Catalogue/SpecificSection/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<List<CatSpecificSectionDto>>>> GetSpecificSection(int id)
    {
        ApiResponse<List<CatSpecificSectionDto>> response = new ApiResponse<List<CatSpecificSectionDto>>();
        try
        {
            var specificSectionDtos = _mapper.Map<List<CatSpecificSectionDto>>(_ticketRepository.getCatSpecificSections());
            response.Success = true;
            response.Message = "Consult was successfully";
            response.Result = specificSectionDtos.Where(x=>x.PartBranchId==id).ToList();
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
    /// GET:
    /// Catalogo de Estatus
    /// </summary>
    /// <returns>Regresa lista de Estatus</returns>
    [HttpGet("Catalogue/Status")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<List<CatStatusTicketDto>>>> GetStatus()
    {
        ApiResponse<List<CatStatusTicketDto>> response = new ApiResponse<List<CatStatusTicketDto>>();
        try
        {
            var statusTicketDtos = _mapper.Map<List<CatStatusTicketDto>>(_ticketRepository.getStatusTickets());
            response.Success = true;
            response.Message = "Consult was successfully";
            response.Result = statusTicketDtos;
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