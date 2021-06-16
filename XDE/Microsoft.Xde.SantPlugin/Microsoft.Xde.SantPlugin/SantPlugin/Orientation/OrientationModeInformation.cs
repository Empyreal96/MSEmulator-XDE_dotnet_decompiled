using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.SantPlugin.Orientation
{
	// Token: 0x02000021 RID: 33
	public class OrientationModeInformation
	{
		// Token: 0x06000155 RID: 341 RVA: 0x000067B4 File Offset: 0x000049B4
		public OrientationModeInformation(OrientationMode orientationMode, string displayName, string bitmapName, DisplayOrientation displayOrientation, MonitorMode monitorMode, OrientationMode[] modes, OrientationConfiguration config)
		{
			if (!OrientationModeInformation.Init)
			{
				new UserControl();
				OrientationModeInformation.Init = true;
			}
			this.Mode = orientationMode;
			this.DisplayName = displayName;
			this.Config = config;
			this.DisplayOrientation = displayOrientation;
			this.MonitorMode = monitorMode;
			string uriString = string.Format("pack://application:,,,/Microsoft.Xde.SantPlugin;component/Orientation/Modes/{0}", bitmapName);
			this.Source = new BitmapImage(new Uri(uriString));
			int num = Array.IndexOf<OrientationMode>(modes, orientationMode);
			int num2 = num + 1;
			if (num2 >= modes.Length)
			{
				num2 = 0;
			}
			int num3 = num - 1;
			if (num3 < 0)
			{
				num3 = modes.Length - 1;
			}
			this.LeftMode = modes[num2];
			this.RightMode = modes[num3];
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00006854 File Offset: 0x00004A54
		// (set) Token: 0x06000157 RID: 343 RVA: 0x0000685C File Offset: 0x00004A5C
		public OrientationMode Mode { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00006865 File Offset: 0x00004A65
		// (set) Token: 0x06000159 RID: 345 RVA: 0x0000686D File Offset: 0x00004A6D
		public string DisplayName { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00006876 File Offset: 0x00004A76
		// (set) Token: 0x0600015B RID: 347 RVA: 0x0000687E File Offset: 0x00004A7E
		public BitmapSource Source { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00006887 File Offset: 0x00004A87
		// (set) Token: 0x0600015D RID: 349 RVA: 0x0000688F File Offset: 0x00004A8F
		public OrientationConfiguration Config { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00006898 File Offset: 0x00004A98
		// (set) Token: 0x0600015F RID: 351 RVA: 0x000068A0 File Offset: 0x00004AA0
		public DisplayOrientation DisplayOrientation { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000160 RID: 352 RVA: 0x000068A9 File Offset: 0x00004AA9
		// (set) Token: 0x06000161 RID: 353 RVA: 0x000068B1 File Offset: 0x00004AB1
		public MonitorMode MonitorMode { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000162 RID: 354 RVA: 0x000068BA File Offset: 0x00004ABA
		// (set) Token: 0x06000163 RID: 355 RVA: 0x000068C2 File Offset: 0x00004AC2
		public OrientationMode LeftMode { get; private set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000164 RID: 356 RVA: 0x000068CB File Offset: 0x00004ACB
		// (set) Token: 0x06000165 RID: 357 RVA: 0x000068D3 File Offset: 0x00004AD3
		public OrientationMode RightMode { get; private set; }

		// Token: 0x040000BE RID: 190
		private static bool Init;
	}
}
