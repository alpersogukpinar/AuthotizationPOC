using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MoneyTransferService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SwiftController : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "SWIFT.Create")]
        public IActionResult DoSwift()
        {
            return Ok("Swift işlemi başarılı!");
        }

        [HttpGet]
        [Authorize(Policy = "SWIFT.Read")]
        public IActionResult ReadSwift()
        {
            return Ok("SWIFT Okuma başarılı!");
        }
    }
}