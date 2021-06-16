using System;
using System.Globalization;

namespace System.Management.Automation.Host
{
	// Token: 0x0200021F RID: 543
	public struct Size
	{
		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060019B1 RID: 6577 RVA: 0x0009A79A File Offset: 0x0009899A
		// (set) Token: 0x060019B2 RID: 6578 RVA: 0x0009A7A2 File Offset: 0x000989A2
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x0009A7AB File Offset: 0x000989AB
		// (set) Token: 0x060019B4 RID: 6580 RVA: 0x0009A7B3 File Offset: 0x000989B3
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0009A7BC File Offset: 0x000989BC
		public Size(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0009A7CC File Offset: 0x000989CC
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0},{1}", new object[]
			{
				this.Width,
				this.Height
			});
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0009A80C File Offset: 0x00098A0C
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is Size)
			{
				result = (this == (Size)obj);
			}
			return result;
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0009A838 File Offset: 0x00098A38
		public override int GetHashCode()
		{
			ulong num;
			if (this.Width < 0)
			{
				if (this.Width == -2147483648)
				{
					num = (ulong)((long)(-1 * (this.Width + 1)));
				}
				else
				{
					num = (ulong)((long)(-(long)this.Width));
				}
			}
			else
			{
				num = (ulong)((long)this.Width);
			}
			num *= 4294967296UL;
			if (this.Height < 0)
			{
				if (this.Height == -2147483648)
				{
					num += (ulong)((long)(-1 * (this.Height + 1)));
				}
				else
				{
					num += (ulong)((long)(-(long)this.Height));
				}
			}
			else
			{
				num += (ulong)((long)this.Height);
			}
			return num.GetHashCode();
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0009A8D4 File Offset: 0x00098AD4
		public static bool operator ==(Size first, Size second)
		{
			return first.Width == second.Width && first.Height == second.Height;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x0009A906 File Offset: 0x00098B06
		public static bool operator !=(Size first, Size second)
		{
			return !(first == second);
		}

		// Token: 0x04000A86 RID: 2694
		private int width;

		// Token: 0x04000A87 RID: 2695
		private int height;
	}
}
