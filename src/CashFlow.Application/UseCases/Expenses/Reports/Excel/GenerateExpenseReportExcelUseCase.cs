using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Repositories;
using CashFlow.Exception;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpenseReportExcelUseCase : IGenerateExpenseReportExcelUseCase
{
    private readonly IExpensesReadOnlyRepository _repository;

    public GenerateExpenseReportExcelUseCase(IExpensesReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(DateOnly date)
    {
        var expensesByDate = await _repository.FilterByMonth(date);

        if (expensesByDate is []) return [];

        using var workbook = new XLWorkbook();
        workbook.Style.Font.FontName = "Calibri";
        workbook.Style.Font.FontSize = 11;
        var worksheet = workbook.Worksheets.Add(date.ToString("Y"));

        CreateHeader(worksheet);
        InsertContent(expensesByDate, worksheet);

        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();
        workbook.SaveAs(file);
        return file.ToArray();
    }

    private void CreateHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");

        worksheet.Column("B").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Column("C").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Column("D").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }

    private void InsertContent(List<Expense> expensesByDate, IXLWorksheet worksheet)
    {
        var row = 2;
        foreach(var expense in expensesByDate)
        {
            worksheet.Cells($"A{row}").Value = expense.Title;
            worksheet.Cells($"B{row}").Value = expense.Date;
            worksheet.Cells($"C{row}").Value = expense.PaymentType.PaymentTypeToString();

            worksheet.Cells($"D{row}").Value = expense.Amount;
            worksheet.Cells($"D{row}").Style.NumberFormat.Format = "- R$ #,##0.00";

            worksheet.Cells($"E{row}").Value = expense.Description;
            row++;
        }
    }
}
