using System;
using System.ComponentModel;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Communication
{
	// Token: 0x02000006 RID: 6
	public class XdeAudioPipe : XdePipe, IXdeAudioPipe, IXdeAutomationAudioPipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdePipe, IXdeConnectionController, IDisposable
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00002919 File Offset: 0x00000B19
		protected XdeAudioPipe(IXdeConnectionAddressInfo addressInfo) : base(addressInfo, XdeAudioPipe.audioGuid)
		{
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002927 File Offset: 0x00000B27
		// (set) Token: 0x06000048 RID: 72 RVA: 0x0000292F File Offset: 0x00000B2F
		private byte[] Buffer { get; set; }

		// Token: 0x06000049 RID: 73 RVA: 0x00002938 File Offset: 0x00000B38
		public static IXdeAudioPipe XdeAudioPipeFactory(IXdeConnectionAddressInfo addressInfo)
		{
			return new XdeAudioPipe(addressInfo);
		}

		// Token: 0x0400000B RID: 11
		private static readonly Guid audioGuid = new Guid("{9DDEAB9F-ED2B-450B-B8E5-03F0E20A3555}");
	}
}
