using System;
using System.Windows.Forms;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200000E RID: 14
	public interface IXdeGuestInput
	{
		// Token: 0x0600003B RID: 59
		void SendMouseEvent(MouseButtons flags, uint x, uint y);

		// Token: 0x0600003C RID: 60
		void SendKeyboardEvent(ushort scanCode, bool keyUp, bool keyRepeat, bool extended);

		// Token: 0x0600003D RID: 61
		void SendMouseWheelEvent(ushort wheelRotation);
	}
}
