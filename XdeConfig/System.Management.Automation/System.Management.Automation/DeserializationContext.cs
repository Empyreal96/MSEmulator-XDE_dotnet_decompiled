using System;
using System.Management.Automation.Internal;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x0200044B RID: 1099
	internal class DeserializationContext
	{
		// Token: 0x06002FE3 RID: 12259 RVA: 0x0010571D File Offset: 0x0010391D
		internal DeserializationContext() : this(DeserializationOptions.None, null)
		{
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x00105727 File Offset: 0x00103927
		internal DeserializationContext(DeserializationOptions options, PSRemotingCryptoHelper cryptoHelper)
		{
			this.options = options;
			this.cryptoHelper = cryptoHelper;
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x00105751 File Offset: 0x00103951
		// (set) Token: 0x06002FE5 RID: 12261 RVA: 0x00105748 File Offset: 0x00103948
		internal int? MaximumAllowedMemory
		{
			get
			{
				return this.maxAllowedMemory;
			}
			set
			{
				this.maxAllowedMemory = value;
			}
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x0010575C File Offset: 0x0010395C
		internal void LogExtraMemoryUsage(int amountOfExtraMemory)
		{
			if (amountOfExtraMemory < 0)
			{
				return;
			}
			if (this.maxAllowedMemory != null)
			{
				if (amountOfExtraMemory > this.maxAllowedMemory.Value - this.totalDataProcessedSoFar)
				{
					string message = StringUtil.Format(Serialization.DeserializationMemoryQuota, new object[]
					{
						(double)this.maxAllowedMemory.Value / 1048576.0,
						"PSMaximumReceivedObjectSizeMB",
						"PSMaximumReceivedDataSizePerCommandMB"
					});
					throw new XmlException(message);
				}
				this.totalDataProcessedSoFar += amountOfExtraMemory;
			}
		}

		// Token: 0x040019ED RID: 6637
		private int totalDataProcessedSoFar;

		// Token: 0x040019EE RID: 6638
		private int? maxAllowedMemory;

		// Token: 0x040019EF RID: 6639
		internal readonly DeserializationOptions options;

		// Token: 0x040019F0 RID: 6640
		internal readonly PSRemotingCryptoHelper cryptoHelper;

		// Token: 0x040019F1 RID: 6641
		internal static int MaxItemsInCimClassCache = 100;

		// Token: 0x040019F2 RID: 6642
		internal readonly CimClassDeserializationCache<CimClassSerializationId> cimClassSerializationIdCache = new CimClassDeserializationCache<CimClassSerializationId>();
	}
}
