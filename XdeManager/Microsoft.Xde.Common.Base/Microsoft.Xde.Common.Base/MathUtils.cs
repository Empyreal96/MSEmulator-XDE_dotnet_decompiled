using System;
using System.Drawing;

namespace Microsoft.Xde.Common
{
	// Token: 0x0200000B RID: 11
	public static class MathUtils
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00002F76 File Offset: 0x00001176
		public static T Clamp<T>(T value, T min, T max) where T : IComparable
		{
			if (value.CompareTo(min) < 0)
			{
				return min;
			}
			if (value.CompareTo(max) > 0)
			{
				return max;
			}
			return value;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002FA9 File Offset: 0x000011A9
		public static int ScaleInt(int value, float scale)
		{
			return (int)Math.Round((double)value * (double)scale);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002FB8 File Offset: 0x000011B8
		public static SizeF GetTriangleSidesUsingNewDiagAndOldSides(float newDiag, SizeF oldSides)
		{
			float num = (float)Math.Sqrt((double)(oldSides.Width * oldSides.Width + oldSides.Height * oldSides.Height));
			float num2 = newDiag * oldSides.Height / num;
			return new SizeF(oldSides.Width / oldSides.Height * num2, num2);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000300F File Offset: 0x0000120F
		public static float ConvertFromInchesToMillimeters(float inches)
		{
			return inches * 25.4f;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003018 File Offset: 0x00001218
		public static float ConvertFromInchesToCentimeters(float inches)
		{
			return MathUtils.ConvertFromInchesToMillimeters(inches) / 10f;
		}
	}
}
