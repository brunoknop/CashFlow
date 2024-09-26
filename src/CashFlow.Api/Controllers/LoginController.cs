using CashFlow.Application.UseCases.Users.Login;
using CashFlow.Communication.Requests.Users;
using CashFlow.Communication.Responses;
using CashFlow.Communication.Responses.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CashFlow.Api.Controllers;

public class LoginController : CashFLowBaseController
{
    [HttpPost]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Efetua o login no sistema")]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestLoginJson request
    )
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }
}
