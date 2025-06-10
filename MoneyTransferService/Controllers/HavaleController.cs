using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MoneyTransferService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HavaleController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "Havale.Read")]
        public IActionResult ReadHavale()
        {
            return Ok("Havale Okuma başarılı!");
        }
    }
}