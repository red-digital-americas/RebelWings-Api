using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.PrecookedChicken;
using AutoMapper;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.PrecookedChicken;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controlador de Pollo Precocido
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PrecookedChickenController : ControllerBase
{
   private readonly ILoggerManager _loggerManager;
   private readonly IMapper _mapper;
   private readonly IPrecookedChickenRepository _precookedChickenRepository;
   private readonly IPhotoPrecookedChickenRepository _photoPrecookedChickenRepository;

   public PrecookedChickenController(ILoggerManager loggerManager, 
      IMapper mapper, 
      IPrecookedChickenRepository precookedChickenRepository,
      IPhotoPrecookedChickenRepository photoPrecookedChickenRepository)
   {
      _loggerManager = loggerManager;
      _mapper = mapper;
      _precookedChickenRepository = precookedChickenRepository;
      _photoPrecookedChickenRepository = photoPrecookedChickenRepository;
   }
   /// <summary>
   /// GET:
   /// Regresa objeto de Pollo precosido
   /// </summary>
   /// <param name="id">ID de Sucursal</param>
   /// <param name="user">ID de Sucursal</param>
   /// <returns></returns>
   [HttpGet("{id}/{user}")]
   [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public ActionResult<ApiResponse<PrecookedChickenDto>> Get(int id, int user)
   {
      var response = new ApiResponse<PrecookedChickenDto>();
      try
      {
         var today = DateTime.Now.AddDays(-1);
         today = today.AbsoluteEnd().ToUniversalTime();
         var order = _precookedChickenRepository.GetAllIncluding(i => i.PhotoPrecookedChickens)
            .FirstOrDefault(f => f.BranchId == id && f.CreatedDate > today && f.CreatedBy == user);
         response.Result = _mapper.Map<PrecookedChickenDto>(order);
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
   /// Return By ID
   /// </summary>
   /// <param name="id"></param>
   /// <returns></returns>
   [HttpGet("{id}/By-Id")]
   [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public ActionResult<ApiResponse<PrecookedChickenDto>> GetById(int id)
   {
       var response = new ApiResponse<PrecookedChickenDto>();
       try
       {
           var order = _precookedChickenRepository.GetAllIncluding(i => i.PhotoPrecookedChickens)
               .FirstOrDefault(f => f.Id == id);
           response.Result = _mapper.Map<PrecookedChickenDto>(order);
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
   /// POST:
   /// Add new Precooked Chicken
   /// </summary>
   /// <param name="precookedChickenDto"></param>
   /// <returns></returns>
   [HttpPost]
   [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
   public async Task<ActionResult<ApiResponse<PrecookedChickenDto>>> Post([FromBody] PrecookedChickenDto precookedChickenDto)
   {
       var response = new ApiResponse<PrecookedChickenDto>();
       try
       {
           foreach (var item in precookedChickenDto.PhotoPrecookedChickens)
           {
               if (_precookedChickenRepository.IsBase64(item.Photo))
               {
                   item.Photo = _precookedChickenRepository.UploadImageBase64(item.Photo, "Files/PrecookedChicken/", item.PhotoPath);
               }
               else
               {
                   response.Success = false;
                   response.Message = "Photo is not Base64";
                   return StatusCode(415, response);
               }
           }
           var precooked = await _precookedChickenRepository.AddAsyn(_mapper.Map<PrecookedChicken>(precookedChickenDto));
           response.Result = _mapper.Map<PrecookedChickenDto>(precooked);
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
   /// PUT:
   /// Update Precooked Chicken
   /// </summary>
   /// <param name="precookedChickenDto"></param>
   /// <returns></returns>
   [HttpPut]
   [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
   public async Task<ActionResult<ApiResponse<PrecookedChickenDto>>> Put([FromBody] PrecookedChickenDto precookedChickenDto)
   {
       var response = new ApiResponse<PrecookedChickenDto>();
       try
       {
           
           foreach (var item in precookedChickenDto.PhotoPrecookedChickens)
           {
               if (_precookedChickenRepository.IsBase64(item.Photo) && item.Id == 0)
               {
                   item.PrecookedChickenId = precookedChickenDto.Id;
                   item.Photo = _precookedChickenRepository.UploadImageBase64(item.Photo, "Files/PrecookedChicken/", item.PhotoPath);
                   await _photoPrecookedChickenRepository.AddAsyn(_mapper.Map<PhotoPrecookedChicken>(item));
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

           var precookedChicken =
               await _precookedChickenRepository.UpdateAsync(_mapper.Map<PrecookedChicken>(precookedChickenDto),
                   precookedChickenDto.Id);

           response.Result = _mapper.Map<PrecookedChickenDto>(precookedChicken);
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
           response.Result = await _photoPrecookedChickenRepository.DeleteByAsync(d=>d.Id==id);
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
       return StatusCode(201, response);
   }
   
}
