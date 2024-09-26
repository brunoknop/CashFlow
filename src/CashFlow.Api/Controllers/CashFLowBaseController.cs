using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public abstract class CashFLowBaseController : ControllerBase
{
    
}
