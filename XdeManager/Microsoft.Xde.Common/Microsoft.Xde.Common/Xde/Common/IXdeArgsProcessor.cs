using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000026 RID: 38
	[ComVisible(false)]
	public interface IXdeArgsProcessor
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000DD RID: 221
		string VirtualMachineName { get; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000DE RID: 222
		string VhdPath { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000DF RID: 223
		string OriginalVideoResolution { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000E0 RID: 224
		string VideoResolution { get; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000E1 RID: 225
		bool ShowUsage { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000E2 RID: 226
		string Language { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000E3 RID: 227
		string BootLanguage { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000E4 RID: 228
		string ScreenDiagonalSize { get; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000E5 RID: 229
		bool ShowName { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000E6 RID: 230
		bool NoStart { get; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000E7 RID: 231
		bool FastShutdown { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000E8 RID: 232
		int? MemSize { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000E9 RID: 233
		string Com1PipeName { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000EA RID: 234
		string Com2PipeName { get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000EB RID: 235
		bool ReuseExistingDiffDisk { get; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000EC RID: 236
		string DiffDiskVhd { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000ED RID: 237
		bool SilentUi { get; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000EE RID: 238
		bool SilentSnapshot { get; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000EF RID: 239
		bool WaitForClientConnection { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000F0 RID: 240
		bool BootToSnapshot { get; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000F1 RID: 241
		bool UseWmi { get; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000F2 RID: 242
		bool DisableStateSep { get; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000F3 RID: 243
		string DisplayName { get; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000F4 RID: 244
		string AddUserToHyperVAdmins { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000F5 RID: 245
		string AddUserToPerformanceLogUsersGroup { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000F6 RID: 246
		XdeSensors SensorsEnabled { get; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000F7 RID: 247
		string Sku { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000F8 RID: 248
		bool AutomateFeatures { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000F9 RID: 249
		int PipeTimeout { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000FA RID: 250
		bool ServiceDebugEnabled { get; }

		// Token: 0x060000FB RID: 251
		void ParseArgs(string[] args);

		// Token: 0x060000FC RID: 252
		void LoadRegistryOverrides();
	}
}
