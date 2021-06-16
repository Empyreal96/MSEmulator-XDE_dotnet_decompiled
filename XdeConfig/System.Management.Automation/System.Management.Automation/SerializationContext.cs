using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000447 RID: 1095
	internal class SerializationContext
	{
		// Token: 0x06002FD1 RID: 12241 RVA: 0x00105514 File Offset: 0x00103714
		internal SerializationContext() : this(2, true)
		{
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x0010551E File Offset: 0x0010371E
		internal SerializationContext(int depth, bool useDepthFromTypes) : this(depth, (useDepthFromTypes ? SerializationOptions.UseDepthFromTypes : SerializationOptions.None) | SerializationOptions.PreserveSerializationSettingOfOriginal, null)
		{
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x00105534 File Offset: 0x00103734
		internal SerializationContext(int depth, SerializationOptions options, PSRemotingCryptoHelper cryptoHelper)
		{
			if (depth < 1)
			{
				throw PSTraceSource.NewArgumentException("writer", Serialization.DepthOfOneRequired, new object[0]);
			}
			this.depth = depth;
			this.options = options;
			this.cryptoHelper = cryptoHelper;
		}

		// Token: 0x040019E0 RID: 6624
		private const int DefaultSerializationDepth = 2;

		// Token: 0x040019E1 RID: 6625
		internal readonly int depth;

		// Token: 0x040019E2 RID: 6626
		internal readonly SerializationOptions options;

		// Token: 0x040019E3 RID: 6627
		internal readonly PSRemotingCryptoHelper cryptoHelper;

		// Token: 0x040019E4 RID: 6628
		internal readonly CimClassSerializationCache<CimClassSerializationId> cimClassSerializationIdCache = new CimClassSerializationCache<CimClassSerializationId>();
	}
}
