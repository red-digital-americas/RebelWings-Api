using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Fridge;
using api.rebel_wings.Models.Order;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Fridge;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controlador de Refrigeradores
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FridgeController : ControllerBase
{
    private readonly IFridgeRepository _fridgeRepository;
    private readonly IPhotoFridgeRepository _photoFridgeRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    /// <summary>
    /// Contructor
    /// </summary>
    /// <param name="fridgeRepository"></param>
    /// <param name="photoFridgeRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    public FridgeController(IFridgeRepository fridgeRepository,
        IPhotoFridgeRepository photoFridgeRepository,
        IMapper mapper,
        ILoggerManager logger
        )
    {
        _fridgeRepository = fridgeRepository;
        _photoFridgeRepository = photoFridgeRepository;
        _mapper = mapper;
        _logger = logger;
    }
    /// <summary>
    /// GET:
    /// regresa lista de refiregeradores
    /// </summary>
    /// <param name="id">ID ==> Sucursal ID</param>
    /// <param name="user">ID ==> Sucursal ID</param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<FridgeDto>>> Get(int id, int user)
    {
        ApiResponse<List<FridgeDto>> response = new ApiResponse<List<FridgeDto>>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _fridgeRepository.GetAllIncluding(i => i.PhotoFridges)
                .Where(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user).ToList();
            response.Result = _mapper.Map<List<FridgeDto>>(order);
            response.Message = "Consult was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(201, response);
    }
    /// <summary>
    /// GET:
    /// Return  by ID
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<FridgeDto>> GetById(int id)
    {
        var response = new ApiResponse<FridgeDto>();
        try
        {
            var order = _fridgeRepository.GetAllIncluding(i => i.PhotoFridges)
                .FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<FridgeDto>(order);
            response.Message = "Consult was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(201, response);
    }
    
    /// <summary>
    /// POST:
    /// Lista de refrigeradores a agregar
    /// </summary>
    /// <param name="fridgeDtos">Lista de refrigeradores</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<FridgeDto>>>> Post([FromBody] List<FridgeDto> fridgeDtos)
    {
        ApiResponse<List<FridgeDto>> response = new ApiResponse<List<FridgeDto>>();
        try
        {
            var orders = new List<FridgeDto>();
            foreach (var fridgeDto in fridgeDtos)
            {
                foreach (var item in fridgeDto.PhotoFridges)
                {
                    if (_fridgeRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _fridgeRepository.UploadImageBase64(item.Photo, "Files/Fridge/", item.PhotoPath);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }
                }
                var order = await _fridgeRepository.AddAsyn(_mapper.Map<Fridge>(fridgeDto));
                orders.Add(_mapper.Map<FridgeDto>(order));
            }
           
            response.Result = orders;
            response.Message = "Consult was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(201, response);
    }
    /// <summary>
    /// PUT:
    /// Lista de refrigeradores a actualizar
    /// </summary>
    /// <param name="fridgeDtos">Lista de refrigeradores</param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<FridgeDto>>>> Put([FromBody] List<FridgeDto> fridgeDtos)
    {
        var response = new ApiResponse<List<FridgeDto>>();
        try
        {
            List<FridgeDto> dtos = new List<FridgeDto>();
            foreach (var fridgeDto in fridgeDtos)
            {
                if (fridgeDto.Id != 0)
                {
                    foreach (var item in fridgeDto.PhotoFridges)
                    {
                        if (_fridgeRepository.IsBase64(item.Photo) && item.Id == 0)
                        {
                            item.FridgeId = fridgeDto.Id;
                            item.Photo = _fridgeRepository.UploadImageBase64(item.Photo, "Files/Fridge/", item.PhotoPath);
                            await _photoFridgeRepository.AddAsyn(_mapper.Map<PhotoFridge>(item));
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
                
                    var order = await _fridgeRepository.UpdateAsync(_mapper.Map<Fridge>(fridgeDto), fridgeDto.Id);
                    dtos.Add(_mapper.Map<FridgeDto>(order));
                }
                else
                {
                    foreach (var item in fridgeDto.PhotoFridges)
                    {
                        if (_fridgeRepository.IsBase64(item.Photo))
                        {
                            item.Photo = _fridgeRepository.UploadImageBase64(item.Photo, "Files/Fridge/", item.PhotoPath);
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Photo is not Base64";
                            return StatusCode(415, response);
                        }
                    }
                    var order = await _fridgeRepository.AddAsyn(_mapper.Map<Fridge>(fridgeDto));
                    dtos.Add(_mapper.Map<FridgeDto>(order));
                }
            }
            
            
            response.Result = dtos;
            response.Message = "Updated was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(201, response);
    }
    /// <summary>
    /// DELETE:
    /// Remove Photo
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}/Photo")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
    {
        var response = new ApiResponse<int>();
        try
        {
            response.Result = await _photoFridgeRepository.DeleteByAsync(d=>d.Id==id);
            response.Message = "Removed was success";
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            response.Success = false;
            response.Message = ex.ToString();
            return StatusCode(500, response);
        }
        return StatusCode(201, response);
    }
}
