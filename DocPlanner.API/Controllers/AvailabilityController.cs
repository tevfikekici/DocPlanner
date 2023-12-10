using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DocPlanner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public AvailabilityController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpGet("GetWeeklyAvailability/{date}")]
        public async Task<IActionResult> GetWeeklyAvailability(int date)
        {
            try
            {
                var result = await _slotService.GetWeeklyAvailabilityAsync(DateTime.ParseExact(date.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture));

                return Ok(result);
            }
            catch (HttpRequestException e)
            {
                return StatusCode(503, e.Message);
            }
        }

        [HttpPost("TakeSlot")]
        public async Task<IActionResult> TakeSlot([FromBody] SlotBooking slotBooking)
        {
            try
            {
                var success = await _slotService.TakeSlotAsync(slotBooking);
                if (success.StatusCode == "OK")
                {
                    return Ok();
                }
                return BadRequest(success.StatusCode + " - " + success.Content);
            }
            catch (HttpRequestException e)
            {
                return StatusCode(503, e.Message);
            }
        }
    }
}
