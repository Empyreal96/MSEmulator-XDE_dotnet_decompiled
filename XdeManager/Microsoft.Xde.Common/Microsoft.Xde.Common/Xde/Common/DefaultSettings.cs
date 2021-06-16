using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200001C RID: 28
	public static class DefaultSettings
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00004AAD File Offset: 0x00002CAD
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00004ABD File Offset: 0x00002CBD
		public static bool NATDisabled
		{
			get
			{
				return DefaultSettings.NatDisabledInRegistry || DefaultSettings.disableNAT;
			}
			set
			{
				DefaultSettings.disableNAT = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004AC5 File Offset: 0x00002CC5
		public static bool DefaultSwitchDisabled
		{
			get
			{
				return DefaultSettings.DefaultSwitchDisabledInRegistry;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004ACC File Offset: 0x00002CCC
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00004AD3 File Offset: 0x00002CD3
		public static bool UseDefaultSwitch { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004ADB File Offset: 0x00002CDB
		public static bool GpuDisabledForXde
		{
			get
			{
				return RegistryHelper.GetValue<int>("DisableGpu", 0) == 1;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004AEB File Offset: 0x00002CEB
		private static bool NatDisabledInRegistry
		{
			get
			{
				if (DefaultSettings.natDisabledRegistry == null)
				{
					DefaultSettings.natDisabledRegistry = new bool?(RegistryHelper.GetValue<int>("DisableNAT", 0) == 1);
				}
				return DefaultSettings.natDisabledRegistry.Value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004B1B File Offset: 0x00002D1B
		private static bool DefaultSwitchDisabledInRegistry
		{
			get
			{
				if (DefaultSettings.defaultSwitchDisabledRegistry == null)
				{
					DefaultSettings.defaultSwitchDisabledRegistry = new bool?(RegistryHelper.GetValue<int>("DisableDefaultSwitch", 0) == 1);
				}
				return DefaultSettings.defaultSwitchDisabledRegistry.Value;
			}
		}

		// Token: 0x040000C9 RID: 201
		public const int DefaultGuestUDPPort = 3553;

		// Token: 0x040000CA RID: 202
		public const string DefaultVMName = "Default Emulator";

		// Token: 0x040000CB RID: 203
		public const string DefaultSnapshotName = "Default Emulator Checkpoint";

		// Token: 0x040000CC RID: 204
		private static bool disableNAT;

		// Token: 0x040000CD RID: 205
		private static bool? natDisabledRegistry;

		// Token: 0x040000CE RID: 206
		private static bool? defaultSwitchDisabledRegistry;
	}
}
