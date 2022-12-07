using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.Articulos;
using AutoMapper;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.rebel_wings.Controllers
{
    /// <summary>
    /// Controlador para Articulos de tienda
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ItemsController : ControllerBase
    {
        private readonly biz.bd1.Repository.Articulos.IArticulosRespository _articulosRespositoryBD1;
        private readonly biz.bd2.Repository.Articulos.IArticulosRepository _articulosRespositoryBD2; 
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="articulosRespositoryBD1"></param>
        /// <param name="articulosRespositoryBD2"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public ItemsController(biz.bd1.Repository.Articulos.IArticulosRespository articulosRespositoryBD1,
            biz.bd2.Repository.Articulos.IArticulosRepository articulosRespositoryBD2,
            IMapper mapper,
            ILoggerManager logger)
        {
            _articulosRespositoryBD1 = articulosRespositoryBD1;
            _articulosRespositoryBD2 = articulosRespositoryBD2;
            _mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        ///  LISTA de Articulos
        /// </summary>
        /// <param name="id">ID de Sucursal</param>
        /// <param name="word">Palabra CLAVE para la busqueda</param>
        /// <returns></returns>
        [HttpGet("{id}/{word}")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<List<ItemsDto>>>> Get(int id, string word)
        {
            ApiResponse<List<List<ItemsDto>>> response = new ApiResponse<List<List<ItemsDto>>>();
            try
            {
                word = word.ToUpper();
                List<List<ItemsDto>> articulos = new List<List<ItemsDto>>();
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
                
                var customerQueryList = word.ToCharArray();
                string init = "";
                for (int i = 0; i < customerQueryList.Length; i++)
                {
                    init = $"{init}{customerQueryList[i].ToString()}";
                    Console.WriteLine($"WORD : {init}");
                    // var res = _articulosRespositoryBD1.GetAll()
                    //     .Where(x => x.Descripcion.StartsWith("*") && x.Descripcion.Contains(init) && x.Descatalogado.Equals("F"))
                    //     .Select(s => new ItemsDto()
                    //     {
                    //         Codarticulo = s.Codarticulo,
                    //         Descripcion = s.Descripcion
                    //     }).ToList();
                    var res = repository
                        .Where(x => x.Descripcion.Contains(init))// !x.Descripcion.StartsWith("*")  cuando la conexión sea establezca cambiear por esta linea
                        .Select(s => new ItemsDto()
                        {
                            Codarticulo = s.Codarticulo,
                            Descripcion = s.Descripcion
                        }).ToList();
                    articulos.Add(res);
                }
                // articulos.Add(repository);
                response.Result = articulos;
                response.Success = true;
                response.Message = "Consult was success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.Success = false;
                response.Message = ex.ToString();
                return StatusCode(500, response);
            }
            return StatusCode(200, response);
        }
    }
}
