using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Articulos;
using api.rebel_wings.Models.RiskProduct;
using AutoMapper;
using biz.bd1.Repository.Articulos;
using biz.bd2.Repository.Articulos;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.RiskProduct;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.rebel_wings.Controllers;
/// <summary>
/// Controlador de Producto en Riesgo
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RiskProductController : ControllerBase
{
    private readonly IRiskProductRepository _riskProductRepository;
    private readonly IArticulosRepository _articulosRespositoryBD2;
    private readonly IArticulosRespository _articulosRespositoryBD1;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="riskProductRepository"></param>
    /// <param name="articulosRespositoryBd2"></param>
    /// <param name="articulosRespositoryBd1"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    public RiskProductController(IRiskProductRepository riskProductRepository,
        IArticulosRepository articulosRespositoryBd2,
        IArticulosRespository articulosRespositoryBd1,
        IMapper mapper,
        ILoggerManager logger)
    {
        _riskProductRepository = riskProductRepository;
        _articulosRespositoryBD2 = articulosRespositoryBd2;
        _articulosRespositoryBD1 = articulosRespositoryBd1;
        _mapper = mapper;
        _logger = logger;
    }
    
    /// <summary>
    /// GET:
    /// Obtiene por ID de sucursal de los Productos en Riesgo
    /// </summary>
    /// <param name="id">Sucursal</param>
    /// <returns></returns>
    [HttpGet("{id}/Branch")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<List<RiskProductGetDto>>>> GetByBranchId(int id)
    {
        var response = new ApiResponse<List<RiskProductGetDto>>();
        try
        {
            #region Catalogue

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
            // List<ItemsDto> repository = new List<ItemsDto>();
            // using (StreamReader r = new StreamReader(Path.GetFullPath("Files/Items.json")))
            // {
            //     string json = r.ReadToEnd();
            //     repository.AddRange(JsonConvert.DeserializeObject<List<ItemsDto>>(json));
            // }

            #endregion

            var date = DateTime.Now;
            var alarm = _mapper.Map<List<RiskProductGetDto>>(await _riskProductRepository.GetAllAsyn());
            alarm = alarm.Where(f => f.BranchId == id && f.CreatedDate >= date.AbsoluteStart() && f.CreatedDate <= date.AbsoluteEnd()).ToList();
            foreach (var item in alarm)
            {
                item.Product = repository.FirstOrDefault(f => f.Codarticulo == item.ProductId)?.Descripcion;
            }
            
            response.Result = alarm;
            response.Success = true;
            response.Message = "Consult was success";
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
    /// Obtiene por ID de Producto en Riesgo
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ApiResponse<RiskProductGetDto>>> GetById(int id)
    {
        ApiResponse<RiskProductGetDto> response = new ApiResponse<RiskProductGetDto>();
        try
        {
            #region Catalogue

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

            #endregion
            var alarm = _mapper.Map<RiskProductGetDto>(await _riskProductRepository.GetAll().FirstOrDefaultAsync(f=>f.Id == id));
            alarm.Product = repository.FirstOrDefault(f => f.Codarticulo == alarm.ProductId)?.Descripcion;
            response.Result = alarm;
            response.Success = true;
            response.Message = "Consult was success";
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
    /// Agregar nuevo registro Producto en Riesgo
    /// </summary>
    /// <param name="riskProduct">Objeto de Producto en Riesgo</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<RiskProductDto>>>> Post([FromBody] List<RiskProductDto> riskProduct)
    {
        ApiResponse<List<RiskProductDto>> response = new ApiResponse<List<RiskProductDto>>();
        try
        {
            List<RiskProductDto> list = new List<RiskProductDto>();
            foreach (var item in riskProduct)
            {
                list.Add(_mapper.Map<RiskProductDto>(await _riskProductRepository.AddAsyn(_mapper.Map<RiskProduct>(item))));
            }
            response.Result = list;
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
    /// <summary>
    /// PUT:
    /// Actualizar Producto en Riesgo
    /// </summary>
    /// <param name="productDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<ActionResult<ApiResponse<List<RiskProductDto>>>> Put([FromBody] List<RiskProductDto> productDto)
    {
        var response = new ApiResponse<List<RiskProductDto>>();
        try
        {
            var riskProducts = new List<RiskProductDto>();
            foreach (var item in productDto)
            {
                if (item.Id != 0)
                {
                    riskProducts.Add(_mapper.Map<RiskProductDto>(await _riskProductRepository.UpdateAsync(_mapper.Map<RiskProduct>(item), item.Id)));
                }
                else if (item.Id == 0)
                {
                    riskProducts.Add(
                        _mapper.Map<RiskProductDto>(
                            await _riskProductRepository.AddAsyn(_mapper.Map<RiskProduct>(item))));
                }
            }
            response.Result = riskProducts;
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