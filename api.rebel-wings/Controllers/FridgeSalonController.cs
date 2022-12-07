using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.FridgeSalon;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.FridgeSalon;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Fridge Salon Controller 
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
public class FridgeSalonController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IFridgeSalonRepository _fridgeSalonRepository;
    private readonly IPhotoFridgeSalonRepository _photoFridgeSalonRepository;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="loggerManager"></param>
    /// <param name="fridgeSalonRepository"></param>
    /// <param name="photoFridgeSalonRepository"></param>
    public FridgeSalonController(
        IMapper mapper, 
        ILoggerManager loggerManager, 
        IFridgeSalonRepository fridgeSalonRepository,
        IPhotoFridgeSalonRepository photoFridgeSalonRepository)
    {
        _mapper = mapper;
        _loggerManager = loggerManager;
        _fridgeSalonRepository = fridgeSalonRepository;
        _photoFridgeSalonRepository = photoFridgeSalonRepository;
    }
    /// <summary>
    /// GET: Return a Fridge Salon By Branch
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpGet("{id}/{user}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<List<FridgeSalonDto>>> Get(int id, int user)
    {
        var response = new ApiResponse<List<FridgeSalonDto>>();
        try
        {
            var today = DateTime.Now.AddDays(-1);
            today = today.AbsoluteEnd().ToUniversalTime();
            var @default = _fridgeSalonRepository.GetAllIncluding(i => i.PhotoFridgeSalons)
                .Where(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user).ToList();
            response.Result = _mapper.Map<List<FridgeSalonDto>>(@default);
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
        return StatusCode(201, response);
    }
    /// <summary>
    /// GET:
    /// Return By Id
    /// </summary>
    /// <param name="id">PK</param>
    /// <returns></returns>
    [HttpGet("{id}/By-Id")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ApiResponse<FridgeSalonDto>> GetById(int id)
    {
        var response = new ApiResponse<FridgeSalonDto>();
        try
        {
            var @default = _fridgeSalonRepository.GetAllIncluding(i => i.PhotoFridgeSalons)
                .FirstOrDefault(f => f.Id == id);
            response.Result = _mapper.Map<FridgeSalonDto>(@default);
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
        return StatusCode(201, response);
    }
    
    /// <summary>
    /// POST: Add new Fridge Salon
    /// </summary>
    /// <param name="fridgeSalonDto">Object</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<FridgeSalonDto>>>> Post([FromBody] List<FridgeSalonDto> fridgeSalonDtos)
    {
        var response = new ApiResponse<List<FridgeSalonDto>>();
        try
        {
            var dtos = new List<FridgeSalonDto>();
            foreach (var fridgeSalonDto in fridgeSalonDtos)
            {
                foreach (var item in fridgeSalonDto.PhotoFridgeSalons)
                {
                    if (_photoFridgeSalonRepository.IsBase64(item.Photo))
                    {
                        item.FridgeSalonId = fridgeSalonDto.Id;
                        item.Photo = _photoFridgeSalonRepository.UploadImageBase64(item.Photo, "Files/FridgeSalon/", item.PhotoPath);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Photo is not Base64";
                        return StatusCode(415, response);
                    }
                }
                var fridgeSalon = await _fridgeSalonRepository.AddAsyn(_mapper.Map<FridgeSalon>(fridgeSalonDto));
                dtos.Add(_mapper.Map<FridgeSalonDto>(fridgeSalon));
            }
            
            
            
            response.Result = dtos;
            response.Message = "added was success";
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
    /// PUT: Update a Fridge Salon
    /// </summary>
    /// <param name="fridgeSalonDto">Object</param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<List<FridgeSalonDto>>>> Put([FromBody] List<FridgeSalonDto> fridgeSalonDtos)
    {
        var response = new ApiResponse<List<FridgeSalonDto>>();
        try
        {
            var dtos = new List<FridgeSalonDto>();
            foreach (var fridgeSalonDto in fridgeSalonDtos)
            {
                // START
                // Update record
                if (fridgeSalonDto.Id != 0 && await _fridgeSalonRepository.ExistsAsync(e=>e.Id ==fridgeSalonDto.Id))
                {
                    foreach (var item in fridgeSalonDto.PhotoFridgeSalons)
                    {
                        if (_photoFridgeSalonRepository.IsBase64(item.Photo) && item.Id == 0)
                        {
                            item.FridgeSalonId = fridgeSalonDto.Id;
                            item.Photo = _photoFridgeSalonRepository.UploadImageBase64(item.Photo, "Files/FridgeSalon/", item.PhotoPath);
                            await _photoFridgeSalonRepository.AddAsyn(_mapper.Map<PhotoFridgeSalon>(item));
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
            
                    var updateAsync = await _fridgeSalonRepository.UpdateAsync(_mapper.Map<FridgeSalon>(fridgeSalonDto), fridgeSalonDto.Id);
                    dtos.Add(_mapper.Map<FridgeSalonDto>(updateAsync));
                }
                // END Update record
                // Start Add new record
                else
                {
                    foreach (var item in fridgeSalonDto.PhotoFridgeSalons)
                    {
                        if (_photoFridgeSalonRepository.IsBase64(item.Photo) && item.Id == 0)
                        {
                            item.FridgeSalonId = fridgeSalonDto.Id;
                            item.Photo = _photoFridgeSalonRepository.UploadImageBase64(item.Photo, "Files/FridgeSalon/", item.PhotoPath);
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
            
                    var addAsyn = await _fridgeSalonRepository.AddAsyn(_mapper.Map<FridgeSalon>(fridgeSalonDto));
                    dtos.Add(_mapper.Map<FridgeSalonDto>(addAsyn));
                }
                // END Add new Record
            }
            
            response.Result = dtos;
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
    /// GET:
    /// Remove a Photo
    /// </summary>
    /// <param name="id">ID => Photo</param>
    /// <returns></returns>
    [HttpDelete("{id}/Photo")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
    {
        var response = new ApiResponse<int>();
        try
        {
            response.Result = await _photoFridgeSalonRepository.DeleteByAsync(d=>d.Id==id);
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
        return StatusCode(200, response);
    }
    
}
