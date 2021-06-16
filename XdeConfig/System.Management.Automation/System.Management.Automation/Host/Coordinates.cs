using System;
using System.Globalization;

namespace System.Management.Automation.Host
{
	// Token: 0x0200021E RID: 542
	public struct Coordinates
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060019A7 RID: 6567 RVA: 0x0009A620 File Offset: 0x00098820
		// (set) Token: 0x060019A8 RID: 6568 RVA: 0x0009A628 File Offset: 0x00098828
		public int X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060019A9 RID: 6569 RVA: 0x0009A631 File Offset: 0x00098831
		// (set) Token: 0x060019AA RID: 6570 RVA: 0x0009A639 File Offset: 0x00098839
		public int Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x0009A642 File Offset: 0x00098842
		public Coordinates(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x0009A654 File Offset: 0x00098854
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0},{1}", new object[]
			{
				this.X,
				this.Y
			});
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0009A694 File Offset: 0x00098894
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is Coordinates)
			{
				result = (this == (Coordinates)obj);
			}
			return result;
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x0009A6C0 File Offset: 0x000988C0
		public override int GetHashCode()
		{
			ulong num;
			if (this.X < 0)
			{
				if (this.X == -2147483648)
				{
					num = (ulong)((long)(-1 * (this.X + 1)));
				}
				else
				{
					num = (ulong)((long)(-(long)this.X));
				}
			}
			else
			{
				num = (ulong)((long)this.X);
			}
			num *= 4294967296UL;
			if (this.Y < 0)
			{
				if (this.Y == -2147483648)
				{
					num += (ulong)((long)(-1 * (this.Y + 1)));
				}
				else
				{
					num += (ulong)((long)(-(long)this.Y));
				}
			}
			else
			{
				num += (ulong)((long)this.Y);
			}
			return num.GetHashCode();
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x0009A75C File Offset: 0x0009895C
		public static bool operator ==(Coordinates first, Coordinates second)
		{
			return first.X == second.X && first.Y == second.Y;
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x0009A78E File Offset: 0x0009898E
		public static bool operator !=(Coordinates first, Coordinates second)
		{
			return !(first == second);
		}

		// Token: 0x04000A84 RID: 2692
		private int x;

		// Token: 0x04000A85 RID: 2693
		private int y;
	}
}
