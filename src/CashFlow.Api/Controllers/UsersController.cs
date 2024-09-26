using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Requests.Users;
using CashFlow.Communication.Responses.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CashFlow.Api.Controllers;

public class UsersController : CashFLowBaseController
{
    [HttpPost]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Cadastra um novo usuário.")]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase, 
        [FromBody] RequestRegisterUserJson request)
    {
        var response = await useCase.Execute(request);
        return Created("", response);
    }
}
