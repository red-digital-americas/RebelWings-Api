using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Articulos;
using api.rebel_wings.Models.RequestTransfer;
using AutoMapper;
using biz.bd1.Repository.Articulos;
using biz.bd2.Repository.Articulos;
using biz.fortia.Repository.RH;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.RequestTransfer;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controlador de Transferencias
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class TransferController : ControllerBase
{
    private readonly IRequestTransferRepository _requestTransferRepository;
    private readonly ICatAmountRepository _catAmountRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly IRHTrabRepository _iRHTrabRepository;
    private readonly IArticulosRepository _articulosRespositoryBD2;
    private readonly IArticulosRespository _articulosRespositoryBD1;
    private readonly biz.bd1.Repository.Sucursal.ISucursalRepository _sucursalDB1Repository;
    private readonly biz.bd2.Repository.Sucursal.ISucursalRepository _sucursalDB2Repository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="requestTransferRepository"></param>
    /// <param name="catAmountRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="rhTrabRepository"></param>
    /// <param name="articulosRespositoryBd2"></param>
    /// <param name="articulosRespositoryBd1"></param>
    public TransferController(IRequestTransferRepository requestTransferRepository,
        ICatAmountRepository catAmountRepository,
        IMapper mapper,
        ILoggerManager loggerManager,
        IRHTrabRepository rhTrabRepository,
        IArticulosRepository articulosRespositoryBd2,
        IArticulosRespository articulosRespositoryBd1,
        biz.bd1.Repository.Sucursal.ISucursalRepository sucursalDB1Repository,
        biz.bd2.Repository.Sucursal.ISucursalRepository sucursalDB2Repository)
    {
        _logger = loggerManager;
        _mapper = mapper;
        _requestTransferRepository = requestTransferRepository;
        _catAmountRepository = catAmountRepository;
        _iRHTrabRepository = rhTrabRepository;
        _articulosRespositoryBD1 = articulosRespositoryBd1;
        _articulosRespositoryBD2 = articulosRespositoryBd2;
        _sucursalDB1Repository = sucursalDB1Repository;
        _sucursalDB2Repository = sucursalDB2Repository;
    }

    /// <summary>
    /// GET:
    /// Retorna lista de Sucursales
    /// </summary>
    /// <param name="id">ID de sucursal</param>
    /// <returns></returns>
    /// <response code="200">Regresa el objeto</response>
    /// <response code="404">Si el objeto no existe</response>        
    /// <response code="500">Error interno de servidor</response> 
    [HttpGet("BranchList/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<List<RequestTransferListNewDto>>>> GetByBranch(int id, string dataBase)
    {
        ApiResponse<List<RequestTransferListNewDto>> response = new ApiResponse<List<RequestTransferListNewDto>>();
        try
        {
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

            var repository = _mapper.Map<List<ItemsDto>>(_articulosRespositoryBD1.GetAll()
                .Where(x => !x.Descripcion.StartsWith("*") && x.Descatalogado.Equals("F")).Select(s => new ItemsDto()
                {
                    Codarticulo = s.Codarticulo,
                    Descripcion = s.Descripcion
                }).ToList());
            repository.Union(_mapper.Map<List<ItemsDto>>(_articulosRespositoryBD2.GetAll()
                .Where(x => !x.Descripcion.StartsWith("*") && x.Descatalogado.Equals("F")).Select(s => new ItemsDto()
                {
                    Codarticulo = s.Codarticulo,
                    Descripcion = s.Descripcion
                }).ToList())).ToList();
            List<RequestTransferListNewDto> newDtos = new List<RequestTransferListNewDto>();
            foreach (var item in list)
            {
                newDtos.Add(new RequestTransferListNewDto()
                {
                    Description = item.Description,
                    Name = item.Name,
                    BranchId = item.BranchId,
                    RequestId = new List<TranseferRequestDto>(),
                    TranferId = new List<TranseferRequestDto>()
                });
            }
            foreach (var item in newDtos)
            {
                var i = await _requestTransferRepository.GetAll()
                    .Where(x => x.Type == 1 && x.ToBranchId == id && x.FromBranchId == item.BranchId)
                    .Select(s => new TranseferRequestDto()
                    {
                        Id = s.Id,
                        productId = s.ProductId,
                        Name = s.Amount
                    }).ToListAsync();
                var ia = await _requestTransferRepository.GetAll()
                    .Where(x => x.Type == 1 && x.ToBranchId == item.BranchId && x.FromBranchId == id)
                    .Select(s => new TranseferRequestDto()
                    {
                        Id = s.Id,
                        productId = s.ProductId,
                        Name = s.Amount
                    }).ToListAsync();
                var typeOne = i.Union(ia).ToList();
                foreach (var transeferRequestDto in typeOne)
                {
                    string? desc = repository.FirstOrDefault(f => f.Codarticulo == transeferRequestDto.productId)?.Descripcion;
                    transeferRequestDto.Name = $"{desc}, {transeferRequestDto.Name}";
                }
                if(typeOne.Any()) item.RequestId.AddRange(typeOne);
                var a = await _requestTransferRepository.GetAll()
                    .Where(f => f.Type == 2 && f.ToBranchId == id && f.FromBranchId == item.BranchId )
                    .Select(s => new TranseferRequestDto()
                    {
                        Id = s.Id,
                        productId = s.ProductId,
                        Name = s.Amount
                    }).ToListAsync();
                var ai = await _requestTransferRepository.GetAll()
                    .Where(f => f.Type == 2 && f.ToBranchId == item.BranchId && f.FromBranchId == id)
                    .Select(s => new TranseferRequestDto()
                    {
                        Id = s.Id,
                        productId = s.ProductId,
                        Name = s.Amount
                    }).ToListAsync();
                var typeTwo = a.Union(ai).ToList();
                foreach (var transeferRequestDto in typeTwo)
                {
                    string? desc = repository.FirstOrDefault(f => f.Codarticulo == transeferRequestDto.productId)?.Descripcion;
                    transeferRequestDto.Name = $"{desc}, {transeferRequestDto.Name}";
                }
                if(typeTwo.Any()) item.TranferId.AddRange(typeTwo);
            }

            response.Result = newDtos;
            response.Success = true;
            response.Message = "Consult was success";
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
    /// Retorna la lista de Cantidades 
    /// </summary>
    /// <param name="word">Palabra de busqueda</param>
    /// <returns></returns>
    /// <response code="200">Regresa lista de resultados similares</response>
    /// <response code="404">No existe coincidencias</response>        
    /// <response code="500">Error interno de servidor</response> 
    [HttpGet("Catalogue/Amount")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<string>>> GetAmount(string word)
    {
        ApiResponse<List<string>> response = new ApiResponse<List<string>>();
        try
        {
            var amounts = _requestTransferRepository.GetAll().Where(x => x.Amount.Contains(word)).Select(s => s.Amount)
                .GroupBy(g => g).Select(s=>s.Key).ToList();
            response.Result = amounts;
            response.Success = true;
            response.Message = "Consult was success";
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
    /// Regresa lista de Status
    /// </summary>
    /// <returns>Regresa lista de estatus</returns>
    /// <response code="200">Regresa el nuevo objeto creado</response>
    /// <response code="404">No existe</response>        
    /// <response code="500">Error interno de servidor</response> 
    [HttpGet("Catalogue/Status")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<CatStatusRequestTransferDto>>> GetAmount()
    {
        ApiResponse<List<CatStatusRequestTransferDto>> response = new ApiResponse<List<CatStatusRequestTransferDto>>();
        try
        {
            var status = _mapper.Map<List<CatStatusRequestTransferDto>>(_requestTransferRepository.GetStatus());
            response.Result = status;
            response.Success = true;
            response.Message = "Consult was success";
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
    /// Retorna objeto de Hacer Transferencias/Solicitud
    /// </summary>
    /// <remarks>
    /// Example:
    /// {
    ///     "id": 1,
    ///     "type": 1,
    ///     "fromBranchId": 1,
    ///     "toBranchId": 539314232,
    ///     "date": "2021-11-09T22:49:03.803Z",
    ///     "time": "23:59:59",
    ///     "productId": 2574,
    ///     "code": "CODE-101010",
    ///     "amount": 1,
    ///     "comment": "string",
    ///     "createdBy": 1,
    ///     "createdDate":
    ///     "2021-11-09T22:49:03.803Z",
    ///     "updatedBy": null,
    ///     "updatedDate": null
    /// }
    /// </remarks>
    /// <param name="id">ID de Transferencias/Solicitud</param>
    /// <returns></returns>
    /// <response code="200">Regresa el nuevo objeto creado</response>
    /// <response code="404">No existe</response>        
    /// <response code="500">Error interno de servidor</response> 
    [HttpGet("{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<RequestTransferConsultDto>> GetById(int id)
    {
        ApiResponse<RequestTransferConsultDto> response = new ApiResponse<RequestTransferConsultDto>();
        try
        {
            if (_requestTransferRepository.Exists(id))
            {
                var itemsDtos = _mapper.Map<List<ItemsDto>>(_articulosRespositoryBD1.GetAll()
                    .Where(x => !x.Descripcion.StartsWith("*") && x.Descatalogado.Equals("F")).Select(s => new ItemsDto()
                    {
                        Codarticulo = s.Codarticulo,
                        Descripcion = s.Descripcion
                    }).ToList());
                itemsDtos.Union(_mapper.Map<List<ItemsDto>>(_articulosRespositoryBD2.GetAll()
                    .Where(x => !x.Descripcion.StartsWith("*") && x.Descatalogado.Equals("F")).Select(s => new ItemsDto()
                    {
                        Codarticulo = s.Codarticulo,
                        Descripcion = s.Descripcion
                    }).ToList())).ToList();

                var consult = _mapper.Map<RequestTransferConsultDto>(_requestTransferRepository.Find(f => f.Id == id));
                consult.Product = itemsDtos.First(f => f.Codarticulo == consult.ProductId).Descripcion;
                response.Result = consult;
                response.Success = true;
                response.Message = "Consult was success";
            }
            else
            {
                response.Success = false;
                response.Message = "Record not exist";
                return StatusCode(404, response);
            }
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
    /// GET: regresa la lista de notificaciones 
    /// </summary>
    /// <param name="id">ID de sucursal</param>
    /// <returns></returns>
    /// <response code="200">Regresa el nuevo objeto creado</response>
    /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>        
    /// <response code="500">Error interno de servidor</response>   
    [HttpGet("Notifications")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<NotificationList>>> GetNotifications(int id)
    {
        ApiResponse<List<NotificationList>> response = new ApiResponse<List<NotificationList>>();
        try
        {
            if (_requestTransferRepository.GetAll().Any(a=> a.Status != 3 && (a.FromBranchId == id || a.ToBranchId == id)))
            {
                var itemsDtos = _mapper.Map<List<ItemsDto>>(_articulosRespositoryBD1.GetAll()
                    .Where(x => !x.Descripcion.StartsWith("*") && x.Descatalogado.Equals("F")).Select(s => new ItemsDto()
                    {
                        Codarticulo = s.Codarticulo,
                        Descripcion = s.Descripcion
                    }).ToList());
                itemsDtos.Union(_mapper.Map<List<ItemsDto>>(_articulosRespositoryBD2.GetAll()
                    .Where(x => !x.Descripcion.StartsWith("*") && x.Descatalogado.Equals("F")).Select(s => new ItemsDto()
                    {
                        Codarticulo = s.Codarticulo,
                        Descripcion = s.Descripcion
                    }).ToList())).ToList();
                var branchList = _mapper.Map<List<TransfersListDto>>(_iRHTrabRepository.GetBranchList());
                
                var transeferRequestDtos = _requestTransferRepository.GetAllIncluding(i=>i.StatusNavigation)
                    .Where(x => x.Status != 3 && (x.FromBranchId == id || x.ToBranchId == id))
                    .Select(s => new NotificationList()
                    {
                        Id = s.Id,
                        Branch = GetBranch(branchList, s.FromBranchId, s.ToBranchId, s.Type),
                        Name = GetProductName(itemsDtos, s.ProductId),
                        Type = s.Type == 1 ? "Transferido" : "Solicitado",
                        Status = s.StatusNavigation.Status
                    }).ToList();
                response.Result = transeferRequestDtos;
                response.Success = true;
                response.Message = "Consult was success";
            }
            else
            {
                response.Success = false;
                response.Message = "Record not exist";
                return StatusCode(404, response);
            }
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

    private static string GetBranch(List<TransfersListDto> transfersListDtos, int from, int to, int type)
    {
        return transfersListDtos.FirstOrDefault(f => type == 1 ? from == f.BranchId : f.BranchId == to)?.Description;
    }
    private static string GetProductName(List<ItemsDto> itemsDtos, int productId)
    {
        return itemsDtos.FirstOrDefault(f => f.Codarticulo == productId)?.Descripcion;
    }
    /// <summary>
    /// POST:
    /// Agregar un nuevo HACER UNA TRANSFERENCIA/SOLICITAR UNA TRANSFERENCIA
    /// </summary>
    /// <param name="requestTransferDto">Objeto para agregar</param>
    /// <returns></returns>
    /// <response code="200">Regresa el nuevo objeto creado</response>
    /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>        
    /// <response code="500">Error interno de servidor</response>   
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<RequestTransferDto>>> Post([FromBody] RequestTransferDto requestTransferDto)
    {
        ApiResponse<RequestTransferDto> response = new ApiResponse<RequestTransferDto>();
        try
        {
            // requestTransferDto.Time = DateTime.Now.TimeOfDay;
            response.Result =
                _mapper.Map<RequestTransferDto>( await 
                    _requestTransferRepository.AddAsyn(_mapper.Map<RequestTransfer>(requestTransferDto)));
            response.Success = true;
            response.Message = "Record was added success";
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
    /// PUT:
    /// Actualizar objeto de SOLICITAR UNA TRANSFERENCIA/SOLICITAR UNA TRANSFERENCIA
    /// </summary>
    /// <param name="requestTransferDto"></param>
    /// <returns></returns>
    /// <response code="200">Regresa el nuevo objeto creado</response>
    /// <response code="404">No se encuentro este registro</response>        
    /// <response code="500">Error interno de servidor</response>   
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<RequestTransferDto>>> Put([FromBody] RequestTransferDto requestTransferDto)
    {
        ApiResponse<RequestTransferDto> response = new ApiResponse<RequestTransferDto>();
        try
        {
            if (await _requestTransferRepository.ExistsAsync(f=>f.Id==requestTransferDto.Id))
            {
                var find = await _requestTransferRepository.FindAsync(f => f.Id == requestTransferDto.Id);
                if (find.Status != requestTransferDto.Status && requestTransferDto.Status == 2)
                {
                    requestTransferDto.Type = 1;
                }
                
                response.Result =
                    _mapper.Map<RequestTransferDto>( await 
                        _requestTransferRepository.UpdateAsync(_mapper.Map<RequestTransfer>(requestTransferDto), requestTransferDto.Id));
                response.Success = true;
                response.Message = "Record was updated success";
            }
            else
            {
                response.Success = false;
                response.Message = "Record not exist";
                return StatusCode(404, response);
            }
            
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