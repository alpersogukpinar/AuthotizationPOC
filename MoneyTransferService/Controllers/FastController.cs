using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MoneyTransferService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FastController : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "FAST.Create")]
        public IActionResult DoFast()
        {
            return Ok("Fast işlemi başarılı!");
        }
    }
}