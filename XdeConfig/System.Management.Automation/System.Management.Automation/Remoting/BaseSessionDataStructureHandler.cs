using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000294 RID: 660
	internal abstract class BaseSessionDataStructureHandler
	{
		// Token: 0x06001F9F RID: 8095
		internal abstract void RaiseKeyExchangeMessageReceived(RemoteDataObject<PSObject> receivedData);
	}
}
