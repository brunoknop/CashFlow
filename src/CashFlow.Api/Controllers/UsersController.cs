using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Application.UseCases.Users.GetProfile;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Application.UseCases.Users.Update.Password;
using CashFlow.Application.UseCases.Users.Update.Profile;
using CashFlow.Communication.Requests.Users;
using CashFlow.Communication.Responses.Users;
using CashFlow.Exception.ExceptionsBase;
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
    [ProducesResponseType(typeof(ErrorOnValidationException), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase, 
        [FromBody] RequestRegisterUserJson request)
    {
        var response = await useCase.Execute(request);
        return Created("", response);
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Obtem o perfil do usuário.")]
    [ProducesResponseType(typeof(ResponseProfileUserJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile(
        [FromServices] IGetUserProfileUseCase useCase)
    {
        var response = await useCase.Execute();
        return Ok(response);
    }
    
    [HttpDelete]
    [SwaggerOperation(Summary = "Exclui um usuário.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        [FromServices] IDeleteUserUseCase useCase)
    {
        await useCase.Execute();
        return NoContent();
    }
    
    [HttpPut]
    [SwaggerOperation(Summary = "Atualiza o perfil de um usuário.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOnValidationException), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProfile(
        [FromServices] IUpdateUserProfileUseCase useCase, 
        [FromBody] RequestUpdateUserProfileJson request)
    {
        await useCase.Execute(request);
        return NoContent();
    }
    
    [HttpPut("change-password")]
    [SwaggerOperation(Summary = "Atualiza a senha de um usuário.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorOnValidationException), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePassword(
        [FromServices] IUpdateUserPasswordUseCase useCase, 
        [FromBody] RequestUpdateUserPasswordJson request)
    {
        await useCase.Execute(request);
        return NoContent();
    }
}
