using System;

namespace DiscUtils.Streams
{
	// Token: 0x02000033 RID: 51
	internal static class Numbers<T> where T : struct, IComparable<T>, IEquatable<T>
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x000068B0 File Offset: 0x00004AB0
		public static bool GreaterThan(T a, T b)
		{
			return a.CompareTo(b) > 0;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000068C3 File Offset: 0x00004AC3
		public static bool GreaterThanOrEqual(T a, T b)
		{
			return a.CompareTo(b) >= 0;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000068D9 File Offset: 0x00004AD9
		public static bool LessThan(T a, T b)
		{
			return a.CompareTo(b) < 0;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000068EC File Offset: 0x00004AEC
		public static bool LessThanOrEqual(T a, T b)
		{
			return a.CompareTo(b) <= 0;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00006902 File Offset: 0x00004B02
		public static bool Equal(T a, T b)
		{
			return a.CompareTo(b) == 0;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00006915 File Offset: 0x00004B15
		public static bool NotEqual(T a, T b)
		{
			return a.CompareTo(b) != 0;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00006928 File Offset: 0x00004B28
		private static T GetOne()
		{
			if (typeof(T) == typeof(long))
			{
				return ((Numbers<T>.NoParamFn)(() => 1L))();
			}
			if (typeof(T) == typeof(int))
			{
				return ((Numbers<T>.NoParamFn)(() => 1))();
			}
			throw new NotSupportedException();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000069C4 File Offset: 0x00004BC4
		private static Numbers<T>.ConvertLongFn GetConvertLong()
		{
			if (typeof(T) == typeof(long))
			{
				return (Numbers<T>.ConvertLongFn)((long x) => x);
			}
			if (typeof(T) == typeof(int))
			{
				return (Numbers<T>.ConvertLongFn)((long x) => (int)x);
			}
			throw new NotSupportedException();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00006A58 File Offset: 0x00004C58
		private static Numbers<T>.ConvertIntFn GetConvertInt()
		{
			if (typeof(T) == typeof(long))
			{
				return (Numbers<T>.ConvertIntFn)((int x) => (long)x);
			}
			if (typeof(T) == typeof(int))
			{
				return (Numbers<T>.ConvertIntFn)((int x) => x);
			}
			throw new NotSupportedException();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00006AEC File Offset: 0x00004CEC
		private static Numbers<T>.DualParamFn GetAdd()
		{
			if (typeof(T) == typeof(long))
			{
				return (Numbers<T>.DualParamFn)((long a, long b) => a + b);
			}
			if (typeof(T) == typeof(int))
			{
				return (Numbers<T>.DualParamFn)((int a, int b) => a + b);
			}
			throw new NotSupportedException();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00006B80 File Offset: 0x00004D80
		private static Numbers<T>.DualParamFn GetSubtract()
		{
			if (typeof(T) == typeof(long))
			{
				return (Numbers<T>.DualParamFn)((long a, long b) => a - b);
			}
			if (typeof(T) == typeof(int))
			{
				return (Numbers<T>.DualParamFn)((int a, int b) => a - b);
			}
			throw new NotSupportedException();
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00006C14 File Offset: 0x00004E14
		private static Numbers<T>.DualParamFn GetMultiply()
		{
			if (typeof(T) == typeof(long))
			{
				return (Numbers<T>.DualParamFn)((long a, long b) => a * b);
			}
			if (typeof(T) == typeof(int))
			{
				return (Numbers<T>.DualParamFn)((int a, int b) => a * b);
			}
			throw new NotSupportedException();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00006CA8 File Offset: 0x00004EA8
		private static Numbers<T>.DualParamFn GetDivide()
		{
			if (typeof(T) == typeof(long))
			{
				return (Numbers<T>.DualParamFn)((long a, long b) => a / b);
			}
			if (typeof(T) == typeof(int))
			{
				return (Numbers<T>.DualParamFn)((int a, int b) => a / b);
			}
			throw new NotSupportedException();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00006D3C File Offset: 0x00004F3C
		private static Numbers<T>.DualParamFn GetRoundUp()
		{
			if (typeof(T) == typeof(long))
			{
				return (Numbers<T>.DualParamFn)((long a, long b) => (a + b - 1L) / b * b);
			}
			if (typeof(T) == typeof(int))
			{
				return (Numbers<T>.DualParamFn)((int a, int b) => (a + b - 1) / b * b);
			}
			throw new NotSupportedException();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00006DD0 File Offset: 0x00004FD0
		private static Numbers<T>.DualParamFn GetRoundDown()
		{
			if (typeof(T) == typeof(long))
			{
				return (Numbers<T>.DualParamFn)((long a, long b) => a / b * b);
			}
			if (typeof(T) == typeof(int))
			{
				return (Numbers<T>.DualParamFn)((int a, int b) => a / b * b);
			}
			throw new NotSupportedException();
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00006E64 File Offset: 0x00005064
		private static Numbers<T>.DualParamFn GetCeil()
		{
			if (typeof(T) == typeof(long))
			{
				return (Numbers<T>.DualParamFn)((long a, long b) => (a + b - 1L) / b);
			}
			if (typeof(T) == typeof(int))
			{
				return (Numbers<T>.DualParamFn)((int a, int b) => (a + b - 1) / b);
			}
			throw new NotSupportedException();
		}

		// Token: 0x04000077 RID: 119
		public static readonly T Zero = default(T);

		// Token: 0x04000078 RID: 120
		public static readonly T One = Numbers<T>.GetOne();

		// Token: 0x04000079 RID: 121
		public static readonly Numbers<T>.DualParamFn Add = Numbers<T>.GetAdd();

		// Token: 0x0400007A RID: 122
		public static readonly Numbers<T>.DualParamFn Subtract = Numbers<T>.GetSubtract();

		// Token: 0x0400007B RID: 123
		public static readonly Numbers<T>.DualParamFn Multiply = Numbers<T>.GetMultiply();

		// Token: 0x0400007C RID: 124
		public static readonly Numbers<T>.DualParamFn Divide = Numbers<T>.GetDivide();

		// Token: 0x0400007D RID: 125
		public static readonly Numbers<T>.DualParamFn RoundUp = Numbers<T>.GetRoundUp();

		// Token: 0x0400007E RID: 126
		public static readonly Numbers<T>.DualParamFn RoundDown = Numbers<T>.GetRoundDown();

		// Token: 0x0400007F RID: 127
		public static readonly Numbers<T>.DualParamFn Ceil = Numbers<T>.GetCeil();

		// Token: 0x04000080 RID: 128
		public static readonly Numbers<T>.ConvertLongFn ConvertLong = Numbers<T>.GetConvertLong();

		// Token: 0x04000081 RID: 129
		public static readonly Numbers<T>.ConvertIntFn ConvertInt = Numbers<T>.GetConvertInt();

		// Token: 0x0200004F RID: 79
		// (Invoke) Token: 0x060002C5 RID: 709
		public delegate bool ComparisonFn(T a, T b);

		// Token: 0x02000050 RID: 80
		// (Invoke) Token: 0x060002C9 RID: 713
		public delegate T ConvertIntFn(int a);

		// Token: 0x02000051 RID: 81
		// (Invoke) Token: 0x060002CD RID: 717
		public delegate T ConvertLongFn(long a);

		// Token: 0x02000052 RID: 82
		// (Invoke) Token: 0x060002D1 RID: 721
		public delegate T DualParamFn(T a, T b);

		// Token: 0x02000053 RID: 83
		// (Invoke) Token: 0x060002D5 RID: 725
		public delegate T NoParamFn();

		// Token: 0x02000054 RID: 84
		// (Invoke) Token: 0x060002D9 RID: 729
		private delegate long LongNoParamFn();

		// Token: 0x02000055 RID: 85
		// (Invoke) Token: 0x060002DD RID: 733
		private delegate long LongDualParamFn(long a, long b);

		// Token: 0x02000056 RID: 86
		// (Invoke) Token: 0x060002E1 RID: 737
		private delegate long LongConvertLongFn(long x);

		// Token: 0x02000057 RID: 87
		// (Invoke) Token: 0x060002E5 RID: 741
		private delegate long LongConvertIntFn(int x);

		// Token: 0x02000058 RID: 88
		// (Invoke) Token: 0x060002E9 RID: 745
		private delegate int IntNoParamFn();

		// Token: 0x02000059 RID: 89
		// (Invoke) Token: 0x060002ED RID: 749
		private delegate int IntDualParamFn(int a, int b);

		// Token: 0x0200005A RID: 90
		// (Invoke) Token: 0x060002F1 RID: 753
		private delegate int IntConvertLongFn(long x);

		// Token: 0x0200005B RID: 91
		// (Invoke) Token: 0x060002F5 RID: 757
		private delegate int IntConvertIntFn(int x);
	}
}
