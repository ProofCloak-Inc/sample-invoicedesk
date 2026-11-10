using System.Runtime.CompilerServices;
using System.Text.Json;
using CilSentinel.RuntimeSupport;

namespace InvoiceDesk.Configuration;

internal sealed class AscadMdiaBbFbFemdbaLe
{
	[CompilerGenerated]
	private string AseadNcydVcJeMaadNcPc = ArcJaBddbgbibiajaZbLd.AqdhbaagckcPaIcAaybCa(-483195073, 598445458, 299222900, 243254893, cacheResult: true);

	[CompilerGenerated]
	private string AQamdYaXaVaxaZdLdLeDe = ArcJaBddbgbibiajaZbLd.AqdhbaagckcPaIcAaybCa(1808907341, 1708876851, -1293044857, 2023413664, cacheResult: true);

	[CompilerGenerated]
	private int ABbSbVecbcaSePeSecdse = 30;

	public string CompanyName
	{
		[CompilerGenerated]
		get
		{
			return AseadNcydVcJeMaadNcPc;
		}
		[CompilerGenerated]
		set
		{
			AseadNcydVcJeMaadNcPc = value;
		}
	}

	public string DefaultCurrency
	{
		[CompilerGenerated]
		get
		{
			return AQamdYaXaVaxaZdLdLeDe;
		}
		[CompilerGenerated]
		set
		{
			AQamdYaXaVaxaZdLdLeDe = value;
		}
	}

	public int PaymentTermDays
	{
		[CompilerGenerated]
		get
		{
			return ABbSbVecbcaSePeSecdse;
		}
		[CompilerGenerated]
		set
		{
			ABbSbVecbcaSePeSecdse = value;
		}
	}

	public string ToJson()
	{
		return JsonSerializer.Serialize(this);
	}

	public static AscadMdiaBbFbFemdbaLe FromJson(string json)
	{
		return JsonSerializer.Deserialize<AscadMdiaBbFbFemdbaLe>(json) ?? new AscadMdiaBbFbFemdbaLe();
	}
}
