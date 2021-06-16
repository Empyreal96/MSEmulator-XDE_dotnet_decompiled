using System;
using System.Collections.Specialized;
using System.Text;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004F5 RID: 1269
	internal class TableWriter
	{
		// Token: 0x06003668 RID: 13928 RVA: 0x001269D8 File Offset: 0x00124BD8
		internal static int ComputeWideViewBestItemsPerRowFit(int stringLen, int screenColumns)
		{
			if (stringLen <= 0 || screenColumns < 1)
			{
				return 1;
			}
			if (stringLen >= screenColumns)
			{
				return 1;
			}
			int num = 1;
			for (;;)
			{
				int num2 = num + 1;
				int num3 = stringLen * num2 + (num2 - 1);
				if (num3 >= screenColumns)
				{
					break;
				}
				num++;
			}
			return num;
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x00126A10 File Offset: 0x00124C10
		internal void Initialize(int leftMarginIndent, int screenColumns, int[] columnWidths, int[] alignment, bool suppressHeader)
		{
			if (leftMarginIndent < 0)
			{
				leftMarginIndent = 0;
			}
			if (screenColumns - leftMarginIndent < 5)
			{
				this.disabled = true;
				return;
			}
			this.startColumn = leftMarginIndent;
			this.hideHeader = suppressHeader;
			ColumnWidthManager columnWidthManager = new ColumnWidthManager(screenColumns - leftMarginIndent, 1, 1);
			columnWidthManager.CalculateColumnWidths(columnWidths);
			bool flag = false;
			for (int i = 0; i < columnWidths.Length; i++)
			{
				if (columnWidths[i] >= 1)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.disabled = true;
				return;
			}
			this.si = new TableWriter.ScreenInfo();
			this.si.screenColumns = screenColumns;
			this.si.columnInfo = new TableWriter.ColumnInfo[columnWidths.Length];
			int num = this.startColumn;
			for (int j = 0; j < columnWidths.Length; j++)
			{
				this.si.columnInfo[j] = new TableWriter.ColumnInfo();
				this.si.columnInfo[j].startCol = num;
				this.si.columnInfo[j].width = columnWidths[j];
				this.si.columnInfo[j].alignment = alignment[j];
				num += columnWidths[j] + 1;
			}
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x00126B1C File Offset: 0x00124D1C
		internal void GenerateHeader(string[] values, LineOutput lo)
		{
			if (this.disabled)
			{
				return;
			}
			if (this.hideHeader)
			{
				return;
			}
			this.GenerateRow(values, lo, true, null, lo.DisplayCells);
			string[] array = new string[values.Length];
			for (int i = 0; i < this.si.columnInfo.Length; i++)
			{
				if (this.si.columnInfo[i].width <= 0)
				{
					array[i] = "";
				}
				else
				{
					int num = this.si.columnInfo[i].width;
					if (!string.IsNullOrEmpty(values[i]))
					{
						int num2 = lo.DisplayCells.Length(values[i]);
						if (num2 < num)
						{
							num = num2;
						}
					}
					array[i] = new string('-', num);
				}
			}
			this.GenerateRow(array, lo, false, null, lo.DisplayCells);
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x00126BD8 File Offset: 0x00124DD8
		internal void GenerateRow(string[] values, LineOutput lo, bool multiLine, int[] alignment, DisplayCells dc)
		{
			if (this.disabled)
			{
				return;
			}
			int num = this.si.columnInfo.Length;
			int[] array = new int[num];
			if (alignment == null)
			{
				for (int i = 0; i < num; i++)
				{
					array[i] = this.si.columnInfo[i].alignment;
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					if (alignment[j] == 0)
					{
						array[j] = this.si.columnInfo[j].alignment;
					}
					else
					{
						array[j] = alignment[j];
					}
				}
			}
			if (multiLine)
			{
				string[] array2 = this.GenerateTableRow(values, array, lo.DisplayCells);
				for (int k = 0; k < array2.Length; k++)
				{
					lo.WriteLine(array2[k]);
				}
				return;
			}
			lo.WriteLine(this.GenerateRow(values, array, dc));
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x00126C9C File Offset: 0x00124E9C
		private string[] GenerateTableRow(string[] values, int[] alignment, DisplayCells ds)
		{
			int[] array = new int[this.si.columnInfo.Length];
			int num = 0;
			for (int i = 0; i < this.si.columnInfo.Length; i++)
			{
				if (this.si.columnInfo[i].width > 0)
				{
					array[num++] = i;
				}
			}
			if (num == 0)
			{
				return null;
			}
			StringCollection[] array2 = new StringCollection[num];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = this.GenerateMultiLineRowField(values[array[j]], array[j], alignment[array[j]], ds);
				if (j > 0)
				{
					for (int k = 0; k < array2[j].Count; k++)
					{
						array2[j][k] = new string(' ', 1) + array2[j][k];
					}
				}
				else if (this.startColumn > 0)
				{
					for (int l = 0; l < array2[j].Count; l++)
					{
						array2[j][l] = new string(' ', this.startColumn) + array2[j][l];
					}
				}
			}
			int num2 = 0;
			for (int m = 0; m < array2.Length; m++)
			{
				if (array2[m].Count > num2)
				{
					num2 = array2[m].Count;
				}
			}
			for (int n = 0; n < array2.Length; n++)
			{
				int num3 = this.si.columnInfo[array[n]].width;
				if (n > 0)
				{
					num3++;
				}
				else
				{
					num3 += this.startColumn;
				}
				int num4 = num2 - array2[n].Count;
				if (num4 > 0)
				{
					for (int num5 = 0; num5 < num4; num5++)
					{
						array2[n].Add(new string(' ', num3));
					}
				}
			}
			string[] array3 = new string[num2];
			for (int num6 = 0; num6 < array3.Length; num6++)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int num7 = 0; num7 < array2.Length; num7++)
				{
					stringBuilder.Append(array2[num7][num6]);
				}
				array3[num6] = stringBuilder.ToString();
			}
			return array3;
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x00126EB8 File Offset: 0x001250B8
		private StringCollection GenerateMultiLineRowField(string val, int k, int aligment, DisplayCells dc)
		{
			StringCollection stringCollection = StringManipulationHelper.GenerateLines(dc, val, this.si.columnInfo[k].width, this.si.columnInfo[k].width);
			for (int i = 0; i < stringCollection.Count; i++)
			{
				if (dc.Length(stringCollection[i]) < this.si.columnInfo[k].width)
				{
					stringCollection[i] = TableWriter.GenerateRowField(stringCollection[i], this.si.columnInfo[k].width, aligment, dc);
				}
			}
			return stringCollection;
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x00126F50 File Offset: 0x00125150
		private string GenerateRow(string[] values, int[] alignment, DisplayCells dc)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.si.columnInfo.Length; i++)
			{
				if (this.si.columnInfo[i].width > 0)
				{
					int length = stringBuilder.Length;
					if (i > 0)
					{
						stringBuilder.Append(new string(' ', 1));
					}
					else if (this.startColumn > 0)
					{
						stringBuilder.Append(new string(' ', this.startColumn));
					}
					stringBuilder.Append(TableWriter.GenerateRowField(values[i], this.si.columnInfo[i].width, alignment[i], dc));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x00126FF8 File Offset: 0x001251F8
		private static string GenerateRowField(string val, int width, int alignment, DisplayCells dc)
		{
			string text = StringManipulationHelper.TruncateAtNewLine(val);
			if (text == null)
			{
				text = "";
			}
			string str = text;
			int num = dc.Length(str);
			if (num < width)
			{
				int num2 = width - num;
				switch (alignment)
				{
				case 2:
				{
					int num3 = num2 / 2;
					int count = num2 - num3;
					text = new string(' ', num3) + text + new string(' ', count);
					break;
				}
				case 3:
					text = new string(' ', num2) + text;
					break;
				default:
					text += new string(' ', num2);
					break;
				}
			}
			else if (num > width)
			{
				int num4 = width - "...".Length;
				if (num4 > 0)
				{
					switch (alignment)
					{
					case 2:
						text = text.Substring(0, dc.GetHeadSplitLength(text, num4));
						text += "...";
						break;
					case 3:
					{
						int tailSplitLength = dc.GetTailSplitLength(text, num4);
						text = text.Substring(text.Length - tailSplitLength);
						text = "..." + text;
						break;
					}
					default:
						text = text.Substring(0, dc.GetHeadSplitLength(text, num4));
						text += "...";
						break;
					}
				}
				else
				{
					switch (alignment)
					{
					case 2:
						text = text.Substring(0, dc.GetHeadSplitLength(text, width));
						break;
					case 3:
					{
						int tailSplitLength2 = dc.GetTailSplitLength(text, width);
						text = text.Substring(text.Length - tailSplitLength2, tailSplitLength2);
						break;
					}
					default:
						text = text.Substring(0, dc.GetHeadSplitLength(text, width));
						break;
					}
				}
			}
			int num5 = dc.Length(text);
			if (num5 == width)
			{
				return text;
			}
			switch (alignment)
			{
			case 2:
				text += " ";
				break;
			case 3:
				text = " " + text;
				break;
			default:
				text += " ";
				break;
			}
			return text;
		}

		// Token: 0x04001BDD RID: 7133
		private const string ellipsis = "...";

		// Token: 0x04001BDE RID: 7134
		private TableWriter.ScreenInfo si;

		// Token: 0x04001BDF RID: 7135
		private bool disabled;

		// Token: 0x04001BE0 RID: 7136
		private bool hideHeader;

		// Token: 0x04001BE1 RID: 7137
		private int startColumn;

		// Token: 0x020004F6 RID: 1270
		private class ColumnInfo
		{
			// Token: 0x04001BE2 RID: 7138
			internal int startCol;

			// Token: 0x04001BE3 RID: 7139
			internal int width;

			// Token: 0x04001BE4 RID: 7140
			internal int alignment = 1;
		}

		// Token: 0x020004F7 RID: 1271
		private class ScreenInfo
		{
			// Token: 0x04001BE5 RID: 7141
			internal const int separatorCharacterCount = 1;

			// Token: 0x04001BE6 RID: 7142
			internal const int minimumScreenColumns = 5;

			// Token: 0x04001BE7 RID: 7143
			internal const int minimumColumnWidth = 1;

			// Token: 0x04001BE8 RID: 7144
			internal int screenColumns;

			// Token: 0x04001BE9 RID: 7145
			internal TableWriter.ColumnInfo[] columnInfo;
		}
	}
}
