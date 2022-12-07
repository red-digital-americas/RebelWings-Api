using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.RequestTransfer;
using api.rebel_wings.Models.SalesExpectations;
using api.rebel_wings.Models.Stock;
using AutoMapper;
using biz.fortia.Repository.RH;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Repository.CatStatusSalesExpectations;
using biz.rebel_wings.Repository.SalesExpectations;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.rebel_wings.Controllers
{
    /// <summary>
    /// Controlados para Expectativas de Ventas
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class StockChickenController : ControllerBase
    {
        private readonly ICatStatusStockChickenRepository _catStatusSalesExpectationRepository;
        private readonly IStockChickenRepository _salesExpectationRepository;
        private readonly IStockChickeUsedRepository _stockChickeUsedRepository;
        private readonly IStockChickenByBranchRepository _stockChickenByBranchRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IRHTrabRepository _iRHTrabRepository;
        private readonly biz.bd1.Repository.Sucursal.ISucursalRepository _sucursalDB1Repository;
        private readonly biz.bd2.Repository.Sucursal.ISucursalRepository _sucursalDB2Repository;
        private readonly biz.bd1.Repository.Stock.IStockRepository _stockDB1Repository;
        private readonly biz.bd2.Repository.Stock.IStockRepository _stockDB2Repository;
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="catStatusSalesExpectationRepository"></param>
        /// <param name="salesExpectationRepository"></param>
        /// <param name="stockChickeUsedRepository"></param>
        /// <param name="stockChickenByBranchRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        /// <param name="iRhTrabRepository"></param>
        public StockChickenController(
            ICatStatusStockChickenRepository catStatusSalesExpectationRepository,
            IStockChickenRepository salesExpectationRepository,
            IMapper mapper,
            ILoggerManager logger,
            IStockChickeUsedRepository stockChickeUsedRepository,
            IStockChickenByBranchRepository stockChickenByBranchRepository,
            IRHTrabRepository iRhTrabRepository,
            biz.bd1.Repository.Sucursal.ISucursalRepository sucursalDB1Repository,
            biz.bd2.Repository.Sucursal.ISucursalRepository sucursalDB2Repository,
            biz.bd1.Repository.Stock.IStockRepository stockDB1Repository,
            biz.bd2.Repository.Stock.IStockRepository stockDB2Repository)
        {
            _logger = logger;
            _mapper = mapper;
            _catStatusSalesExpectationRepository = catStatusSalesExpectationRepository;
            _salesExpectationRepository = salesExpectationRepository;
            _stockChickeUsedRepository = stockChickeUsedRepository;
            _stockChickenByBranchRepository = stockChickenByBranchRepository;
            _iRHTrabRepository = iRhTrabRepository;
            _sucursalDB1Repository = sucursalDB1Repository;
            _sucursalDB2Repository = sucursalDB2Repository;
            _stockDB1Repository = stockDB1Repository;
            _stockDB2Repository = stockDB2Repository;
        }
        /// <summary>
        /// GET para retornar por ID Expectativa de Venta
        /// </summary>
        /// <param name="id">ID del registro que se quiere retornar</param>
        /// <returns></returns>
        [HttpGet]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [Route("{id}")]
        public ActionResult<Models.ApiResponse.ApiResponse<StockChickenDto>> GetById(int id)
        {
            Models.ApiResponse.ApiResponse<StockChickenDto> response = new Models.ApiResponse.ApiResponse<StockChickenDto>();
            try
            {
                var res = _mapper.Map<StockChickenDto>(_salesExpectationRepository.Find(f => f.Id == id));
                response.Success = true;
                response.Message = "Consult was successfully";
                response.Result = res;
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
        /// GET para retornar catálogo de articulos para stock de pollo
        /// </summary>
        /// <param name="dataBase">dataBase base de datos que se obtiene de login</param>
        /// <returns></returns>
        [HttpGet("GetStock", Name = "GetStock")]
        public ActionResult<ApiResponse<List<StockDto>>> GetStock(int id_sucursal, string dataBase)
        {
            var response = new ApiResponse<List<StockDto>>();

            try
            {
                switch (dataBase)
                {
                    case "DB1":
                        response.Result = _mapper.Map<List<StockDto>>(_stockDB1Repository.GetStock(id_sucursal));
                        response.Message = "success";
                        break;
                    case "DB2":
                        response.Result = _mapper.Map<List<StockDto>>(_stockDB2Repository.GetStock(id_sucursal));
                        response.Message = "success";
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = ex.ToString();
                _logger.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        /// <summary>
        /// GET para validar stock de pollo con la base ICG
        /// </summary>
        /// <param name="id_sucursal">ID del sucursal que se obtiene de login</param>
        /// <param name="dataBase">dataBase base de datos que se obtiene de login</param>
        /// <param name="cantidad">cantidad de stock de pollo a ingresar al sistema</param>
        /// <returns></returns>
        [HttpGet("ValidateStock", Name = "ValidateStock")]
        public ActionResult<ApiResponse<List<StockDto>>> ValidateStock(int id_sucursal, string dataBase, decimal cantidad, int codarticulo)
        {
            var response = new ApiResponse<List<StockDto>>();
            decimal _cantidad = 0;
            try
            {
                switch (dataBase)
                {
                    case "DB1":
                        if(_stockDB1Repository.StockValidate(id_sucursal, codarticulo) < 0)
                        {
                            _cantidad = _stockDB1Repository.StockValidate(id_sucursal, codarticulo) + cantidad;

                        }
                        else
                        {
                            _cantidad = _stockDB1Repository.StockValidate(id_sucursal, codarticulo) - cantidad;
                        }
                        
                        _cantidad = _cantidad < 0 ? _cantidad * -1 : _cantidad;
                        if (_cantidad >= 10)
                        {
                            response.Success = true;
                            response.Message = "" + _cantidad;
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "" + _cantidad;
            }
                        break;
                    case "DB2":
                        if (_stockDB2Repository.StockValidate(id_sucursal, codarticulo) < 0)
                        {
                            _cantidad = _stockDB2Repository.StockValidate(id_sucursal, codarticulo) + cantidad;
                        }
                        else
                        {
                            _cantidad = _stockDB2Repository.StockValidate(id_sucursal, codarticulo) - cantidad;
                        }

                        _cantidad = _cantidad < 0 ? _cantidad * -1 : _cantidad;
                        if (_cantidad >= 10)
                        {
                            response.Success = true;
                            response.Message = _cantidad.ToString();
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = _cantidad.ToString();
                        }
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = ex.ToString();
                _logger.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        /// <summary>
        /// POST Para actualizar Stock y hacer regularizacion en ICG
        /// </summary>
        /// <param name="dataBase">dataBase base de datos que se obtiene de login</param>
        /// <returns></returns>
        [HttpPost("AddRegularizate", Name = "AddRegularizate")]
        public ActionResult<ApiResponse<StockDto>> AddRegularizate(int codArticulo, string codAlmacen, int cantidad, string dataBase)
        {
            var response = new ApiResponse<StockDto>();

            try
            {
                switch (dataBase)
                {
                    case "DB1":
                        response.Result = _mapper.Map<StockDto>(_stockDB1Repository.UpdateStock(codArticulo, codAlmacen, cantidad));
                        response.Message = "success";
                        break;
                    case "DB2":
                        response.Result = _mapper.Map<StockDto>(_stockDB2Repository.UpdateStock(codArticulo, codAlmacen, cantidad));
                        response.Message = "success";
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = ex.ToString();
                _logger.LogError($"Something went wrong: {ex.ToString()}");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet("{id}/Sales-Expectation")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<ApiResponse<SalesExpectationGet>>> GetSalesExpectationByBranch(int id)
        {
            var response = new Models.ApiResponse.ApiResponse<SalesExpectationGet>();
            try
            {
                DateTime date = DateTime.Now;
                var salesExpectation = _stockChickenByBranchRepository.GetAll().FirstOrDefault(f =>
                    f.BranchId == id && f.CreatedDate >= date.AbsoluteStart() && f.CreatedDate <= date.AbsoluteEnd());
                var stockChicken = _mapper.Map<List<StockChickenGetDto>>(await _salesExpectationRepository.GetAll(id));
                var amount = stockChicken.Sum(s => s.Amount);
                var res = new SalesExpectationGet()
                {
                    AmountTotal = salesExpectation?.Amount,
                    SalesExpectationTotal = salesExpectation?.SalesExpectations,
                    CompletePercentage = salesExpectation != null ? (decimal)amount * 100 / salesExpectation.Amount : 0
                };
                response.Success = true;
                response.Message = "Consult was successfully";
                response.Result = res;
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
        /// Retorno de lista de Stock de Pollo por Día y por Sucursal
        /// </summary>
        /// <param name="id">ID de Sucursal</param>
        /// <returns>Lista de Stock de Pollos</returns>
        [HttpGet("By-Branch/{id}")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<Models.ApiResponse.ApiResponse<List<StockChickenGetDto>>>> GetByBranch(int id)
        {
            Models.ApiResponse.ApiResponse<List<StockChickenGetDto>> response = new Models.ApiResponse.ApiResponse<List<StockChickenGetDto>>();
            try
            {
                var res = _mapper.Map<List<StockChickenGetDto>>(await _salesExpectationRepository.GetAll(id));
                response.Success = true;
                response.Message = "Consult was successfully";
                response.Result = res;
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
        /// Agregar un nuevo stock de Pollo
        /// </summary>
        /// <param name="stockChicken"></param>
        /// <returns>Retorna un nuevo objeto de StockChicken</returns>
        /// <response code="201">Regresa el nuevo objeto creado</response>
        /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>
        /// <response code="500">Error Interno</response>
        [HttpPost]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Models.ApiResponse.ApiResponse<StockChickenDto>>> Post([FromBody] StockChickenDto stockChicken)
        {
            Models.ApiResponse.ApiResponse<StockChickenDto> response = new Models.ApiResponse.ApiResponse<StockChickenDto>();
            try
            {
                response.Result = _mapper.Map<StockChickenDto>(await _salesExpectationRepository.AddAsyn(_mapper.Map<StockChicken>(stockChicken)));
                response.Success = true;
                response.Message = "Record was added succesfully.";
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
        /// Actualiza un registro de Stock de Pollo
        /// </summary>
        /// <param name="stockChicken"> Objeto de Stock Chicken</param>
        /// <returns>Regresa registro actualizado</returns>
        /// <returns>Retorna un nuevo objeto de StockChicken</returns>
        /// <response code="200">Regresa el nuevo objeto creado</response>
        /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>
        /// <response code="500">Error Interno</response>
        [HttpPut]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Models.ApiResponse.ApiResponse<StockChickenDto>>> Put([FromBody] StockChickenDto stockChicken)
        {
            Models.ApiResponse.ApiResponse<StockChickenDto> response = new Models.ApiResponse.ApiResponse<StockChickenDto>();
            try
            {
                response.Result = _mapper.Map<StockChickenDto>(await _salesExpectationRepository.UpdateAsync(_mapper.Map<StockChicken>(stockChicken), stockChicken.Id));
                response.Success = true;
                response.Message = "Record was updated succesfully.";
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
        /// Eliminar un Stock de Pollo
        /// </summary>
        /// <param name="id">ID de Stock de Pollo a eliminar</param>
        /// <returns>Regresa objeto eliminado</returns>
        /// <response code="200">Regresa respuesta exitosa de objeto elimanado</response>
        /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>
        /// <response code="404">No existe registro</response>
        /// <response code="500">Error Interno</response>
        [HttpDelete("{id}")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Models.ApiResponse.ApiResponse<StockChickenDto>>> Delete(int id)
        {
            Models.ApiResponse.ApiResponse<StockChickenDto> response = new Models.ApiResponse.ApiResponse<StockChickenDto>();
            try
            {
                if (_salesExpectationRepository.Exists(id))
                {
                    var stock = _salesExpectationRepository.Get(id);
                    await _salesExpectationRepository.DeleteAsyn(stock);
                    response.Success = true;
                    response.Result = _mapper.Map<StockChickenDto>(stock);
                    response.Message = "Record was deleted successfully.";
                }
                else
                {
                    _logger.LogError("Not Exist Record");
                    response.Success = false;
                    response.Message = "Record not exist.";
                    return StatusCode(404, response);
                }
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
        /// Agregar uso de paquetes de pollos usados
        /// </summary>
        /// <param name="packageUsed">Objeto que se recive para agregar un nuevo registro de paquete usado</param>
        /// <returns></returns>
        /// <response code="200">Regresa respuesta exitosa de objeto elimanado</response>
        /// <response code="400">Si el objeto no cumple con los requeriemientos necesarios</response>
        /// <response code="404">No existe registro</response>
        /// <response code="406">No existe registro</response>
        /// <response code="500">Error Interno</response>
        [HttpPut("Package-Used")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Models.ApiResponse.ApiResponse<string>>> PackageUsed(PackageUsed packageUsed)
        {
            Models.ApiResponse.ApiResponse<string> response = new Models.ApiResponse.ApiResponse<string>();
            try
            {
                if (_salesExpectationRepository.Exists(e => e.Id == packageUsed.Id))
                {
                    var stockChicken = _salesExpectationRepository.GetAllIncluding(f => f.StockChickeUseds).First(f=>f.Id ==packageUsed.Id);
                    decimal a = stockChicken.Amount;
                    decimal b = stockChicken.StockChickeUseds.Sum(s => s.AmountUsed);
                    if (a > b && (a - b) >= packageUsed.Amount)
                    {
                        await _stockChickeUsedRepository.AddAsyn(new StockChickeUsed()
                        {
                            Id = 0,
                            StockChickenId = packageUsed.Id,
                            AmountUsed = packageUsed.Amount,
                            CreatedBy = packageUsed.UserId,
                            CreatedDate = DateTime.Now,
                        });
                        stockChicken.StatusId = a == (b + packageUsed.Amount) ? 2 : 3;
                        stockChicken.UpdatedBy = packageUsed.UserId;
                        stockChicken.UpdatedDate = DateTime.Now;
                        await _salesExpectationRepository.UpdateAsync(stockChicken, stockChicken.Id);
                        response.Success = true;
                        response.Message = "Uso de paquete agregado correctamente.";
                    }
                    else if(a == (b + packageUsed.Amount) && (a - b) < packageUsed.Amount)
                    {
                        await _stockChickeUsedRepository.AddAsyn(new StockChickeUsed()
                        {
                            Id = 0,
                            StockChickenId = packageUsed.Id,
                            AmountUsed = packageUsed.Amount,
                            CreatedBy = packageUsed.UserId,
                            CreatedDate = DateTime.Now,
                        });
                        stockChicken.StatusId = 2;
                        stockChicken.UpdatedBy = packageUsed.UserId;
                        stockChicken.UpdatedDate = DateTime.Now;
                        await _salesExpectationRepository.UpdateAsync(stockChicken, stockChicken.Id);
                        response.Success = true;
                        response.Message = "Uso de paquete agregado correctamente.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "El monto supera al total actual o usado.";
                        return StatusCode(406, response);
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Not Found";
                    return StatusCode(404, response);
                }
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
        /// Agregar un Stock de Pollo para Admin
        /// </summary>
        /// <param name="stockChickenByBranchDto">Objeto de Petición</param>
        /// <returns></returns>
        [HttpPost("Admin")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<StockChickenByBranchDto>>> PostAdmin([FromBody] StockChickenByBranchDto stockChickenByBranchDto)
        {
            ApiResponse<StockChickenByBranchDto> response = new ApiResponse<StockChickenByBranchDto>();
            try
            {
                response.Result = _mapper.Map<StockChickenByBranchDto>(
                    await _stockChickenByBranchRepository.AddAsyn(
                        _mapper.Map<StockChickenByBranch>(stockChickenByBranchDto)));
                response.Success = true;
                response.Message = "Record was added successfully.";
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
        /// Actualizar un Stock de pollo para Admin
        /// </summary>
        /// <param name="stockChickenByBranchDto">Objeto a actualizar</param>
        /// <returns></returns>
        [HttpPut("Admin")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<StockChickenByBranchDto>>> PutAdmin(StockChickenByBranchDto stockChickenByBranchDto)
        {
            ApiResponse<StockChickenByBranchDto> response = new ApiResponse<StockChickenByBranchDto>();
            try
            {
                if (await _stockChickenByBranchRepository.ExistsAsync(e => e.Id == stockChickenByBranchDto.Id))
                {
                    await _stockChickenByBranchRepository.UpdateAsync(_mapper.Map<StockChickenByBranch>(stockChickenByBranchDto), stockChickenByBranchDto.Id);
                    response.Success = true;
                    response.Message = "Record was updated successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Not Found";
                    return StatusCode(404, response);
                }
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
        /// Regresa Objeto de Stock de pollo para Admin por ID
        /// </summary>
        /// <param name="id">ID de registro de Stock de pollo</param>
        /// <returns></returns>
        [HttpGet("Admin/By-Branch/{id}")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<StockChickenByBranchDto>>> GetAdminByBranch(int id)
        {
            ApiResponse<StockChickenByBranchDto> response = new ApiResponse<StockChickenByBranchDto>();
            try
            {
                var res = _mapper.Map<StockChickenByBranchDto>(_stockChickenByBranchRepository.GetAll()
                    .FirstOrDefault(f => f.Id == id));
                response.Success = true;
                response.Message = "Consult was successfully";
                response.Result = res;
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
        /// Regresa lista de Sucursales de Stock de pollo
        /// </summary>
        /// <returns>Lista de Sucursales de Stock de Pollo</returns>
        [HttpGet("Admin/All-Branch/")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<StockChickenAdminList>>>> GetAdminListByBranch(string dataBase)
        {
            ApiResponse<List<StockChickenAdminList>> response = new ApiResponse<List<StockChickenAdminList>>();
            try
            {
                List<TransferDto> list = new List<TransferDto>();


                switch (dataBase)
                {
                    case "DB1":
                        list = _mapper.Map<List<TransferDto>>(_sucursalDB1Repository.GetBranchList());
                        break;
                    case "DB2":
                        list = _mapper.Map<List<TransferDto>>(_sucursalDB2Repository.GetBranchList());
                        break;
                    default:

                        break;
                }
                //var list = _mapper.Map<List<TransfersListDto>>(_iRHTrabRepository.GetBranchList());
                
                var stockChickenByBranchDtos = _mapper.Map<List<StockChickenByBranchDto>>(_stockChickenByBranchRepository.GetAll()
                    .Where(f => f.CreatedDate.Date == DateTime.Now.Date).ToList());
                List<StockChickenAdminList> adminLists = new List<StockChickenAdminList>();

                foreach (var item in list)
                {
                    adminLists.Add(new StockChickenAdminList()
                    {
                        BranchName = item.Name,
                        BranchId = item.BranchId,
                        Description = item.Description,
                        SalesExpectations = "",
                        SupervisorName = "",
                        SupervisorPhoto = "",
                        SalesExpectationsId = 0
                    });
                }

                foreach (var item in adminLists)
                {
                    item.SalesExpectations = stockChickenByBranchDtos.Any(a =>
                        a.BranchId == item.BranchId && a.CreatedDate.Date == DateTime.Now.Date)
                        ? $"{FormattedAmount(stockChickenByBranchDtos.First(a => a.BranchId == item.BranchId && a.CreatedDate.Date == DateTime.Now.Date).SalesExpectations)} MXN / {stockChickenByBranchDtos.First(a => a.BranchId == item.BranchId && a.CreatedDate.Date == DateTime.Now.Date).Amount} Kg de pollo"
                        : "Por definir";
                    item.SalesExpectationsId = stockChickenByBranchDtos.Any(a =>
                        a.BranchId == item.BranchId && a.CreatedDate.Date == DateTime.Now.Date)
                        ? stockChickenByBranchDtos.First(a => a.BranchId == item.BranchId && a.CreatedDate.Date == DateTime.Now.Date).Id
                        : 0;
                }
                
                response.Success = true;
                response.Message = "Consult was successfully";
                response.Result = adminLists;
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
        
        private static string FormattedAmount(decimal _amount)
        {
            return $"{_amount:C}";
        }  
        
    }

}
