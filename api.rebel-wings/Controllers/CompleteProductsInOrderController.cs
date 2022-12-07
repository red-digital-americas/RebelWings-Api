using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.CompleteProductsInOrder;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.CompleteProductsInOrder;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CompleteProductsInOrderController : ControllerBase
{
    private readonly ICompleteProductsInOrderRepository _completeProductsInOrderRepository;
    private readonly IPhotoCompleteProductsInOrderRepository _photoCompleteProductsInOrderRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="completeProductsInOrderRepository"></param>
    /// <param name="photoCompleteProductsInOrderRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    public CompleteProductsInOrderController(ICompleteProductsInOrderRepository completeProductsInOrderRepository,
        IPhotoCompleteProductsInOrderRepository photoCompleteProductsInOrderRepository,
        IMapper mapper,
        ILoggerManager logger
    )
    {
        _completeProductsInOrderRepository = completeProductsInOrderRepository;
        _photoCompleteProductsInOrderRepository = photoCompleteProductsInOrderRepository;
        _mapper = mapper;
        _logger = logger;
    }
    /// <summary>
    /// GET:
    /// Return a Complete product In Order By ID 
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<CompleteProductsInOrderDto>> GetById(int id)
    {
        var response = new ApiResponse<CompleteProductsInOrderDto>();
        try
        {
            var @default = _completeProductsInOrderRepository.GetAllIncluding(i => i.PhotoCompleteProductsInOrders)
                .FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<CompleteProductsInOrderDto>(@default);
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
    /// Return By Branch
    /// </summary>
    /// <param name="id">Branch ID</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<CompleteProductsInOrderDto>>> Get(int id)
    {
        var response = new ApiResponse<List<CompleteProductsInOrderDto>>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd();
            var order = _completeProductsInOrderRepository.GetAllIncluding(i => i.PhotoCompleteProductsInOrders)
                .Where(f => f.BranchId == id && f.CreatedDate > today).ToList();
            response.Result = _mapper.Map<List<CompleteProductsInOrderDto>>(order);
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
    /// Add new Complete products in Order
    /// </summary>
    /// <param name="completeProductsInOrderDtos">New Object</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<CompleteProductsInOrderDto>>>> Post([FromBody] List<CompleteProductsInOrderDto> completeProductsInOrderDtos)
    {
        var response = new ApiResponse<List<CompleteProductsInOrderDto>>();
        try
        {
            var orders = new List<CompleteProductsInOrderDto>();
            foreach (var completeProductsInOrderDto in completeProductsInOrderDtos)
            {
                foreach (var item in completeProductsInOrderDto.PhotoCompleteProductsInOrders)
                {
                    if (_completeProductsInOrderRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _completeProductsInOrderRepository.UploadImageBase64(item.Photo, "Files/Complete/", item.PhotoPath);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }
                }
                var order = await _completeProductsInOrderRepository.AddAsyn(_mapper.Map<CompleteProductsInOrder>(completeProductsInOrderDto));
                orders.Add(_mapper.Map<CompleteProductsInOrderDto>(order));
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
    /// Update a Complete Products In Order
    /// </summary>
    /// <param name="completeProductsInOrderDtos">Object</param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<CompleteProductsInOrderDto>>>> Put([FromBody] List<CompleteProductsInOrderDto> completeProductsInOrderDtos)
    {
        var response = new ApiResponse<List<CompleteProductsInOrderDto>>();
        try
        {
            var dtos = new List<CompleteProductsInOrderDto>();
            foreach (var completeProductsInOrderDto in completeProductsInOrderDtos)
            {
                if (completeProductsInOrderDto.Id != 0)
                {
                    foreach (var item in completeProductsInOrderDto.PhotoCompleteProductsInOrders)
                    {
                        if (_completeProductsInOrderRepository.IsBase64(item.Photo) && item.Id == 0)
                        {
                            item.Photo = _completeProductsInOrderRepository.UploadImageBase64(item.Photo, "Files/Complete/", item.PhotoPath);
                            await _photoCompleteProductsInOrderRepository.AddAsyn(
                                _mapper.Map<PhotoCompleteProductsInOrder>(item));
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
                
                    var order = await _completeProductsInOrderRepository.UpdateAsync(_mapper.Map<CompleteProductsInOrder>(completeProductsInOrderDto), completeProductsInOrderDto.Id);
                    dtos.Add(_mapper.Map<CompleteProductsInOrderDto>(order));
                }
                else
                {
                    foreach (var item in completeProductsInOrderDto.PhotoCompleteProductsInOrders)
                    {
                        if (_completeProductsInOrderRepository.IsBase64(item.Photo))
                        {
                            item.Photo = _completeProductsInOrderRepository.UploadImageBase64(item.Photo, "Files/Complete/", item.PhotoPath);
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Photo is not Base64";
                            return StatusCode(415, response);
                        }
                    }
                    var order = await _completeProductsInOrderRepository.AddAsyn(_mapper.Map<CompleteProductsInOrder>(completeProductsInOrderDto));
                    dtos.Add(_mapper.Map<CompleteProductsInOrderDto>(order));
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
            response.Result = await _photoCompleteProductsInOrderRepository.DeleteByAsync(d=>d.Id==id);
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