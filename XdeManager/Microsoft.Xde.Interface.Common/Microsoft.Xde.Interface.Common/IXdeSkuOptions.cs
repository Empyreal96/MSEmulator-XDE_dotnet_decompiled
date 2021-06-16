using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001A RID: 26
	public interface IXdeSkuOptions : IDisposable
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600008E RID: 142
		bool NATDisabled { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600008F RID: 143
		bool WindowsKeyEnabled { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000090 RID: 144
		bool WindowsKeyCombinationsEnabled { get; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000091 RID: 145
		int DefaultMemSize { get; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000092 RID: 146
		int ProcessorCount { get; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000093 RID: 147
		bool WriteVhdBootSettingsDisabled { get; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000094 RID: 148
		string ValidSensors { get; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000095 RID: 149
		IXdeGuestDisplay GuestDisplay { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000096 RID: 150
		bool HostCursorDisabledInMouseMode { get; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000097 RID: 151
		TouchMode InputMode { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000098 RID: 152
		bool UseHCSIfAvailable { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000099 RID: 153
		bool ShowGuestDisplayASAP { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600009A RID: 154
		GpuAssignmentMode GpuAssignmentMode { get; }
	}
}
