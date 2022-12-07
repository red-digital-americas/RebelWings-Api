using api.rebel_wings.ActionFilter;
using api.rebel_wings.Models.ApiResponse;
using api.rebel_wings.Models.User;
using AutoMapper;
using biz.fortia.Repository.RH;
using biz.rebel_wings.Entities;
using biz.rebel_wings.Paged;
using biz.rebel_wings.Repository.User;
using biz.rebel_wings.Services.Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace api.rebel_wings.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IUserRepository _userRepository;
        private readonly IRHTrabRepository _iRHTrabRepository;
        private readonly biz.bd1.Repository.Sucursal.ISucursalRepository _sucursalDB1Repository;
        private readonly biz.bd2.Repository.Sucursal.ISucursalRepository _sucursalDB2Repository;

        public UserController(IUserRepository userRepository,
            IMapper mapper,
            ILoggerManager logger,
            IRHTrabRepository rHTrabRepository,
            biz.bd1.Repository.Sucursal.ISucursalRepository sucursalDB1Repository,
            biz.bd2.Repository.Sucursal.ISucursalRepository sucursalDB2Repository)
        {
            _userRepository = userRepository;
            _mapper= mapper;
            _logger = logger;   
            _iRHTrabRepository = rHTrabRepository;
            _sucursalDB1Repository = sucursalDB1Repository;
            _sucursalDB2Repository = sucursalDB2Repository;
        }

        [HttpGet]
        public ActionResult<ApiResponse<List<UserDto>>> GetAll()
        {
            var response = new ApiResponse<List<UserDto>>();

            try
            {
                response.Result = _mapper.Map<List<UserDto>>(_userRepository.GetAll());
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error 1";
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public ActionResult<ApiResponse<PagedList<UserDto>>> GetPaged(int pageNumber, int pageSize)
        {
            var response = new ApiResponse<PagedList<UserDto>>();

            try
            {
                response.Result = _mapper.Map<PagedList<UserDto>>
                    (_userRepository.GetAllPaged(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = ex.ToString();
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<ApiResponse<UserDto>> GetById(int id)
        {
            var response = new ApiResponse<UserDto>();

            try
            {
                response.Result = _mapper.Map<UserDto>(_userRepository.Find(c => c.Id == id));
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error2";
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet("GetSucursalList", Name = "GetSucursalList")]
        public ActionResult<ApiResponse<List<SucursalesDto>>> GetSucursalList(int idState)
        {
            var response = new ApiResponse<List<SucursalesDto>>();
  
            try
            {
                switch (idState)
                {
                    case 1:
                        response.Result = _mapper.Map<List<SucursalesDto>>(_sucursalDB2Repository.GetAll().Where(x => x.Descatalogado == false));
                        response.Message = "success";
                        break;
                    case 2:
                        response.Result = _mapper.Map<List<SucursalesDto>>(_sucursalDB1Repository.GetAll().Where(x => x.Descatalogado == false));
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

        [HttpGet("GetStateList", Name = "GetStateList")]
        public ActionResult<ApiResponse<List<CatStateDto>>> GetStateList()
        {
            var response = new ApiResponse<List<CatStateDto>>();

            try
            {
                response.Result = _mapper.Map<List<CatStateDto>>(_userRepository.CatStateList());
                response.Message = "success";
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

        [HttpPost]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public ActionResult<ApiResponse<UserDto>> Create(UserDto item)
        {
            var response = new ApiResponse<UserDto>();

            try
            {
                if (_userRepository.Exists(c => c.Email == item.Email))
                {
                    response.Success = false;
                    response.Message = $"Email: { item.Email } Already Exists";
                    return BadRequest(response);
                }

                User user = _userRepository.Add(_mapper.Map<User>(item));
                response.Result = _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = ex.ToString();
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return StatusCode(201, response);
        }

        [HttpPut("Recovery_password", Name = "Recovery_password")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Recovery_password(string email)
        {
            var response = new ApiResponse<UserDto>();

            try
            {
                var _user = _mapper.Map<User>(_userRepository.Find(c => c.Email == email));

                if (_user != null)
                {

                    var guid = Guid.NewGuid().ToString().Substring(0, 7);
                    var password = _userRepository.HashPassword("$" + guid);

                    _user.Password = password;
                    _user.UpdatedBy = _user.Id;
                    _user.UpdatedDate = DateTime.Now;

                    _userRepository.Update(_mapper.Map<User>(_user), _user.Id);

                    StreamReader reader = new StreamReader(Path.GetFullPath("TemplateMail/Email.html"));
                    string body = string.Empty;
                    body = reader.ReadToEnd();
                    body = body.Replace("{user}", _user.Name + " " + _user.LastName + " " + _user.MotherName);
                    body = body.Replace("{username}", $"{_user.Email}");
                    body = body.Replace("{pass}", "$" + guid);

                    _userRepository.SendMail(_user.Email, body, "Recovery password");

                    response.Result = _mapper.Map<UserDto>(_user);
                    response.Success = true;
                    response.Message = "success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Hubo un error";

                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = ex.ToString();
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpPut("Change_password", Name = "Change_password")]
        public ActionResult<ApiResponse<UserDto>> Change_password(string email, string password)
        {
            var response = new ApiResponse<UserDto>();

            try
            {
                var _user = _mapper.Map<User>(_userRepository.Find(c => c.Email == email));

                if (_user != null)
                {
                    var guid = Guid.NewGuid().ToString().Substring(0, 7);
                    var passwordNew = _userRepository.HashPassword(password);

                    _user.Password = passwordNew;
                    _user.UpdatedBy = _user.Id;
                    _user.UpdatedDate = DateTime.Now;
                    _userRepository.Update(_mapper.Map<User>(_user), _user.Id);

                    response.Result = _mapper.Map<UserDto>(_user);
                    response.Result.Token = _userRepository.BuildToken(_user);
                    response.Success = true;
                    response.Message = "success";

                    StreamReader reader = new StreamReader(Path.GetFullPath("TemplateMail/Email.html"));
                    string body = string.Empty;
                    body = reader.ReadToEnd();
                    body = body.Replace("{user}", _user.Name + " " + _user.LastName + " " + _user.MotherName);
                    body = body.Replace("{username}", $"{_user.Email}");
                    body = body.Replace("{pass}", password);
                    _userRepository.SendMail(_user.Email, body, "Change password");
                }
                else
                {
                    response.Success = false;
                    response.Message = "Usuario y/o contraseña incorrecta";

                }

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error3";
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpPost("Login", Name = "Login")]
        public async Task<ActionResult<ApiResponse<UserReturnDto>>> Login(string email, string password)
        {
            var response = new ApiResponse<UserReturnDto>();

      //try
      //{
        var _user = _mapper.Map<User>(_userRepository.Find(c => c.Email == email));
                if (_user != null)
                {
                    if (_userRepository.VerifyPassword(_user.Password, password))
                    {
                        var userData = _mapper.Map<UserDto>(_user);
                        string dataBase = "";
                        int sucursal = 0;
                        string sucursalName = "";

                        switch (userData.StateId)
                        {
                            case 1:
                                if (_sucursalDB2Repository.getSucursalById(userData.SucursalId.Value))
                                {
                                    dataBase = "DB2";
                                    sucursal = userData.SucursalId.Value;
                                    sucursalName = _sucursalDB2Repository.Find(x => x.Idfront == userData.SucursalId.Value).Titulo;
                                }
                                else
                                {
                                    dataBase = "La sucursal no existe";
                                }
                                break;
                            case 2:
                                if (_sucursalDB1Repository.getSucursalById(userData.SucursalId.Value))
                                {
                                    dataBase = "DB1";
                                    sucursal = userData.SucursalId.Value;
                                    sucursalName = _sucursalDB1Repository.Find(x => x.Idfront == userData.SucursalId.Value).Titulo;
                                }
                                else
                                {
                                    dataBase = "La sucursal no existe";
                                }
                                break;
                            default:
                                dataBase = "La base de datos no existe";
                                break;
                        }                        
                        UserReturnDto userReturnDto = new UserReturnDto()
                        {
                            Email = userData.Email,
                            Password = userData.Password,
                            Id = userData.Id,
                            LastName = userData.LastName,
                            MotherName = userData.MotherName,
                            Name = userData.Name,
                            RoleId = userData.RoleId,
                            Token = userData.Token,
                            StateId  = userData.StateId,
                            DataBase = dataBase,
                            Branch = sucursal,//_iRHTrabRepository.GetBranchId(userData.ClabTrab.Value),
                            BranchName = sucursalName,//userData.BranchId.HasValue ? _iRHTrabRepository.GetBranchNameById(userData.BranchId.Value) : "",
                            BranchId = userData.SucursalId //userData.BranchId
                        };
                        response.Result = userReturnDto;
                        response.Result.Token = _userRepository.BuildToken(_user);
                        response.Success = true;
                        response.Message = "Success";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Usuario y/o contraseña incorrecta";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Usuario y/o contraseña incorrecta";

                }

  //  }
  //          catch (Exception ex)
  //          {
  //              response.Result = null;
  //              response.Success = false;
  //              response.Message = "Internal server error4";
  //              _logger.LogError($"Something went wrong: { ex.ToString() }");
  //              return StatusCode(500, response);
  //}

            return Ok(response);
        }

        [HttpPut("{id}")]
        [ServiceFilterAttribute(typeof(ValidationFilterAttribute))]
        public ActionResult<ApiResponse<UserDto>> Update(int id, UserDto item)
        {
            var response = new ApiResponse<UserDto>();

            try
            {
                var user = _userRepository.Find(c => c.Id == id);

                if (user == null)
                {
                    response.Message = $"User id { id } Not Found";
                    return NotFound(response);
                }

                var userUpdate = _mapper.Map<User>(item);
                _userRepository.Update(userUpdate, item.Id);
                _userRepository.Save();
                response.Message = "Update record was success";
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error5";
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<UserDto>> Delete(int id)
        {
            var response = new ApiResponse<UserDto>();

            try
            {
                var user = _userRepository.Find(c => c.Id == id);

                if (user == null)
                {
                    response.Message = $"User id { id } Not Found";
                    return NotFound(response);
                }

                _userRepository.Delete(user);
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    var number = sqlException.Number;
                    if (number == 547)
                    {
                        response.Result = null;
                        response.Success = false;
                        response.Message = "Operation Not Permitted";
                        _logger.LogError("Operation Not Permitted");
                        return StatusCode(490, response);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Success = false;
                response.Message = "Internal server error6";
                _logger.LogError($"Something went wrong: { ex.ToString() }");
                return StatusCode(500, response);
            }

            return Ok(response);
        }

    }
}
