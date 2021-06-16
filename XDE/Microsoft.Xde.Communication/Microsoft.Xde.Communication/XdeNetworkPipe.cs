using System;
using System.ComponentModel;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Communication
{
	// Token: 0x02000009 RID: 9
	public class XdeNetworkPipe : XdePipe, IXdeNetworkPipe, IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00002D92 File Offset: 0x00000F92
		protected XdeNetworkPipe(IXdeConnectionAddressInfo addressInfo) : base(addressInfo, XdeNetworkPipe.NetworkPipeGuid)
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public static IXdeNetworkPipe XdeNetworkPipeFactory(IXdeConnectionAddressInfo addressInfo)
		{
			return new XdeNetworkPipe(addressInfo);
		}

		// Token: 0x04000016 RID: 22
		private static readonly Guid NetworkPipeGuid = new Guid("{1CC8980D-DF03-4790-B43A-A757225B2B8C}");
	}
}
