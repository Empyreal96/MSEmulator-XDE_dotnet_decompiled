using System;
using System.Globalization;

namespace System.Management.Automation.Host
{
	// Token: 0x02000223 RID: 547
	public struct Rectangle
	{
		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x0009AA9A File Offset: 0x00098C9A
		// (set) Token: 0x060019CA RID: 6602 RVA: 0x0009AAA2 File Offset: 0x00098CA2
		public int Left
		{
			get
			{
				return this.left;
			}
			set
			{
				this.left = value;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x0009AAAB File Offset: 0x00098CAB
		// (set) Token: 0x060019CC RID: 6604 RVA: 0x0009AAB3 File Offset: 0x00098CB3
		public int Top
		{
			get
			{
				return this.top;
			}
			set
			{
				this.top = value;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x0009AABC File Offset: 0x00098CBC
		// (set) Token: 0x060019CE RID: 6606 RVA: 0x0009AAC4 File Offset: 0x00098CC4
		public int Right
		{
			get
			{
				return this.right;
			}
			set
			{
				this.right = value;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x0009AACD File Offset: 0x00098CCD
		// (set) Token: 0x060019D0 RID: 6608 RVA: 0x0009AAD5 File Offset: 0x00098CD5
		public int Bottom
		{
			get
			{
				return this.bottom;
			}
			set
			{
				this.bottom = value;
			}
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x0009AAE0 File Offset: 0x00098CE0
		public Rectangle(int left, int top, int right, int bottom)
		{
			if (right < left)
			{
				throw PSTraceSource.NewArgumentException("right", MshHostRawUserInterfaceStrings.LessThanErrorTemplate, new object[]
				{
					"right",
					"left"
				});
			}
			if (bottom < top)
			{
				throw PSTraceSource.NewArgumentException("bottom", MshHostRawUserInterfaceStrings.LessThanErrorTemplate, new object[]
				{
					"bottom",
					"top"
				});
			}
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x0009AB63 File Offset: 0x00098D63
		public Rectangle(Coordinates upperLeft, Coordinates lowerRight)
		{
			this = new Rectangle(upperLeft.X, upperLeft.Y, lowerRight.X, lowerRight.Y);
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x0009AB88 File Offset: 0x00098D88
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0},{1} ; {2},{3}", new object[]
			{
				this.Left,
				this.Top,
				this.Right,
				this.Bottom
			});
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x0009ABE4 File Offset: 0x00098DE4
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is Rectangle)
			{
				result = (this == (Rectangle)obj);
			}
			return result;
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0009AC10 File Offset: 0x00098E10
		public override int GetHashCode()
		{
			int num = this.Top ^ this.Bottom;
			ulong num2;
			if (num < 0)
			{
				if (num == -2147483648)
				{
					num2 = (ulong)((long)(-1 * (num + 1)));
				}
				else
				{
					num2 = (ulong)((long)(-(long)num));
				}
			}
			else
			{
				num2 = (ulong)((long)num);
			}
			num2 *= 4294967296UL;
			int num3 = this.Left ^ this.Right;
			if (num3 < 0)
			{
				if (num3 == -2147483648)
				{
					num2 += (ulong)((long)(-1 * (num3 + 1)));
				}
				else
				{
					num2 += (ulong)((long)(-(long)num));
				}
			}
			else
			{
				num2 += (ulong)((long)num3);
			}
			return num2.GetHashCode();
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x0009AC94 File Offset: 0x00098E94
		public static bool operator ==(Rectangle first, Rectangle second)
		{
			return first.Top == second.Top && first.Left == second.Left && first.Bottom == second.Bottom && first.Right == second.Right;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x0009ACE6 File Offset: 0x00098EE6
		public static bool operator !=(Rectangle first, Rectangle second)
		{
			return !(first == second);
		}

		// Token: 0x04000A9B RID: 2715
		private int left;

		// Token: 0x04000A9C RID: 2716
		private int top;

		// Token: 0x04000A9D RID: 2717
		private int right;

		// Token: 0x04000A9E RID: 2718
		private int bottom;
	}
}
