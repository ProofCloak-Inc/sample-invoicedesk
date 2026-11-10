using System.Text.Json;

namespace InvoiceDesk.Configuration;

internal sealed class AppSettings
{
	public string CompanyName { get; set; } = "Example Ltd";

	public string DefaultCurrency { get; set; } = "EUR";

	public int PaymentTermDays { get; set; } = 30;

	public string ToJson()
	{
		return JsonSerializer.Serialize(this);
	}

	public static AppSettings FromJson(string json)
	{
		return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
	}
}
