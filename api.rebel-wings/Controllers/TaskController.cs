using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.RequestTransfer;
using api.rebel_wings.Models.Task;
using api.rebel_wings.Models.Ticket;
using AutoMapper;
using biz.fortia.Repository.RH;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Task;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Task = biz.rebel_wings.Entities.Task;

namespace api.rebel_wings.Controllers;
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskBranchRepository _taskBranchRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly IRHTrabRepository _rhTrabRepository;
    private readonly biz.bd1.Repository.Sucursal.ISucursalRepository _sucursalDB1Repository;
    private readonly biz.bd2.Repository.Sucursal.ISucursalRepository _sucursalDB2Repository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="taskRepository"></param>
    /// <param name="taskBranchRepository"></param>
    /// <param name="rhTrabRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    public TaskController(ITaskRepository taskRepository,
        ITaskBranchRepository taskBranchRepository,
        IRHTrabRepository rhTrabRepository,
        IMapper mapper,
        ILoggerManager logger,
        biz.bd1.Repository.Sucursal.ISucursalRepository sucursalDB1Repository,
        biz.bd2.Repository.Sucursal.ISucursalRepository sucursalDB2Repository)
    {
        _logger = logger;
        _mapper = mapper;
        _taskRepository = taskRepository;
        _taskBranchRepository = taskBranchRepository;
        _rhTrabRepository = rhTrabRepository;
        _sucursalDB1Repository = sucursalDB1Repository;
        _sucursalDB2Repository = sucursalDB2Repository;
    }
    /// <summary>
    /// GET:
    /// Regresa lista de Tareas por Turno
    /// </summary>
    /// <param name="workshift">ID de turno</param>
    /// <returns>Regresa lista de TaskList </returns>
    [HttpGet("Task-By-WorkShift/{workshift}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<List<TaskList>>>> GetMaintenances(int workshift, string dataBase)
    {
        ApiResponse<List<TaskList>> response = new ApiResponse<List<TaskList>>();
        try
        {
            var getDtos = _mapper.Map<List<TaskGetDto>>(_taskRepository
                .GetAllIncluding(t => t.AssignedTo, o => o.Workshift, b => b.TaskBranches)
                .Where(x=>x.WorkshiftId==workshift)
            );

            List<TransferDto> list = new List<TransferDto>();

            switch (dataBase)
            {
                case "DB1":
                    list = _mapper.Map<List<TransferDto>>(_sucursalDB1Repository.GetBranchList());
                    break;
                case "DB2":
                    list = _mapper.Map<List<TransferDto>>(_sucursalDB2Repository.GetBranchList());
                    break;
                default:

                    break;
            }
            //var list = _mapper.Map<List<TransfersListDto>>(_rhTrabRepository.GetBranchList());
            
            List<TaskList> taskLists = new List<TaskList>();
            foreach (var item in getDtos)
            {
                string branchName = "";
                if (item.TaskBranches.Any())
                {
                    if (item.TaskBranches.Count() == 1)
                    {
                        branchName =
                            $"{list.FirstOrDefault(f => f.BranchId == item.TaskBranches.FirstOrDefault().BranchId).Name}";
                    } 
                    else if (item.TaskBranches.Count() == 2)
                    {
                        branchName =
                            $"{list.FirstOrDefault(f => f.BranchId == item.TaskBranches.ElementAtOrDefault(0).BranchId).Name} y " +
                            $"{list.FirstOrDefault(f => f.BranchId == item.TaskBranches.ElementAtOrDefault(1).BranchId).Name}";
                    }
                    else
                    {
                        branchName =
                            $"{list.FirstOrDefault(f => f.BranchId == item.TaskBranches.ElementAtOrDefault(0).BranchId)?.Name}, " +
                            $"{list.FirstOrDefault(f => f.BranchId == item.TaskBranches.ElementAtOrDefault(1).BranchId)?.Name} y " +
                            $"{item.TaskBranches.Count() - 2} mas";
                    }
                }
                taskLists.Add(new TaskList()
                {
                    Id = item.Id,
                    NameString = item.Name,
                    Description = item.Description,
                    AssignedTo = item.AssignedTo.AssignedTo,
                    Icon = item.Icon,
                    BranchName = branchName
                });
            }
            response.Success = true;
            response.Message = "Consult was successfully";
            response.Result = taskLists;
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
    /// Regresa catalogo de 'asinado a'
    /// </summary>
    /// <returns></returns>
    [HttpGet("Catalogue/AssignedTo")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<List<CatAssignedToDto>>>> GetAssignedTo()
    {
        ApiResponse<List<CatAssignedToDto>> response = new ApiResponse<List<CatAssignedToDto>>();
        try
        {
            var assignedTos = _mapper.Map<List<CatAssignedToDto>>(_taskRepository.getAssignedTos());
            response.Success = true;
            response.Message = "Consult was successfully";
            response.Result = assignedTos;
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
    /// Regresa catalogo de 'turno'
    /// </summary>
    /// <returns></returns>
    [HttpGet("Catalogue/WorkShift")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<List<CatWorkShiftDto>>>> GetWorkShift()
    {
        ApiResponse<List<CatWorkShiftDto>> response = new ApiResponse<List<CatWorkShiftDto>>();
        try
        {
            var workShifts = _mapper.Map<List<CatWorkShiftDto>>(_taskRepository.getWorkShifts());
            response.Success = true;
            response.Message = "Consult was successfully";
            response.Result = workShifts;
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
    /// Regresa catalogo de 'tipo de campos'
    /// </summary>
    /// <returns></returns>
    [HttpGet("Catalogue/Type-Fields")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<List<CatTypeFieldDto>>>> GetTypeField()
    {
        ApiResponse<List<CatTypeFieldDto>> response = new ApiResponse<List<CatTypeFieldDto>>();
        try
        {
            var typeFields = _mapper.Map<List<CatTypeFieldDto>>(_taskRepository.getTypeFields());
            response.Success = true;
            response.Message = "Consult was successfully";
            response.Result = typeFields;
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
    /// Agregar una nueva tarea
    /// </summary>
    /// <param name="taskDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<TaskDto>>> Add([FromBody] TaskDto taskDto)
    {
        ApiResponse<TaskDto> response = new ApiResponse<TaskDto>();
        try
        {
            var task = await _taskRepository.AddAsyn(_mapper.Map<Task>(taskDto));
            response.Message = "Record was added successfully.";
            response.Result = _mapper.Map<TaskDto>(task);
            response.Success = true;
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
    /// Actualizar una tarea
    /// </summary>
    /// <param name="taskDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<TaskDto>>> Update([FromBody] TaskDto taskDto)
    {
        ApiResponse<TaskDto> response = new ApiResponse<TaskDto>();
        try
        {
            if (!await _taskRepository.ExistsAsync(f=>f.Id==taskDto.Id))
            {
                response.Success = false;
                response.Message = "Record not exist.";
                return StatusCode(404, response);
            }

            var task = _taskRepository.GetAllIncluding(i => i.TaskBranches).First(f => f.Id == taskDto.Id);
            /// **START**
            /// DELETE ALL BRANCHS AREADY EXIST
            foreach (var item in task.TaskBranches)
            {
                _taskBranchRepository.Delete(_mapper.Map<TaskBranch>(item));
            }
            /// ADD ALL NEW BRANCHS
            foreach (var item in taskDto.TaskBranches)
            {
                await _taskBranchRepository.AddAsyn(_mapper.Map<TaskBranch>(item));
            }
            /// **END**
            var taskUpdated = await _taskRepository.UpdateAsync(_mapper.Map<Task>(taskDto), taskDto.Id);
            response.Message = "Record was updated successfully.";
            response.Result = _mapper.Map<TaskDto>(taskUpdated);
            response.Success = true;

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