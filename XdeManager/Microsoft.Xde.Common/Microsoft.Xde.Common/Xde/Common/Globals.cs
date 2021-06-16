using System;
using System.Diagnostics;
using System.Reflection;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001E RID: 30
	public static class Globals
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00004B6C File Offset: 0x00002D6C
		static Globals()
		{
			Globals.XdeVersionLong = FileVersionInfo.GetVersionInfo(typeof(Globals).Assembly.Location).ProductVersion;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004BD3 File Offset: 0x00002DD3
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00004BDA File Offset: 0x00002DDA
		public static string XdeVersionLong { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004BE2 File Offset: 0x00002DE2
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00004BE9 File Offset: 0x00002DE9
		public static Version XdeVersion { get; private set; } = AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version;

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004BF1 File Offset: 0x00002DF1
		public static string XdeVersionShort
		{
			get
			{
				return Globals.XdeVersion.ToString(2);
			}
		}

		// Token: 0x040000D1 RID: 209
		public const string XdeAppDataPath = "Microsoft\\XDE";

		// Token: 0x040000D2 RID: 210
		public const string NatInstanceName = "Microsoft Emulator Nat Instance";

		// Token: 0x040000D3 RID: 211
		public static readonly Guid XdeServicesId = new Guid("01690B1B-FD75-4719-A789-4A6D8C0A7D10");

		// Token: 0x040000D4 RID: 212
		public static readonly Guid XdeExternalMonitorId = new Guid("90280B7E-2016-4BA6-AFF4-688EE255A032");
	}
}
