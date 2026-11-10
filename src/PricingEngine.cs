namespace InvoiceDesk.Billing;

internal sealed record LineItem(string Sku, int Quantity, decimal UnitPrice);

internal static class PricingEngine
{
    internal static decimal VolumeDiscountRate(int totalUnits) => totalUnits switch
    {
        >= 500 => 0.18m,
        >= 100 => 0.11m,
        >= 25 => 0.05m,
        _ => 0m,
    };

    internal static decimal Total(IReadOnlyList<LineItem> items, string countryCode)
    {
        var units = items.Sum(i => i.Quantity);
        var net = items.Sum(i => i.Quantity * i.UnitPrice);
        var discounted = net * (1 - VolumeDiscountRate(units));
        var vat = countryCode == "HU" ? 0.27m : countryCode == "DE" ? 0.19m : 0m;
        return Math.Round(discounted * (1 + vat), 2, MidpointRounding.ToEven);
    }
}
