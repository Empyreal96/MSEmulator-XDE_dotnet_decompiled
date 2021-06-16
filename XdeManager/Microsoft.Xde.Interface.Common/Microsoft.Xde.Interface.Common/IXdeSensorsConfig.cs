using System;
using System.ComponentModel;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000016 RID: 22
	public interface IXdeSensorsConfig : INotifyPropertyChanged
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600007C RID: 124
		XdeSensors EnabledStates { get; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600007D RID: 125
		// (set) Token: 0x0600007E RID: 126
		XdeSensors ValidSensors { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600007F RID: 127
		// (set) Token: 0x06000080 RID: 128
		XdeSensors RequiredSensors { get; set; }

		// Token: 0x06000081 RID: 129
		bool ApplyNewEnabledStates(XdeSensors sensorsEnabledStatesBV, bool rebootDialog = true);

		// Token: 0x06000082 RID: 130
		string GetVhdPackageContents(XdeSensors sensorsEnabled);
	}
}
