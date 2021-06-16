using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000046 RID: 70
	public interface IXdeSDCardPipe : IXdePipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdeConnectionController, IDisposable, IXdeAutomationSDCardPipe
	{
	}
}
