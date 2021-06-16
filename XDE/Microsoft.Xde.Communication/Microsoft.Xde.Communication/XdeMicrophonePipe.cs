using System;
using System.ComponentModel;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Communication
{
	// Token: 0x02000008 RID: 8
	public class XdeMicrophonePipe : XdePipe, IXdeMicrophonePipe, IXdeAutomationMicrophonePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdePipe, IXdeConnectionController, IDisposable
	{
		// Token: 0x0600005B RID: 91 RVA: 0x00002D1A File Offset: 0x00000F1A
		protected XdeMicrophonePipe(IXdeConnectionAddressInfo addressInfo) : base(addressInfo, XdeMicrophonePipe.microphoneGuid)
		{
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002D28 File Offset: 0x00000F28
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002D30 File Offset: 0x00000F30
		public bool AutomationOverrideEnabled { get; set; }

		// Token: 0x0600005E RID: 94 RVA: 0x00002D39 File Offset: 0x00000F39
		public static IXdeMicrophonePipe XdeMicrophonePipeFactory(IXdeConnectionAddressInfo addressInfo)
		{
			return new XdeMicrophonePipe(addressInfo);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002D41 File Offset: 0x00000F41
		public void SendMicrophoneDataToGuest(byte[] data, int size)
		{
			if (!this.AutomationOverrideEnabled)
			{
				this.SendMicrophoneDataToGuestInternal(data, size);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002D53 File Offset: 0x00000F53
		public void SendMicrophoneAutomationDataToGuest(byte[] data, int size)
		{
			if (this.AutomationOverrideEnabled)
			{
				this.SendMicrophoneDataToGuestInternal(data, size);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002D65 File Offset: 0x00000F65
		private void SendMicrophoneDataToGuestInternal(byte[] data, int size)
		{
			if (size > data.Length)
			{
				size = data.Length;
			}
			base.SendToGuest(size);
			base.SendToGuest(data, size);
		}

		// Token: 0x04000014 RID: 20
		private static readonly Guid microphoneGuid = new Guid("{942AA0B2-D167-46A4-9C4B-EB634F09C6B5}");
	}
}
