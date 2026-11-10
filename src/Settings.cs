using System.Text.Json;

namespace InvoiceDesk.Configuration;

// Read from the customer's settings file by property name: renaming these
// members would silently reset every existing file to defaults.
internal sealed class AppSettings
{
    public string CompanyName { get; set; } = "Example Ltd";
    public string DefaultCurrency { get; set; } = "EUR";
    public int PaymentTermDays { get; set; } = 30;

    public string ToJson() => JsonSerializer.Serialize(this);
    public static AppSettings FromJson(string json) => JsonSerializer.Deserialize<AppSettings>(json) ?? new();
}
