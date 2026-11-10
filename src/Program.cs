using InvoiceDesk.Billing;
using InvoiceDesk.Configuration;
using InvoiceDesk.Licensing;

var email = args.Length > 0 ? args[0] : "demo@example.com";
var key = LicenseValidator.ComputeKey(email);
Console.WriteLine($"License valid: {LicenseValidator.IsValid(email, key)}");

var items = new List<LineItem> { new("SEAT", 120, 9.90m), new("SUPPORT", 1, 199m) };
Console.WriteLine($"Total (HU): {PricingEngine.Total(items, "HU")}");

// A settings file written by an earlier release of the app.
const string existingFile = """{"CompanyName":"Contoso Kft","DefaultCurrency":"HUF","PaymentTermDays":45}""";
var settings = AppSettings.FromJson(existingFile);
Console.WriteLine($"Settings file: {settings.CompanyName}, {settings.DefaultCurrency}, {settings.PaymentTermDays} days");
Console.WriteLine($"Settings saved: {settings.ToJson()}");
