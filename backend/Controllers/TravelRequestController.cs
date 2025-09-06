using backend.Dtos.TravelRequest;
using backend.Model;
using backend.Repository;
using backend.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TravelRequestController : ControllerBase
    {
        private readonly ITravelRequest _travelService;

        public TravelRequestController(ITravelRequest travelService)
        {
            _travelService = travelService;
        }

 
        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<TravelRequest>>> Create([FromBody] TravelRequestCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<TravelRequest>.Fail("Datos inválidos"));

            try
            {
                var nowBogota = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SA Pacific Standard Time");

                var travelRequest = new TravelRequest
                {
                    UserId = dto.UserId,
                    OriginCity = dto.OriginCity,
                    DestinationCity = dto.DestinationCity,
                    DepartureDate = dto.DepartureDate,
                    ReturnDate = dto.ReturnDate,
                    Justification = dto.Justification,
                    Status = TravelRequestStatus.pending,
                    CreatedAtUtc = nowBogota,
                    UpdatedAtUtc = nowBogota
                };

                var created = await _travelService.CreateTravelRequest(travelRequest);

                return Ok(ApiResponse<TravelRequest>.Created(created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TravelRequest>.ServerError(ex.Message));
            }
        }

        
        [Authorize(Roles = "Aprobador")]
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TravelRequest>>>> GetAll()
        {
            try
            {
                var requests = await _travelService.GetAll();
                return Ok(ApiResponse<IEnumerable<TravelRequest>>.Success(requests));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<TravelRequest>>.ServerError(ex.Message));
            }
        }

   
        [Authorize(Roles = "Aprobador")]
        [HttpPut("update-status")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateStatus([FromBody] TravelRequestUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Datos inválidos"));

            try
            {
                var nowBogota = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SA Pacific Standard Time");

                var updated = await _travelService.UpdateStatus(dto.Id, dto.Status);
                if (!updated)
                    return NotFound(ApiResponse<string>.NotFound("Solicitud no encontrada"));

                return Ok(ApiResponse<string>.Success($"Estado actualizado a {dto.Status} en {nowBogota}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ServerError(ex.Message));
            }
        }
    }
}
