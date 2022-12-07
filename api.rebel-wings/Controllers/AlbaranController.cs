using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.Albaran;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.RequestTransfer;
using AutoMapper;
using biz.bd1.Repository.Albaran;
using biz.fortia.Repository.RH;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Albaran;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Albaran Controlador
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class AlbaranController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly IAlbaranRepository _albaranRepository;
    private readonly biz.bd2.Repository.Albaran.IAlbaranRepository _albaranBD2Repository;
    private readonly IAlbaranesRepository _albaranesRepository;
    private readonly ICatStatusAlbaranRepository _catStatusAlbaranRepository;
    private readonly IRHTrabRepository _rhTrabRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="albaranRepository"></param>
    /// <param name="albaranesRepository"></param>
    /// <param name="catStatusAlbaranRepository"></param>
    /// <param name="rhTrabRepository"></param>
    public AlbaranController(IMapper mapper,
        ILoggerManager logger,
        IAlbaranRepository albaranRepository,
        IAlbaranesRepository albaranesRepository,
        ICatStatusAlbaranRepository catStatusAlbaranRepository,
        IRHTrabRepository  rhTrabRepository,
        biz.bd2.Repository.Albaran.IAlbaranRepository albaranBD2Repository)
    {
        _mapper = mapper;
        _logger = logger;
        _albaranRepository = albaranRepository;
        _albaranesRepository = albaranesRepository;
        _catStatusAlbaranRepository = catStatusAlbaranRepository;
        _rhTrabRepository = rhTrabRepository;
        _albaranBD2Repository = albaranBD2Repository;
    }
    /// <summary>
    /// GET:
    /// Regresa lista de albaranes
    /// Usar por ahora estos ID s para hacer la consulta 539314253 o 539314252
    /// </summary>
    /// <param name="id">ID de Sucursal</param>
    /// <param name="pageNumber">Número de pagina</param>
    /// <param name="pageSize">Tamaño de pagina</param>
    /// <returns></returns>
    [HttpGet("{id}/{pageNumber}/{pageSize}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<List<AlbaranesDto>>>> Get(int id, int pageNumber, int pageSize)
    {
        ApiResponse<List<AlbaranesDto>> response = new ApiResponse<List<AlbaranesDto>>();
        try
        {
            var list = _mapper.Map<List<TransfersListDto>>(_rhTrabRepository.GetBranchList());

            string? branchName = list.FirstOrDefault(f => f.BranchId == id).Name;
            
            var albaranes1 = _mapper.Map<List<Albaranes>>(_albaranRepository.getAlbaranesList(branchName,pageNumber,pageSize));
            var albranes2 = _mapper.Map<List<Albaranes>>(_albaranBD2Repository.getAlbaranesList(branchName, pageNumber, pageSize));
            var albaranes = albaranes1.Union(albranes2).ToList();
            var albaranesReturn = _mapper.Map<List<AlbaranesDto>>(albaranes);
            var today = DateTime.Now.Date.AddDays(-1);
            var albaran = _albaranesRepository
                .GetAllIncluding(x => x.Status)
                .Where(x => x.CreatedDate > today.Date)
                .ToList();
            // START
            //Sustituir cuando se tenga conexión estable a BD de Rebel Wings 
            // var today = DateTime.Now.Date.AddDays(-1);
            // var albaran = _albaranesRepository
            //     .GetAllIncluding(x=>x.Status)
            //     .Where(x => x.CreatedDate > today.Date)
            //     .ToList();
            //
            // List<AlbaranesDto> albaranesReturn = new List<AlbaranesDto>();
            // if (id == 539314253)
            // {
            //     using (StreamReader r = new StreamReader(Path.GetFullPath("Files/Constituyentes.json")))
            //     {
            //         string json = await r.ReadToEndAsync();
            //         albaranesReturn.AddRange(JsonConvert.DeserializeObject<List<AlbaranesDto>>(json));
            //     }
            // }
            // else
            // {
            //     using (StreamReader r = new StreamReader(Path.GetFullPath("Files/Juriquilla.json")))
            //     {
            //         string json = await r.ReadToEndAsync();
            //         albaranesReturn.AddRange(JsonConvert.DeserializeObject<List<AlbaranesDto>>(json));
            //     }
            // }
            // END
            foreach (var item in albaranesReturn)
            {
                var exist = albaran.FirstOrDefault(f =>
                    f.AlbaranDate.Date == item.AlbaranDate.Date && 
                    f.AlbaranTime == item.AlbaranTime &&
                    f.AlbaranDescription == item.Descripcion &&
                    f.N == item.N &&
                    f.NumAlbaran == item.NumAlbaran &&
                    f.NumSerie == item.NumSerie);
                if (exist != null)
                {
                    item.Id = exist.Id;
                    item.Status = exist.Status.Status;
                }
                else
                {
                    item.Id = 0;
                    item.Status = "Pendiente";
                }
            }
            response.Result = albaranesReturn;
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
    /// <summary>
    /// GET:
    /// Obtener por ID de albaran 
    /// </summary>
    /// <param name="id">ID de albaran a consultar</param>
    /// <returns></returns>
    [HttpGet("By-Id/{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<AlbaranDto>>> GetById(int id)
    {
        ApiResponse<AlbaranDto> response = new ApiResponse<AlbaranDto>();
        try
        {
            response.Result = _mapper.Map<AlbaranDto>(await _albaranesRepository.FindAsync(f => f.Id == id));
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
    /// <summary>
    /// POST:
    /// Agregar un nuevo albaran
    /// </summary>
    /// <param name="albaranDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<AlbaranDto>>> Add([FromBody] AlbaranDto albaranDto)
    {
        ApiResponse<AlbaranDto> response = new ApiResponse<AlbaranDto>();
        try
        {
            response.Result =
                _mapper.Map<AlbaranDto>(await _albaranesRepository.AddAsyn(_mapper.Map<Albaran>(albaranDto)));
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
    /// <summary>
    /// PUT:
    /// Actualizar un albaran
    /// </summary>
    /// <param name="albaranDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<AlbaranDto>>> Update([FromBody] AlbaranDto albaranDto)
    {
        ApiResponse<AlbaranDto> response = new ApiResponse<AlbaranDto>();
        try
        {
            response.Result =
                _mapper.Map<AlbaranDto>(await _albaranesRepository.UpdateAsync(_mapper.Map<Albaran>(albaranDto), albaranDto.Id));
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
    /// <summary>
    /// GET:
    /// Regresa el catalogo para Albaranes
    /// </summary>
    /// <returns></returns>
    [HttpGet("Catalogue/Status")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<List<AlbaranesDto>>>> GetStatus()
    {
        ApiResponse<List<CatStatusAlbaranDto>> response = new ApiResponse<List<CatStatusAlbaranDto>>();
        try
        {

            response.Result = _mapper.Map<List<CatStatusAlbaranDto>>(_catStatusAlbaranRepository.GetAll());
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
    
}