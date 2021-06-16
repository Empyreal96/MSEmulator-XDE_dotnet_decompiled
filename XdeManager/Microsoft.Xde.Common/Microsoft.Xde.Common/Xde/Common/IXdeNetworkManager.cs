using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200003D RID: 61
	public interface IXdeNetworkManager : IDisposable
	{
		// Token: 0x06000172 RID: 370
		void PreInitialize();

		// Token: 0x06000173 RID: 371
		bool TryAquireNecessaryPermissions();

		// Token: 0x06000174 RID: 372
		bool InitializeNetworkConfig();

		// Token: 0x06000175 RID: 373
		void UpdateGuestNetworkProperties(IXdeSimpleCommandsPipe simpleCommandsPipe);
	}
}
