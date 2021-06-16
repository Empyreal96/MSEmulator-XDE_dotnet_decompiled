using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.H2LPlugin.Orientation
{
	// Token: 0x02000010 RID: 16
	public class OrientationModeInformation
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00003C2C File Offset: 0x00001E2C
		public OrientationModeInformation(OrientationMode orientationMode, string displayName, string bitmapName, DisplayOrientation displayOrientation, MonitorMode monitorMode, OrientationConfiguration config)
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
			string uriString = string.Format("pack://application:,,,/Microsoft.Xde.SantPlugin;component/H2LOrientation/Modes/{0}", bitmapName);
			this.Source = new BitmapImage(new Uri(uriString));
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003C95 File Offset: 0x00001E95
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00003C9D File Offset: 0x00001E9D
		public OrientationMode Mode { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003CA6 File Offset: 0x00001EA6
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00003CAE File Offset: 0x00001EAE
		public string DisplayName { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003CB7 File Offset: 0x00001EB7
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00003CBF File Offset: 0x00001EBF
		public BitmapSource Source { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003CC8 File Offset: 0x00001EC8
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00003CD0 File Offset: 0x00001ED0
		public OrientationConfiguration Config { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003CD9 File Offset: 0x00001ED9
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00003CE1 File Offset: 0x00001EE1
		public DisplayOrientation DisplayOrientation { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003CEA File Offset: 0x00001EEA
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003CF2 File Offset: 0x00001EF2
		public MonitorMode MonitorMode { get; private set; }

		// Token: 0x04000041 RID: 65
		private static bool Init;
	}
}
