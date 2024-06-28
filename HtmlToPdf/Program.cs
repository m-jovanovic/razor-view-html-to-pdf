using HtmlToPdf;
using HtmlToPdf.Contracts;
using Razor.Templating.Core;

var builder = WebApplication.CreateBuilder(args);

License.LicenseKey = builder.Configuration["IronPdf:LicenseKey"];

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<InvoiceFactory>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("invoice-report", async (InvoiceFactory invoiceFactory) =>
{
    Invoice invoice = invoiceFactory.Create();

    var html = await RazorTemplateEngine.RenderAsync("Views/InvoiceReport.cshtml", invoice);

    var renderer = new ChromePdfRenderer();

    using var pdfDocument = renderer.RenderHtmlAsPdf(html);

    return Results.File(pdfDocument.BinaryData, "application/pdf", $"invoice-{invoice.Number}.pdf");
});

app.UseHttpsRedirection();

app.Run();
