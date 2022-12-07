using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Inventario;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Inventario;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;



namespace api.rebel_wings.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class InventarioController : ControllerBase
{
  private readonly IInventarioRepository _inventarioRepository;
  private readonly IMapper _mapper;
  private readonly ILoggerManager _loggerManager;
  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="inventarioRepository"></param>
  /// <param name="mapper"></param>
  /// <param name="loggerManager"></param>
  public InventarioController(
      IInventarioRepository inventarioRepository,
      IMapper mapper,
      ILoggerManager loggerManager)
  {
    _inventarioRepository = inventarioRepository;
    _mapper = mapper;
    _loggerManager = loggerManager;
  }
  /// <summary>
  /// POST:
  /// Add new Revisión de Mesas
  /// </summary>
  /// <param name="inventarioDto"></param>
  /// <returns></returns>
  [HttpPost]
  [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
  public async Task<ActionResult<ApiResponse<InventarioDto>>> Post([FromBody] InventarioDto inventarioDto)
  {
    var response = new ApiResponse<InventarioDto>();
    try
    {
      var checkTable = await _inventarioRepository.AddAsyn(_mapper.Map<Inventario>(inventarioDto));
      response.Result = _mapper.Map<InventarioDto>(checkTable);
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
}
