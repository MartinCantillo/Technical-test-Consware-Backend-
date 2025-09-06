using backend.Dtos.User;
using backend.Model;
using backend.Repository;
using backend.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;

        public UserController(IUser userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<Response>>> Register([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<Response>.Fail("Datos inválidos"));

            try
            {
                var user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = dto.Password,
                    Role = dto.Role
                };

                var created = await _userService.CreateUser(user);

                var response = new Response
                {
                    Id = created.Id,
                    Name = created.Name,
                    Role = created.Role,
                    CreatedAtUtc = created.CreatedAtUtc,
                    UpdatedAtUtc = created.UpdatedAtUtc
                };

                return Ok(ApiResponse<Response>.Created(response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Response>.ServerError(ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] UserLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.Fail("Datos inválidos"));

            try
            {
                var user = await _userService.Authenticate(dto.Email, dto.Password);
                if (user == null)
                    return Unauthorized(ApiResponse<object>.Unauthorized("Credenciales inválidas"));

                var token = _userService.GenerateToken(user.Id, user.Name, user.Role);

                var data = new
                {
                    Token = token,
                    User = new Response
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Role = user.Role,
                        CreatedAtUtc = user.CreatedAtUtc,
                        UpdatedAtUtc = user.UpdatedAtUtc
                    }
                };

                return Ok(ApiResponse<object>.Success(data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ServerError(ex.Message));
            }
        }


        [Authorize]
        [HttpPut("update-password")]
        public async Task<ActionResult<ApiResponse<string>>> UpdatePassword([FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Datos inválidos"));

            try
            {
                var updated = await _userService.UpdatePassword(dto.Email, dto.Password);
                if (!updated)
                    return NotFound(ApiResponse<string>.NotFound("Usuario no encontrado"));

                return Ok(ApiResponse<string>.Success("Contraseña actualizada correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Aprobador")]
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Response>>>> GetAll()
        {
            try
            {
                var users = await _userService.GetAllByRol();
                var mapped = users.Select(u => new Response
                {
                    Id = u.Id,
                    Name = u.Name,
                    Role = u.Role,
                    CreatedAtUtc = u.CreatedAtUtc,
                    UpdatedAtUtc = u.UpdatedAtUtc
                });

                return Ok(ApiResponse<IEnumerable<Response>>.Success(mapped));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Response>>.ServerError(ex.Message));
            }
        }


    }
}
