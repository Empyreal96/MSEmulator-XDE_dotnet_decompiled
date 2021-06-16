using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200051F RID: 1311
	internal class DisplayCells
	{
		// Token: 0x06003705 RID: 14085 RVA: 0x00129499 File Offset: 0x00127699
		internal virtual int Length(string str)
		{
			return this.Length(str, 0);
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x001294A3 File Offset: 0x001276A3
		internal virtual int Length(string str, int offset)
		{
			return str.Length - offset;
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x001294AD File Offset: 0x001276AD
		internal virtual int Length(char character)
		{
			return 1;
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x001294B0 File Offset: 0x001276B0
		internal virtual int GetHeadSplitLength(string str, int displayCells)
		{
			return this.GetHeadSplitLength(str, 0, displayCells);
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x001294BC File Offset: 0x001276BC
		internal virtual int GetHeadSplitLength(string str, int offset, int displayCells)
		{
			int num = str.Length - offset;
			if (num >= displayCells)
			{
				return displayCells;
			}
			return num;
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x001294D9 File Offset: 0x001276D9
		internal virtual int GetTailSplitLength(string str, int displayCells)
		{
			return this.GetTailSplitLength(str, 0, displayCells);
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x001294E4 File Offset: 0x001276E4
		internal virtual int GetTailSplitLength(string str, int offset, int displayCells)
		{
			int num = str.Length - offset;
			if (num >= displayCells)
			{
				return displayCells;
			}
			return num;
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x00129504 File Offset: 0x00127704
		protected int GetSplitLengthInternalHelper(string str, int offset, int displayCells, bool head)
		{
			int num = 0;
			int num2 = 0;
			int num3 = head ? offset : (str.Length - 1);
			int num4 = head ? (str.Length - 1) : offset;
			while ((!head || num3 <= num4) && (head || num3 >= num4))
			{
				int num5 = this.Length(str[num3]);
				if (num + num5 > displayCells)
				{
					break;
				}
				num += num5;
				num2++;
				if (num == displayCells)
				{
					break;
				}
				num3 = (head ? (num3 + 1) : (num3 - 1));
			}
			return num2;
		}
	}
}
