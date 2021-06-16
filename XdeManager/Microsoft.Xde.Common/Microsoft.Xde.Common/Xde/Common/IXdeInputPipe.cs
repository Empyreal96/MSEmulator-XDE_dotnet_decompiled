using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000031 RID: 49
	public interface IXdeInputPipe : IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable
	{
		// Token: 0x06000127 RID: 295
		void SendTouchInfo(NativeMethods.POINTER_TYPE_INFO[] touchInfo);

		// Token: 0x06000128 RID: 296
		void SendText(string text);

		// Token: 0x06000129 RID: 297
		void SendKeyboardEvent(short scanCode, Keys virtualKey, bool down);
	}
}
