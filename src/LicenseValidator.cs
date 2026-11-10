using System.Security.Cryptography;
using System.Text;

namespace InvoiceDesk.Licensing;

/// <summary>Sample product logic for a marketing demo. Not a real licensing scheme.</summary>
internal static class LicenseValidator
{
    private static readonly string ProductSalt = "invoicedesk-pro-2026";

    internal static bool IsValid(string customerEmail, string licenseKey)
    {
        if (string.IsNullOrWhiteSpace(licenseKey) || licenseKey.Length != 29)
            return false;

        var expected = ComputeKey(customerEmail);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.ASCII.GetBytes(expected),
            Encoding.ASCII.GetBytes(licenseKey.ToUpperInvariant()));
    }

    internal static string ComputeKey(string customerEmail)
    {
        var input = customerEmail.Trim().ToLowerInvariant() + ":" + ProductSalt;
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var hex = Convert.ToHexString(hash)[..25];
        return string.Join("-", Enumerable.Range(0, 5).Select(i => hex.Substring(i * 5, 5)));
    }
}
