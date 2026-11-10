using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using CilSentinel.RuntimeSupport;

namespace InvoiceDesk.Licensing;

internal static class AvaebCaldbbwcdbJbgbCd
{
	[CompilerGenerated]
	private sealed class ApaVbWdscRaaecaAebbwb
	{
		public string hex;

		internal string AQaYaUdzdmeAebaqeIava(int i)
		{
			return hex.Substring(i * 5, 5);
		}
	}

	private static readonly string ADaPdreGapeTbUcMbXcNc = ArcJaBddbgbibiajaZbLd.AqdhbaagckcPaIcAaybCa(608177074, 145956453, -2074505810, 392448148, cacheResult: true);

	internal static bool AueLdCaUcecNbAaIdYbDe(string customerEmail, string licenseKey)
	{
		int num = -231558403;
		while (true)
		{
			switch ((num - 2000301672) ^ 0x7AF88495)
			{
			case 2:
				return false;
			case 3:
			{
				string s = ACaxbydBdpeMcHbmbhcFe(customerEmail);
				return CryptographicOperations.FixedTimeEquals(Encoding.ASCII.GetBytes(s), Encoding.ASCII.GetBytes(licenseKey.ToUpperInvariant()));
			}
			default:
				num = (string.IsNullOrWhiteSpace(licenseKey) ? (-231558401) : (-231558404));
				break;
			case 1:
				num = ((licenseKey.Length == 29) ? (-231558402) : (-231558401));
				break;
			}
		}
	}

	internal static string ACaxbydBdpeMcHbmbhcFe(string customerEmail)
	{
		string s = customerEmail.Trim().ToLowerInvariant() + ":" + ADaPdreGapeTbUcMbXcNc;
		byte[] inArray = SHA256.HashData(Encoding.UTF8.GetBytes(s));
		string hex = Convert.ToHexString(inArray).Substring(0, 25);
		return string.Join("-", from i in Enumerable.Range(0, 5)
			select hex.Substring(i * 5, 5));
	}
}
