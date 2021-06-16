using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000049 RID: 73
	public interface IXdeAudioPipe : IXdeAutomationAudioPipe, IXdeAutomationPipe, INotifyPropertyChanged, IXdePipe, IXdeConnectionController, IDisposable
	{
	}
}
