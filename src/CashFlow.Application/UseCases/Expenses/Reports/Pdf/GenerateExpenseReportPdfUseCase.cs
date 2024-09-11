using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Repositories;
using CashFlow.Exception;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpenseReportPdfUseCase : IGenerateExpenseReportPdfUseCase
{
    private readonly string CURRENCY_SYMBOL = "R$";
    private readonly int HEIGHT_ROW = 25;
    private readonly IExpensesReadOnlyRepository _repository;

    public GenerateExpenseReportPdfUseCase(IExpensesReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(DateOnly date)
    {
        var expensesByDate = await _repository.FilterByMonth(date);

        if (expensesByDate is []) return [];

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();

        var document = CreateDocument(date);
        var page = CreatePage(document);

        CreateHeaderWithAvatarAndName(page);
        var totalExpenses = expensesByDate.Sum(expense => expense.Amount);
        CreateTotalExpensesSection(page, date, totalExpenses);

        foreach(var expense in expensesByDate)
        {
            CreateExpenseTable(page, expense);
        }

        return RenderDocument(document);
    }

    #region Document Methots
    private Document CreateDocument(DateOnly date)
    {
        var document = new Document();
        document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {date.ToString("Y")}";
        document.Info.Author = "Bruno Gabriel Knop";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.INTER_REGULAR;
        return document;
    }
    
    private Section CreatePage(Document document)
    {
        var section = document.AddSection();

        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }
    
    private byte[] RenderDocument(Document document)
    {
        var render = new PdfDocumentRenderer()
        {
            Document = document
        };
        render.RenderDocument();

        using var file = new MemoryStream();
        render.PdfDocument.Save(file);
        return file.ToArray();
    }
    #endregion

    #region Intro Content Sections
    private void CreateHeaderWithAvatarAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn();

        var row = table.AddRow();
        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "UseCases", "Expenses", "Reports", "Pdf", "Logo", "avatar.png");
        var image = row.Cells[0].AddImage(pathFile);
        image.Width = 60;
        image.Height = 60;
        row.Cells[1].AddParagraph("Hey, Bruno Knop");
        row.Cells[1].Format.Font = new Font
        {
            Name = FontHelper.POPPINS_BLACK,
            Size = 16
        };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }
    
    private void CreateTotalExpensesSection(Section page, DateOnly date, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";
        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPEND_IN, date.ToString("Y"));
        paragraph.AddFormattedText(title, new Font
        {
            Name = FontHelper.INTER_REGULAR,
            Size = 15
        });
        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalExpenses}", new Font
        {
            Name = FontHelper.POPPINS_BLACK,
            Size = 50
        });
    }
    #endregion

    #region Expense Table Methods
    private void CreateExpenseTable(Section page, Expense expense)
    {
        var table = page.AddTable();
        CreateTableColums(table);

        var row = table.AddRow();
        row.Height = HEIGHT_ROW;

        CreateExpenseTitleCell(row.Cells[0], expense.Title);
        CreateHeaderForAmountCell(row.Cells[3]);

        row = table.AddRow();
        row.Height = HEIGHT_ROW;
        row.Shading.Color = ColorHelper.GREEN_DARK;

        CreateSecondRowContentCell(row.Cells[0], expense.Date.ToString("D"));
        row.Cells[0].Format.LeftIndent = 20;
            
        CreateSecondRowContentCell(row.Cells[1], expense.Date.ToString("hh:mm tt"));
        CreateSecondRowContentCell(row.Cells[2], expense.PaymentType.PaymentTypeToString());
        CreateAmountCell(row.Cells[3], $"- {CURRENCY_SYMBOL} {expense.Amount}");
            

        if (string.IsNullOrWhiteSpace(expense.Description) is false)
        {
            var descriptionRow = table.AddRow();
            descriptionRow.Height = 25;
            CreateDescriptionCell(descriptionRow.Cells[0], expense.Description);
            row.Cells[3].MergeDown = 1;
        }
            
        row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }
    
    private void CreateTableColums(Table table)
    {
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;
    }
    
    private void CreateExpenseTitleCell(Cell cell, string title)
    {
        cell.Shading.Color = ColorHelper.RED_LIGHT;
        cell.MergeRight = 2;
        cell.AddParagraph(title);
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.Font = new Font{ Name = FontHelper.POPPINS_BLACK, Size = 15, Color = ColorHelper.BLACK };
        cell.Format.LeftIndent = 20;
    }

    private void CreateHeaderForAmountCell(Cell cell)
    {
        cell.Shading.Color = ColorHelper.RED_DARK;
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font{ Name = FontHelper.POPPINS_BLACK, Size = 15, Color = ColorHelper.WHITE };
        cell.Format.RightIndent = 10;
    }

    private void CreateSecondRowContentCell(Cell cell, string content)
    {
        cell.AddParagraph(content);
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.Font = new Font { Name = FontHelper.INTER_REGULAR, Size = 12, Color = ColorHelper.BLACK };
    }

    private void CreateAmountCell(Cell cell, string amount)
    {
        cell.AddParagraph(amount);
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.Font = new Font { Name = FontHelper.INTER_REGULAR, Size = 14, Color = ColorHelper.BLACK };
        cell.Shading.Color = ColorHelper.WHITE;
        cell.Format.RightIndent = 10;
    }

    private void CreateDescriptionCell(Cell cell, string description)
    {
        cell.MergeRight = 2;
        cell.Shading.Color = ColorHelper.GREEN_LIGHT;
        cell.AddParagraph(description);
        cell.Format.Font = new Font { Name = FontHelper.INTER_REGULAR, Size = 10, Color = ColorHelper.BLACK };
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.LeftIndent = 20;
    }
    #endregion
}
