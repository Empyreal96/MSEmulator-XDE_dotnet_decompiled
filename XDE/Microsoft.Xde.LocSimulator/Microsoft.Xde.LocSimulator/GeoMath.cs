using System;

namespace Microsoft.Xde.LocSimulator
{
	// Token: 0x02000002 RID: 2
	public static class GeoMath
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public static double HaversineDistance(Position point1, Position point2)
		{
			double num = GeoMath.ToRadians(point2.Latitude - point1.Latitude);
			double num2 = GeoMath.ToRadians(point2.Longitude - point1.Longitude);
			double d = GeoMath.ToRadians(point1.Latitude);
			double d2 = GeoMath.ToRadians(point2.Latitude);
			double num3 = Math.Sin(num / 2.0) * Math.Sin(num / 2.0) + Math.Sin(num2 / 2.0) * Math.Sin(num2 / 2.0) * Math.Cos(d) * Math.Cos(d2);
			return 2.0 * Math.Atan2(Math.Sqrt(num3), Math.Sqrt(1.0 - num3)) * 6371.0;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002118 File Offset: 0x00000318
		public static double BearingAngle(Position point1, Position point2)
		{
			double num = GeoMath.ToRadians(point2.Longitude - point1.Longitude);
			double num2 = GeoMath.ToRadians(point1.Latitude);
			double num3 = GeoMath.ToRadians(point2.Latitude);
			double y = Math.Sin(num) * Math.Cos(num3);
			double x = Math.Cos(num2) * Math.Sin(num3) - Math.Sin(num2) * Math.Cos(num3) * Math.Cos(num);
			return GeoMath.ToDegrees(Math.Atan2(y, x));
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000218C File Offset: 0x0000038C
		public static Position TraverseCircleArc(Position point, double bearing, double distance)
		{
			bearing = GeoMath.ToRadians(bearing);
			double num = GeoMath.ToRadians(point.Latitude);
			double num2 = GeoMath.ToRadians(point.Longitude);
			double num3 = Math.Asin(Math.Sin(num) * Math.Cos(distance / 6371.0) + Math.Cos(num) * Math.Sin(distance / 6371.0) * Math.Cos(bearing));
			double radians = num2 + Math.Atan2(Math.Sin(bearing) * Math.Sin(distance / 6371.0) * Math.Cos(num), Math.Cos(distance / 6371.0) - Math.Sin(num) * Math.Sin(num3));
			return new Position(GeoMath.ToDegrees(num3), GeoMath.ToDegrees(radians));
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002248 File Offset: 0x00000448
		private static double ToRadians(double degrees)
		{
			return 3.141592653589793 * degrees / 180.0;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000225F File Offset: 0x0000045F
		private static double ToDegrees(double radians)
		{
			return 180.0 * radians / 3.141592653589793;
		}

		// Token: 0x04000001 RID: 1
		public const double RadiusOfEarth = 6371.0;
	}
}
