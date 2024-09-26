using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace CashFlow.Api.Controllers;

[Authorize(Roles = nameof(Role.Administrator))]
public class ReportController : CashFLowBaseController
{
    [HttpGet("excel")]
    [SwaggerOperation(Summary = "Gera o relatório de despesas em um mês em Excel.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel(
        [FromServices] IGenerateExpenseReportExcelUseCase useCase,
        [FromQuery] DateOnly date
    )
    {
        var file = await useCase.Execute(date);
        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "Report.xlsx");

        return NoContent();
    }

    [HttpGet("pdf")]
    [SwaggerOperation(Summary = "Gera o relatório de despesas em um mês em PDF.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf(
        [FromServices] IGenerateExpenseReportPdfUseCase useCase,
        [FromQuery] DateOnly date
    )
    {
        var file = await useCase.Execute(date);
        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Pdf, "Report.pdf");

        return NoContent();
    }
}
