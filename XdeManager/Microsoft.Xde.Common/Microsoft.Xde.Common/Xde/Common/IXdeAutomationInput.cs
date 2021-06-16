using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows.Forms;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000032 RID: 50
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IXdeAutomationInput : IXdeAutomationPipe, INotifyPropertyChanged
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600012A RID: 298
		// (set) Token: 0x0600012B RID: 299
		TouchMode TouchMode { [OperationContract] get; [OperationContract] set; }

		// Token: 0x0600012C RID: 300
		[OperationContract]
		void SendTouchInfo(NativeMethods.POINTER_TYPE_INFO[] touchInputs);

		// Token: 0x0600012D RID: 301
		[OperationContract]
		void SendText(string text);

		// Token: 0x0600012E RID: 302
		[OperationContract]
		void SendKeyboardEvent(short scanCode, Keys virtualKey, bool down);

		// Token: 0x0600012F RID: 303
		[OperationContract]
		void SendKeyboardKeys(Keys[] keys, KeyboardKeyOption options);

		// Token: 0x06000130 RID: 304
		[OperationContract]
		void SendMouseEvent(MouseArgsInfo mouseArgs);

		// Token: 0x06000131 RID: 305
		[OperationContract]
		void PressSkinButton(SkinButtonType type);
	}
}
