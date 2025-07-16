using Microsoft.AspNetCore.Mvc;
using Application.UseCases;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileService _userProfileService;

        public UserProfileController(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            try
            {
                var profile = _userProfileService.GetProfile(HttpContext);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
