using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Order;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.Order;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPhotoOrderRepository _photopOrderRepository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public OrderController(IOrderRepository orderRepository, IMapper mapper, ILoggerManager logger, IPhotoOrderRepository photoOrderRepository)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
        _photopOrderRepository = photoOrderRepository;
    }
    /// <summary>
    /// GET:
    /// Return a list of Orders
    /// </summary>
    /// <param name="id">ID ==> Sucursal ID</param>
    /// <param name="user">ID ==> Sucursal ID</param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<OrderDto>>> Get(int id, int user)
    {
        ApiResponse<List<OrderDto>> response = new ApiResponse<List<OrderDto>>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var order = _orderRepository.GetAllIncluding(i => i.PhotoOrders)
                .Where(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user ).ToList();
            response.Result = _mapper.Map<List<OrderDto>>(order);
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
    
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<OrderDto>> GetById(int id)
    {
        var response = new ApiResponse<OrderDto>();
        try
        {
            var order = _orderRepository.GetAllIncluding(i => i.PhotoOrders)
                .FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<OrderDto>(order);
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
    /// Agrega lista de ordenes 
    /// </summary>
    /// <param name="orderDto">Lista de Ordenes a agregar</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<OrderDto>>>> Post([FromBody] List<OrderDto> orderDtos)
    {
        ApiResponse<List<OrderDto>> response = new ApiResponse<List<OrderDto>>();
        try
        {
            var orders = new List<OrderDto>();
            foreach (var orderDto in orderDtos)
            {
                foreach (var item in orderDto.PhotoOrders)
                {
                    if (_orderRepository.IsBase64(item.Photo))
                    {
                        item.Photo = _orderRepository.UploadImageBase64(item.Photo, "Files/Orders/", item.PhotoPath);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }
                }
                var order = await _orderRepository.AddAsyn(_mapper.Map<Order>(orderDto));
                orders.Add(_mapper.Map<OrderDto>(order));
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
    /// Update or Add new Order
    /// </summary>
    /// <param name="orderDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<OrderDto>>>> Put([FromBody] List<OrderDto> orderDtos)
    {
        ApiResponse<List<OrderDto>> response = new ApiResponse<List<OrderDto>>();
        try
        {
            List<OrderDto> dtos = new List<OrderDto>();
            foreach (var orderDto in orderDtos)
            {
                if (orderDto.Id == 0)
                {
                    foreach (var item in orderDto.PhotoOrders)
                    {
                        if (_orderRepository.IsBase64(item.Photo) && item.Id == 0)
                        {
                            item.Photo = _orderRepository.UploadImageBase64(item.Photo, "Files/Orders/", item.PhotoPath);
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
                    var newOrder = await _orderRepository.AddAsyn(_mapper.Map<Order>(orderDto));
                    dtos.Add(_mapper.Map<OrderDto>(newOrder));
                }
                else if(orderDto.Id != 0)
                {
                    var order = await _orderRepository.UpdateAsync(_mapper.Map<Order>(orderDto), orderDto.Id);
                    dtos.Add(_mapper.Map<OrderDto>(order));
                    foreach (var item in orderDto.PhotoOrders)
                    {
                        if (_orderRepository.IsBase64(item.Photo) && item.Id == 0)
                        {
                            item.OrderId = order.Id;
                            item.Photo = _orderRepository.UploadImageBase64(item.Photo, "Files/Orders/", item.PhotoPath);
                            await _photopOrderRepository.AddAsyn(_mapper.Map<PhotoOrder>(item));
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
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var response = new ApiResponse<bool>();
        try
        {
            response.Result = await _orderRepository.RemovePhoto(id);
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
    
}
