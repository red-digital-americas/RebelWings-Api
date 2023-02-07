using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Remisiones;
using api.rebel_wings.Models.RequestTransfer;
using AutoMapper;
using biz.bd1.Entities;
using biz.bd1.Repository.Remision;
using biz.fortia.Repository.RH;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controller
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class RemisionController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly IRemisionRepository _remisionBD1Repository;
    private readonly biz.bd2.Repository.Remision.IRemisionRepository _remisionBD2Repository;
    private readonly biz.bd2.Repository.PedidoEntrega.ITEstatusPedidosEntregaRepository _tEstatusPedidosEntregaDB2Repository;
    private readonly biz.bd1.Repository.PedidoEntrega.ITEstatusPedidosEntregaRepository _tEstatusPedidosEntregaDB1Repository;
    private readonly biz.bd2.Repository.PedidoEntrega.ITPedidosEntregaRepository _pedidosEntregaDB2Repository;
    private readonly biz.bd1.Repository.PedidoEntrega.ITPedidosEntregaRepository _pedidosEntregaDB1Repository;
    private readonly biz.bd1.Repository.PedidoEntrega.ITFotosPedidosEntregaRepository _fotosPedidosEntregaDB1Repository;
    private readonly biz.bd2.Repository.PedidoEntrega.ITFotosPedidosEntregaRepository _fotosPedidosEntregaDB2Repository;
    private readonly IRHTrabRepository _rhTrabRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="remisionBd1Repository"></param>
    /// <param name="remisionBd2Repository"></param>
    /// <param name="rhTrabRepository"></param>
    public RemisionController(
        IMapper mapper, 
        ILoggerManager logger, 
        IRemisionRepository remisionBd1Repository, 
        biz.bd2.Repository.Remision.IRemisionRepository remisionBd2Repository,
        biz.bd2.Repository.PedidoEntrega.ITEstatusPedidosEntregaRepository tEstatusPedidosEntregaDB2Repository,
        biz.bd1.Repository.PedidoEntrega.ITEstatusPedidosEntregaRepository tEstatusPedidosEntregaDB1Repository,
        biz.bd2.Repository.PedidoEntrega.ITPedidosEntregaRepository pedidosEntregaDB2Repository,
        biz.bd1.Repository.PedidoEntrega.ITPedidosEntregaRepository pedidosEntregaDB1Repository,
        IRHTrabRepository rhTrabRepository,
        biz.bd1.Repository.PedidoEntrega.ITFotosPedidosEntregaRepository fotosPedidosEntregaDB1Repository,
        biz.bd2.Repository.PedidoEntrega.ITFotosPedidosEntregaRepository fotosPedidosEntregaDB2Repository
        )
    {
        _mapper = mapper;
        _logger = logger;
        _remisionBD1Repository = remisionBd1Repository;
        _remisionBD2Repository = remisionBd2Repository;
        _tEstatusPedidosEntregaDB1Repository = tEstatusPedidosEntregaDB1Repository;
        _tEstatusPedidosEntregaDB2Repository = tEstatusPedidosEntregaDB2Repository;
        _pedidosEntregaDB1Repository = pedidosEntregaDB1Repository;
        _pedidosEntregaDB2Repository = pedidosEntregaDB2Repository;
        _rhTrabRepository = rhTrabRepository;
        _fotosPedidosEntregaDB1Repository = fotosPedidosEntregaDB1Repository;
        _fotosPedidosEntregaDB2Repository = fotosPedidosEntregaDB2Repository;
    }
    [HttpGet("{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public ActionResult<ApiResponse<List<RemisionesGet>>> Get(int id)
    {
        var response = new ApiResponse<List<RemisionesGet>>();
        try
        {
            var list = _mapper.Map<List<TransfersListDto>>(_rhTrabRepository.GetBranchList());
            string branchName = list.FirstOrDefault(f => f.BranchId == id).Name;
            DateTime today = DateTime.Now;
            var remisionesGets1 =
                _mapper.Map<List<RemisionesGet>>(
                    _remisionBD1Repository.GetRemisionesByBranch(branchName, today.AbsoluteStart(),
                        today.AbsoluteEnd()));
            var remisionesGets2 =
                _mapper.Map<List<RemisionesGet>>(
                    _remisionBD2Repository.GetRemisionesByBranch(branchName, today.AbsoluteStart(),
                        today.AbsoluteEnd()));
            var remisionesGets = remisionesGets1.Union(remisionesGets2).ToList();
            response.Result = remisionesGets;
            response.Message = "Operation was success";
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

    [HttpGet("GetPedidosEntregas", Name = "GetPedidosEntregas")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public ActionResult<ApiResponse<List<TPedidoEntregaCalendarDto>>> GetPedidosEntregas(int id_suscursal, string dataBase)
    {
        var response = new ApiResponse<List<TPedidoEntregaCalendarDto>>();
        var colorsPalette = new List<ColorDto>
        {
            new(){ 
                Primary= "rgb(255, 149, 0)",
                Secondary= "rgb(255, 149, 0)",
                Name = "PENDIENTE",
                Id = 1
            },
            new(){
                Primary= "rgb(26,71,42)",
                Secondary= "rgb(26,71,42)",
                Name = "REALIZADO",
                Id = 2
            },
            new(){
                Primary= "rgb(208, 18, 45)",
                Secondary= "rgb(208, 18, 45)",
                Name = "FUERA DE TIEMPO",
                Id = 2
            }
        };
        try
        {
            switch (dataBase)
            {
                case "DB1":
                    var pedidodb1 = _pedidosEntregaDB1Repository
                        .GetAllIncluding(x => x.TFotosPedidosEntregas).Where(x => x.IdSucursal == id_suscursal)
                        .Select(s => new TPedidoEntregaDto()
                        {
                              Id = s.Id,
                              IdProveedor = s.IdProveedor,
                              ProveedorName= _pedidosEntregaDB1Repository.GetProveedor(s.IdProveedor),
                              FechaProg= s.FechaProg,
                              FechaReal= s.FechaReal,
                              Estatus= s.Estatus,
                              IdSucursal= s.IdSucursal,
                              Comentarios= s.Comentarios,
                              EstatusName= s.EstatusNavigation.Estatus,
                              EstatusType= s.EstatusNavigation.Tipo,
                              TFotosPedidosEntregas= s.TFotosPedidosEntregas.Select(f => new TFotosPedidosEntregaDto()
                              {
                                  Id=f.Id,
                                  IdPedido=f.IdPedido,
                                  Foto=f.Foto,
                                  Tipo=f.Tipo
                              }).ToList()

                        }).ToList();

                    #region Calendar

                    var calendarDto = new List<TPedidoEntregaCalendarDto>();
                    foreach (var i in pedidodb1)
                    {
                        calendarDto.Add(new TPedidoEntregaCalendarDto()
                        {
                            Id = i.Id,
                            IdProvedor = i.IdProveedor,
                            Start = i.FechaProg,
                            End = i.FechaProg,
                            title= $"{i.ProveedorName} / {i.EstatusName} / {i.EstatusType}",
                            FechaProg = i.FechaProg,
                            FechaReal = i.FechaReal,
                            Comment = i.Comentarios,
                            EstatusName = i.EstatusName,
                            Estatus = i.Estatus,
                            IdSucursal= i.IdSucursal,
                            Nombre = i.EstatusType,
                            TFotosPedidosEntregas = i.TFotosPedidosEntregas,
                            Color = colorsPalette.First(f=>f.Name.Equals(i.EstatusName)),
                            Draggable = false,
                        });
                    }

                    #endregion
                    response.Result = calendarDto;
                    response.Message = "success";
                    response.Success = true;
                    break;
                case "DB2":
                    var pedidodb2 = _pedidosEntregaDB2Repository
                         .GetAllIncluding(x => x.TFotosPedidosEntregas).Where(x => x.IdSucursal == id_suscursal)
                         .Select(s => new TPedidoEntregaDto()
                         {
                             Id = s.Id,
                             IdProveedor = s.IdProveedor,
                             ProveedorName= _pedidosEntregaDB1Repository.GetProveedor(s.IdProveedor),
                             FechaProg= s.FechaProg,
                             FechaReal= s.FechaReal,
                             Estatus= s.Estatus,
                             IdSucursal= s.IdSucursal,
                             Comentarios= s.Comentarios,
                             EstatusName= s.EstatusNavigation.Estatus,
                             EstatusType = s.EstatusNavigation.Tipo,
                             TFotosPedidosEntregas = s.TFotosPedidosEntregas.Select(f => new TFotosPedidosEntregaDto()
                             {
                                 Id = f.Id,
                                 IdPedido = f.IdPedido,
                                 Foto = f.Foto,
                                 Tipo = f.Tipo
                             }).ToList()

                         }).ToList();
                    
                    #region Calendar

                    var calendarDB2Dto = new List<TPedidoEntregaCalendarDto>();
                    foreach (var i in pedidodb2)
                    {
                        calendarDB2Dto.Add(new TPedidoEntregaCalendarDto()
                        {
                            Id = i.Id,
                            IdProvedor = i.IdProveedor,
                            Start = i.FechaProg,
                            End = i.FechaProg,
                            title= $"{i.ProveedorName} / {i.EstatusName} / {i.EstatusType}",
                            FechaProg = i.FechaProg,
                            FechaReal = i.FechaReal,
                            Comment = i.Comentarios,
                            EstatusName = i.EstatusName,
                            Estatus = i.Estatus,
                            IdSucursal= i.IdSucursal,
                            Nombre = i.EstatusType,
                            TFotosPedidosEntregas = i.TFotosPedidosEntregas,
                            Color = colorsPalette.First(f=>f.Name.Equals(i.EstatusName)),
                            Draggable = false,
                        });

                    }

                    #endregion
                    response.Result = calendarDB2Dto;
                    response.Message = "success";
                    response.Success = true;
                    break;
                default:
                    break;
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

    [HttpGet("GetEstatusPedidosEntregas", Name = "GetEstatusPedidosEntregas")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public ActionResult<ApiResponse<List<TEstatusPedidosEntregaDto>>> GetEstatusPedidosEntregas(int id_suscursal, string dataBase)
    {
        var response = new ApiResponse<List<TEstatusPedidosEntregaDto>>();
        try
        {
            switch (dataBase)
            {
                case "DB1":
                    response.Result = _mapper.Map<List<TEstatusPedidosEntregaDto>>(_tEstatusPedidosEntregaDB1Repository.GetAll());
                    response.Message = "success";
                    response.Success = true;
                    break;
                case "DB2":
                    response.Result = _mapper.Map<List<TEstatusPedidosEntregaDto>>(_tEstatusPedidosEntregaDB2Repository.GetAll());
                    response.Message = "success";
                    response.Success = true;
                    break;
                default:
                    break;
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
    /// Actualiza un registro de Pedido/Entrega
    /// </summary>
    /// <returns>Regresa registro actualizado</returns>
    /// <returns>Retorna un nuevo objeto de Pedido/Entrega</returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<TPedidoEntregaUpadteDto>>> Put([FromBody] TPedidoEntregaUpadteDto dto, [FromQuery] string dataBase)
    {
        var response = new Models.ApiResponse.ApiResponse<TPedidoEntregaUpadteDto>();
        try
        {
            switch (dataBase)
            {
                case "DB1":
                    foreach (var item in dto.TFotosPedidosEntregas)
                    {
                        if (_fotosPedidosEntregaDB1Repository.IsBase64(item.Foto) && item.Id == 0)
                        {
                            item.Foto = _fotosPedidosEntregaDB1Repository.UploadImageBase64(item.Foto, "Files/Alarmas/", item.Tipo);
                            await _fotosPedidosEntregaDB1Repository.AddAsyn(_mapper.Map<TFotosPedidosEntrega>(item));
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Photo is not Base64";
                            return StatusCode(415, response);
                        }
                    }
                    response.Result = _mapper.Map<TPedidoEntregaUpadteDto>(await _pedidosEntregaDB1Repository.UpdateAsync(_mapper.Map<biz.bd1.Entities.TPedidosEntrega>(dto), dto.Id));
                    response.Success = true;
                    response.Message = "Record was updated successfully.";
                    break;
                case "DB2":
                    foreach (var item in dto.TFotosPedidosEntregas)
                    {
                        if (_fotosPedidosEntregaDB2Repository.IsBase64(item.Foto) && item.Id == 0)
                        {
                            item.Foto = _fotosPedidosEntregaDB2Repository.UploadImageBase64(item.Foto, "Files/Alarmas/", item.Tipo);
                            await _fotosPedidosEntregaDB2Repository.AddAsyn(_mapper.Map<biz.bd2.Entities.TFotosPedidosEntrega>(item));
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Photo is not Base64";
                            return StatusCode(415, response);
                        }
                    }
                    response.Result = _mapper.Map<TPedidoEntregaUpadteDto>(await _pedidosEntregaDB2Repository.UpdateAsync(_mapper.Map<biz.bd2.Entities.TPedidosEntrega>(dto), dto.Id));
                    response.Success = true;
                    response.Message = "Record was updated successfully.";
                    break;
                default:
                    break;
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
    /// Agregar un nuevo estatus para entregas/pedidos
    /// </summary>
    /// <param name="Create Estatus Pedido/Entrega"></param>
    /// <returns>Retorna un nuevo objeto de Pedido/Entrega</returns>
    /// <response code="201">Regresa el nuevo objeto creado</response>
    /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>
    /// <response code="500">Error Interno</response>
    [HttpPost("create_status_db1", Name = "create_status_db1")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<TEstatusPedidosEntregaDto>>> create_status_db1([FromBody] TEstatusPedidosEntregaDto dto)
    {
        Models.ApiResponse.ApiResponse<TEstatusPedidosEntregaDto> response = new Models.ApiResponse.ApiResponse<TEstatusPedidosEntregaDto>();
        try
        {
            TEstatusPedidosEntrega _estatus = _tEstatusPedidosEntregaDB1Repository.Add(_mapper.Map<TEstatusPedidosEntrega>(dto));
            response.Result = _mapper.Map<TEstatusPedidosEntregaDto>(_estatus);
            response.Success = true;
            response.Message = "Record was added succesfully.";
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

    [HttpPost("create_status_db2", Name = "create_status_db2")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Models.ApiResponse.ApiResponse<TEstatusPedidosEntregaDto>>> create_status_db2([FromBody] TEstatusPedidosEntregaDto dto)
    {
        Models.ApiResponse.ApiResponse<TEstatusPedidosEntregaDto> response = new Models.ApiResponse.ApiResponse<TEstatusPedidosEntregaDto>();
        try
        {
            biz.bd2.Entities.TEstatusPedidosEntrega _estatus = _tEstatusPedidosEntregaDB2Repository.Add(_mapper.Map<biz.bd2.Entities.TEstatusPedidosEntrega>(dto));
            response.Result = _mapper.Map<TEstatusPedidosEntregaDto>(_estatus);
            response.Success = true;
            response.Message = "Record was added succesfully.";
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

    [HttpPost("create_pedido_entrega", Name = "create_pedido_entrega")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<TPedidoEntregaDto>>> Post([FromBody] TPedidoEntregaDto pedido, [FromQuery] string dataBase)
    {
        var response = new ApiResponse<TPedidoEntregaDto>();
        try
        {
            var insertData = new TPedidoEntregaDto();
            /*
             * Identify where insert data if dataBase indicate which database insert record 
             */
            switch (dataBase)
            {
                case "DB1":
                    foreach (var item in pedido.TFotosPedidosEntregas)
                    {
                        if (_pedidosEntregaDB1Repository.IsBase64(item.Foto))
                        {
                            item.Foto = _pedidosEntregaDB1Repository.UploadImageBase64(item.Foto, "Files/Alarmas/", item.Tipo);
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Photo is not Base64";
                            return StatusCode(415, response);
                        }
                    }
                    insertData = _mapper.Map<TPedidoEntregaDto>(await _pedidosEntregaDB1Repository.AddAsyn(_mapper.Map<TPedidosEntrega>(pedido)));
                    break;
                case "DB2":
                    foreach (var item in pedido.TFotosPedidosEntregas)
                    {
                        if (_pedidosEntregaDB2Repository.IsBase64(item.Foto))
                        {
                            item.Foto = _pedidosEntregaDB2Repository.UploadImageBase64(item.Foto, "Files/Alarmas/", item.Tipo);
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Photo is not Base64";
                            return StatusCode(415, response);
                        }
                    }
                    insertData = _mapper.Map<TPedidoEntregaDto>(await _pedidosEntregaDB2Repository.AddAsyn(_mapper.Map<biz.bd2.Entities.TPedidosEntrega>(pedido)));
                    break;
            }


            response.Result = insertData;
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
}