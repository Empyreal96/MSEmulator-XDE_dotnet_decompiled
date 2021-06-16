using System;

namespace Microsoft.Xde.LocSimulator
{
	// Token: 0x02000004 RID: 4
	public class Position
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002333 File Offset: 0x00000533
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000233B File Offset: 0x0000053B
		public double Accuracy { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002344 File Offset: 0x00000544
		// (set) Token: 0x06000010 RID: 16 RVA: 0x0000234C File Offset: 0x0000054C
		public double Latitude { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002355 File Offset: 0x00000555
		// (set) Token: 0x06000012 RID: 18 RVA: 0x0000235D File Offset: 0x0000055D
		public double Longitude { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002366 File Offset: 0x00000566
		// (set) Token: 0x06000014 RID: 20 RVA: 0x0000236E File Offset: 0x0000056E
		public double Heading { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002377 File Offset: 0x00000577
		// (set) Token: 0x06000016 RID: 22 RVA: 0x0000237F File Offset: 0x0000057F
		public double Speed { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002388 File Offset: 0x00000588
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002390 File Offset: 0x00000590
		public PositionSource Source { get; set; }

		// Token: 0x06000019 RID: 25 RVA: 0x0000239C File Offset: 0x0000059C
		public Position(double latitude, double longitude)
		{
			this.Latitude = latitude;
			this.Longitude = longitude;
			this.Accuracy = 0.0;
			this.Heading = 0.0;
			this.Speed = 0.0;
			this.Source = PositionSource.None;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000023F1 File Offset: 0x000005F1
		public Position(double latitude, double longitude, double accuracy, double speed, double heading, PositionSource source)
		{
			this.Latitude = latitude;
			this.Longitude = longitude;
			this.Accuracy = accuracy;
			this.Speed = speed;
			this.Heading = heading;
			this.Source = source;
		}
	}
}
