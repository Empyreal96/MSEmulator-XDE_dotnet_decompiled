using System;

namespace System.Management.Automation.Provider
{
	// Token: 0x02000471 RID: 1137
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class CmdletProviderAttribute : Attribute
	{
		// Token: 0x060032A2 RID: 12962 RVA: 0x00113FA4 File Offset: 0x001121A4
		public CmdletProviderAttribute(string providerName, ProviderCapabilities providerCapabilities)
		{
			if (string.IsNullOrEmpty(providerName))
			{
				throw PSTraceSource.NewArgumentNullException("providerName");
			}
			if (providerName.IndexOfAny(this.illegalCharacters) != -1)
			{
				throw PSTraceSource.NewArgumentException("providerName", SessionStateStrings.ProviderNameNotValid, new object[]
				{
					providerName
				});
			}
			this.provider = providerName;
			this.providerCapabilities = providerCapabilities;
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x060032A3 RID: 12963 RVA: 0x00114025 File Offset: 0x00112225
		public string ProviderName
		{
			get
			{
				return this.provider;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x060032A4 RID: 12964 RVA: 0x0011402D File Offset: 0x0011222D
		public ProviderCapabilities ProviderCapabilities
		{
			get
			{
				return this.providerCapabilities;
			}
		}

		// Token: 0x04001A79 RID: 6777
		private char[] illegalCharacters = new char[]
		{
			':',
			'\\',
			'[',
			']',
			'?',
			'*'
		};

		// Token: 0x04001A7A RID: 6778
		private string provider = string.Empty;

		// Token: 0x04001A7B RID: 6779
		private ProviderCapabilities providerCapabilities;
	}
}
