using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004D6 RID: 1238
	internal sealed class ColumnWidthManager
	{
		// Token: 0x06003609 RID: 13833 RVA: 0x00125221 File Offset: 0x00123421
		internal ColumnWidthManager(int tableWidth, int minimumColumnWidth, int separatorWidth)
		{
			this.tableWidth = tableWidth;
			this.minimumColumnWidth = minimumColumnWidth;
			this.separatorWidth = separatorWidth;
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x0012523E File Offset: 0x0012343E
		internal void CalculateColumnWidths(int[] columnWidths)
		{
			if (this.AssignColumnWidths(columnWidths))
			{
				return;
			}
			this.TrimToFit(columnWidths);
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x00125254 File Offset: 0x00123454
		private bool AssignColumnWidths(int[] columnWidths)
		{
			bool flag = true;
			int num = 0;
			for (int i = 0; i < columnWidths.Length; i++)
			{
				if (columnWidths[i] <= 0)
				{
					flag = false;
					break;
				}
				num += columnWidths[i];
			}
			if (flag)
			{
				num += this.separatorWidth * (columnWidths.Length - 1);
				return num <= this.tableWidth;
			}
			bool[] array = new bool[columnWidths.Length];
			for (int j = 0; j < columnWidths.Length; j++)
			{
				array[j] = (columnWidths[j] > 0);
				if (columnWidths[j] == 0)
				{
					columnWidths[j] = this.minimumColumnWidth;
				}
			}
			int num2 = this.CurrentTableWidth(columnWidths);
			int k = this.tableWidth - num2;
			if (k < 0)
			{
				return false;
			}
			if (k == 0)
			{
				return true;
			}
			while (k > 0)
			{
				for (int l = 0; l < columnWidths.Length; l++)
				{
					if (!array[l])
					{
						columnWidths[l]++;
						k--;
						if (k == 0)
						{
							break;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x00125338 File Offset: 0x00123538
		private void TrimToFit(int[] columnWidths)
		{
			for (;;)
			{
				int num = this.CurrentTableWidth(columnWidths);
				int num2 = num - this.tableWidth;
				if (num2 <= 0)
				{
					break;
				}
				int lastVisibleColumn = ColumnWidthManager.GetLastVisibleColumn(columnWidths);
				if (lastVisibleColumn < 0)
				{
					return;
				}
				int num3 = columnWidths[lastVisibleColumn] - num2;
				if (num3 < this.minimumColumnWidth)
				{
					columnWidths[lastVisibleColumn] = -1;
				}
				else
				{
					columnWidths[lastVisibleColumn] = num3;
				}
			}
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x00125384 File Offset: 0x00123584
		private int CurrentTableWidth(int[] columnWidths)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < columnWidths.Length; i++)
			{
				if (columnWidths[i] > 0)
				{
					num += columnWidths[i];
					num2++;
				}
			}
			return num + this.separatorWidth * (num2 - 1);
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x001253C0 File Offset: 0x001235C0
		private static int GetLastVisibleColumn(int[] columnWidths)
		{
			for (int i = 0; i < columnWidths.Length; i++)
			{
				if (columnWidths[i] < 0)
				{
					return i - 1;
				}
			}
			return columnWidths.Length - 1;
		}

		// Token: 0x04001B8F RID: 7055
		private int tableWidth;

		// Token: 0x04001B90 RID: 7056
		private int minimumColumnWidth;

		// Token: 0x04001B91 RID: 7057
		private int separatorWidth;
	}
}
