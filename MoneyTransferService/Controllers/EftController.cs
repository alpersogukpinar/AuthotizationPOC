using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MoneyTransferService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EftController : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = "EFT.Create")]
        public IActionResult DoEft()
        {
            return Ok("EFT işlemi başarılı!");
        }
    }
}