using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.OrderScheduleReview;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.OrderScheduleReview;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrderScheduleReviewController : ControllerBase
{
    private readonly IOrderScheduleReviewRepository _orderScheduleReviewRepository;
    private readonly IPhotoOrderScheduleReviewRepository _photoOrderScheduleReviewRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    public OrderScheduleReviewController(
        IOrderScheduleReviewRepository orderScheduleReviewRepository,
        IPhotoOrderScheduleReviewRepository photoOrderScheduleReviewRepository,
        IMapper mapper,
        ILoggerManager loggerManager
    )
    {
        _orderScheduleReviewRepository = orderScheduleReviewRepository;
        _photoOrderScheduleReviewRepository = photoOrderScheduleReviewRepository;
        _mapper = mapper;
        _loggerManager = loggerManager;
    }
    /// <summary>
    /// GET:
    /// Return a Revisión de Pedido vs calendario
    /// </summary>
    /// <param name="id">Sucursal ID</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<OrderScheduleReviewDto>> Get(int id)
    {
        var response = new ApiResponse<OrderScheduleReviewDto>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd();
            var queryable =
                _orderScheduleReviewRepository.GetAllIncluding(i => i.PhotoOrderScheduleReviews);
            var @default = queryable.FirstOrDefault(f => f.BranchId == id && f.CreatedDate > today);
            response.Result = _mapper.Map<OrderScheduleReviewDto>(@default);
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
    /// Return By Id
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<OrderScheduleReviewDto>> GetById(int id)
    {
        var response = new ApiResponse<OrderScheduleReviewDto>();
        try
        {
            var queryable =
                _orderScheduleReviewRepository.GetAllIncluding(i => i.PhotoOrderScheduleReviews);
            var @default = queryable.FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<OrderScheduleReviewDto>(@default);
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
    /// POST:
    /// Add mew Revisión de Pedido vs calendario
    /// </summary>
    /// <param name="orderScheduleReviewDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<OrderScheduleReviewDto>>> Post([FromBody] OrderScheduleReviewDto orderScheduleReviewDto)
    {
        var response = new ApiResponse<OrderScheduleReviewDto>();
        try
        {
            foreach (var item in orderScheduleReviewDto.PhotoOrderScheduleReviews)
            {
                if (_photoOrderScheduleReviewRepository.IsBase64(item.Photo))
                {
                    item.Photo = _photoOrderScheduleReviewRepository.UploadImageBase64(item.Photo, "Files/OrderScheduleReview/", item.PhotoPath);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Photo is not Base64";
                    return StatusCode(415, response);
                }
            }
            var order = await _orderScheduleReviewRepository.AddAsyn(_mapper.Map<OrderScheduleReview>(orderScheduleReviewDto));
            response.Result = _mapper.Map<OrderScheduleReviewDto>(order);
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
    /// Update a Revisión de Pedido vs calendario
    /// </summary>
    /// <param name="orderScheduleReviewDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<OrderScheduleReviewDto>>> Put([FromBody] OrderScheduleReviewDto orderScheduleReviewDto)
    {
        var response = new ApiResponse<OrderScheduleReviewDto>();
        try
        {
            foreach (var item in orderScheduleReviewDto.PhotoOrderScheduleReviews)
            {
                if (_photoOrderScheduleReviewRepository.IsBase64(item.Photo) && item.Id == 0)
                {
                    item.Photo = _photoOrderScheduleReviewRepository.UploadImageBase64(item.Photo, "Files/OrderScheduleReview/", item.PhotoPath);
                    await _photoOrderScheduleReviewRepository.AddAsyn(_mapper.Map<PhotoOrderScheduleReview>(item));
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

            var @async = await _orderScheduleReviewRepository.UpdateAsync(
                _mapper.Map<OrderScheduleReview>(orderScheduleReviewDto), orderScheduleReviewDto.Id);
            response.Result = _mapper.Map<OrderScheduleReviewDto>(@async);
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
    /// Remove a PHOTO Revisión de Pedido vs calendario
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
            response.Result = await _photoOrderScheduleReviewRepository.DeleteByAsync(d => d.Id == id);
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
    
}