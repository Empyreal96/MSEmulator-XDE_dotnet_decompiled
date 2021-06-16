using System;

namespace Microsoft.Xde.LocSimulator
{
	// Token: 0x02000003 RID: 3
	public class LocationSimulator
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002276 File Offset: 0x00000476
		// (set) Token: 0x06000007 RID: 7 RVA: 0x0000227E File Offset: 0x0000047E
		public Position CurrentPosition { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002287 File Offset: 0x00000487
		// (set) Token: 0x06000009 RID: 9 RVA: 0x0000228F File Offset: 0x0000048F
		public Position CurrentSimulatedPosition { get; private set; }

		// Token: 0x0600000A RID: 10 RVA: 0x00002298 File Offset: 0x00000498
		public LocationSimulator(LocationSimulator.Profile profile)
		{
			this.Reset(profile);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022A7 File Offset: 0x000004A7
		public void Reset(LocationSimulator.Profile profile)
		{
			this.currentProfile = profile;
			this.CurrentPosition = null;
			this.CurrentSimulatedPosition = null;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022C0 File Offset: 0x000004C0
		public void Next(Position point)
		{
			if (point == null)
			{
				return;
			}
			if (this.CurrentPosition == null)
			{
				this.CurrentPosition = point;
			}
			double speed = GeoMath.HaversineDistance(this.CurrentPosition, point) * 1000.0;
			double heading = GeoMath.BearingAngle(this.CurrentPosition, point);
			this.CurrentSimulatedPosition = new Position(point.Latitude, point.Longitude, 10.0, speed, heading, PositionSource.GNSS);
			this.CurrentPosition = this.CurrentSimulatedPosition;
		}

		// Token: 0x04000002 RID: 2
		private LocationSimulator.Profile currentProfile;

		// Token: 0x02000007 RID: 7
		public enum Profile
		{
			// Token: 0x04000015 RID: 21
			PIN_POINT_LOCATION_PROFILE,
			// Token: 0x04000016 RID: 22
			URBAN_LOCATION_PROFILE,
			// Token: 0x04000017 RID: 23
			RURAL_LOCATION_PROFILE,
			// Token: 0x04000018 RID: 24
			SUBURB_LOCATION_PROFILE
		}
	}
}
