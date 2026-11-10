using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InvoiceDesk.Licensing;

internal static class LicenseValidator
{
	private static readonly string ProductSalt = "invoicedesk-pro-2026";

	internal static bool IsValid(string customerEmail, string licenseKey)
	{
		if (string.IsNullOrWhiteSpace(licenseKey) || licenseKey.Length != 29)
		{
			return false;
		}
		string s = ComputeKey(customerEmail);
		return CryptographicOperations.FixedTimeEquals(Encoding.ASCII.GetBytes(s), Encoding.ASCII.GetBytes(licenseKey.ToUpperInvariant()));
	}

	internal static string ComputeKey(string customerEmail)
	{
		string s = customerEmail.Trim().ToLowerInvariant() + ":" + ProductSalt;
		byte[] inArray = SHA256.HashData(Encoding.UTF8.GetBytes(s));
		string hex = Convert.ToHexString(inArray).Substring(0, 25);
		return string.Join("-", from i in Enumerable.Range(0, 5)
			select hex.Substring(i * 5, 5));
	}
}
