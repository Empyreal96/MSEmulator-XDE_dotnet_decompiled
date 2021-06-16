using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000011 RID: 17
	[ComVisible(false)]
	public interface IXdePipe : IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable
	{
		// Token: 0x06000064 RID: 100
		void SetMinimumReceiveBufferSize(int bufferSize);

		// Token: 0x06000065 RID: 101
		void SendToGuest(byte[] data);

		// Token: 0x06000066 RID: 102
		void SendToGuest(byte[] data, int size);

		// Token: 0x06000067 RID: 103
		void SendToGuest(int[] data);

		// Token: 0x06000068 RID: 104
		void SendToGuest(int data);

		// Token: 0x06000069 RID: 105
		void SendToGuest(string stringValue);

		// Token: 0x0600006A RID: 106
		void SendToGuest(Guid guidValue);

		// Token: 0x0600006B RID: 107
		void ReceiveFromGuest(byte[] data);

		// Token: 0x0600006C RID: 108
		void ReceiveFromGuest(byte[] data, int offset, int receiveSize);

		// Token: 0x0600006D RID: 109
		void ReceiveFromGuest(byte[] data, int receiveSize);

		// Token: 0x0600006E RID: 110
		int ReceiveFromGuestWithSpin(byte[] data, int minimumSize, int maximumSize, int spinMilliseconds);

		// Token: 0x0600006F RID: 111
		void ReceiveFromGuest(int[] data);

		// Token: 0x06000070 RID: 112
		int ReceiveIntFromGuest();

		// Token: 0x06000071 RID: 113
		Guid ReceiveGuidFromGuest();

		// Token: 0x06000072 RID: 114
		T ReceiveStructFromGuest<T>();
	}
}
