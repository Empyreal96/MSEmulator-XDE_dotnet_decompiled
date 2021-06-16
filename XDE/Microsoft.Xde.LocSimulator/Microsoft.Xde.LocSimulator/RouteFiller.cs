using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace Microsoft.Xde.LocSimulator
{
	// Token: 0x02000006 RID: 6
	public static class RouteFiller
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002426 File Offset: 0x00000626
		public static IEnumerable<Position> Fill(XmlDocument response, XmlNamespaceManager nameManager, double speed)
		{
			Position[] path = RouteFiller.GetPath(response, nameManager);
			double time = 0.0;
			int endKeyFramePointIndex = 0;
			foreach (object obj in response.SelectNodes("//map:ItineraryItem", nameManager))
			{
				XmlNode xmlNode = (XmlNode)obj;
				int num = endKeyFramePointIndex;
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("map:Detail", nameManager);
				int num2 = int.Parse(xmlNodeList[0].SelectSingleNode("map:StartPathIndex", nameManager).InnerText, CultureInfo.InvariantCulture);
				int num3 = int.Parse(xmlNodeList[xmlNodeList.Count - 1].SelectSingleNode("map:EndPathIndex", nameManager).InnerText, CultureInfo.InvariantCulture);
				endKeyFramePointIndex += num3 - num2;
				double num4 = double.Parse(xmlNode.SelectSingleNode("map:TravelDistance", nameManager).InnerText, CultureInfo.InvariantCulture);
				double num5 = double.Parse(xmlNode.SelectSingleNode("map:TravelDuration", nameManager).InnerText, CultureInfo.InvariantCulture);
				if (num4 != 0.0)
				{
					double speedInKilometersPerSecond = 0.0;
					if (speed <= 0.0)
					{
						if (num5 != 0.0)
						{
							speedInKilometersPerSecond = num4 / num5;
						}
						speedInKilometersPerSecond = Math.Max(speedInKilometersPerSecond, 0.011176);
					}
					else
					{
						speedInKilometersPerSecond = speed / 3600.0;
					}
					int num7;
					for (int i = num; i < endKeyFramePointIndex; i = num7 + 1)
					{
						double bearing = GeoMath.BearingAngle(path[i], path[i + 1]);
						double num6 = GeoMath.HaversineDistance(path[i], path[i + 1]);
						double timeInSeconds = num6 / speedInKilometersPerSecond;
						int steps = (int)Math.Floor(time - Math.Floor(time) + timeInSeconds);
						double initDistance = (1.0 - (time - Math.Floor(time))) * speedInKilometersPerSecond;
						for (int s = 0; s < steps; s = num7 + 1)
						{
							Position position = GeoMath.TraverseCircleArc(path[i], bearing, initDistance + (double)s * speedInKilometersPerSecond);
							yield return position;
							num7 = s;
						}
						time += timeInSeconds;
						num7 = i;
					}
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002444 File Offset: 0x00000644
		public static Position[] GetPath(XmlDocument response, XmlNamespaceManager nameManager)
		{
			XmlNodeList xmlNodeList = response.SelectNodes("//map:Point", nameManager);
			Position[] array = new Position[xmlNodeList.Count];
			int num = 0;
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				double latitude = double.Parse(xmlNode.SelectSingleNode("map:Latitude", nameManager).InnerText, CultureInfo.InvariantCulture);
				double longitude = double.Parse(xmlNode.SelectSingleNode("map:Longitude", nameManager).InnerText, CultureInfo.InvariantCulture);
				array[num++] = new Position(latitude, longitude);
			}
			return array;
		}

		// Token: 0x04000011 RID: 17
		private const int MinDrivingSpeedMph = 25;

		// Token: 0x04000012 RID: 18
		private const double MphToKps = 0.00044704;

		// Token: 0x04000013 RID: 19
		private const double MinDrivingSpeedKps = 0.011176;
	}
}
