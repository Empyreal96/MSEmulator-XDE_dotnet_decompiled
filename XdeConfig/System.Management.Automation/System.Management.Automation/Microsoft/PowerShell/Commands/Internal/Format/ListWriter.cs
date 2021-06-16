using System;
using System.Collections.Specialized;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004F4 RID: 1268
	internal class ListWriter
	{
		// Token: 0x06003663 RID: 13923 RVA: 0x001266A8 File Offset: 0x001248A8
		internal void Initialize(string[] propertyNames, int screenColumnWidth, DisplayCells dc)
		{
			this.columnWidth = screenColumnWidth;
			if (propertyNames == null || propertyNames.Length == 0)
			{
				this.disabled = true;
				return;
			}
			this.disabled = false;
			if (screenColumnWidth - " : ".Length - 1 - 1 < 0)
			{
				this.disabled = true;
				return;
			}
			int num = screenColumnWidth - " : ".Length - 1;
			this.propertyLabelsDisplayLength = 0;
			int[] array = new int[propertyNames.Length];
			for (int i = 0; i < propertyNames.Length; i++)
			{
				array[i] = dc.Length(propertyNames[i]);
				if (array[i] > this.propertyLabelsDisplayLength)
				{
					this.propertyLabelsDisplayLength = array[i];
				}
			}
			if (this.propertyLabelsDisplayLength > num)
			{
				this.propertyLabelsDisplayLength = num;
			}
			this.propertyLabels = new string[propertyNames.Length];
			for (int j = 0; j < propertyNames.Length; j++)
			{
				if (array[j] < this.propertyLabelsDisplayLength)
				{
					this.propertyLabels[j] = propertyNames[j] + new string(' ', this.propertyLabelsDisplayLength - array[j]);
				}
				else if (array[j] > this.propertyLabelsDisplayLength)
				{
					this.propertyLabels[j] = propertyNames[j].Substring(0, dc.GetHeadSplitLength(propertyNames[j], this.propertyLabelsDisplayLength));
				}
				else
				{
					this.propertyLabels[j] = propertyNames[j];
				}
				string[] array2;
				IntPtr intPtr;
				(array2 = this.propertyLabels)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + " : ";
			}
			this.propertyLabelsDisplayLength += " : ".Length;
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x00126808 File Offset: 0x00124A08
		internal void WriteProperties(string[] values, LineOutput lo)
		{
			if (this.disabled)
			{
				return;
			}
			string[] array;
			if (values == null)
			{
				array = new string[this.propertyLabels.Length];
				for (int i = 0; i < this.propertyLabels.Length; i++)
				{
					array[i] = "";
				}
			}
			else if (values.Length < this.propertyLabels.Length)
			{
				array = new string[this.propertyLabels.Length];
				for (int j = 0; j < this.propertyLabels.Length; j++)
				{
					if (j < values.Length)
					{
						array[j] = values[j];
					}
					else
					{
						array[j] = "";
					}
				}
			}
			else if (values.Length > this.propertyLabels.Length)
			{
				array = new string[this.propertyLabels.Length];
				for (int k = 0; k < this.propertyLabels.Length; k++)
				{
					array[k] = values[k];
				}
			}
			else
			{
				array = values;
			}
			for (int l = 0; l < this.propertyLabels.Length; l++)
			{
				this.WriteProperty(l, array[l], lo);
			}
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x001268F0 File Offset: 0x00124AF0
		private void WriteProperty(int k, string propertyValue, LineOutput lo)
		{
			if (propertyValue == null)
			{
				propertyValue = "";
			}
			string[] array = StringManipulationHelper.SplitLines(propertyValue);
			string text = null;
			for (int i = 0; i < array.Length; i++)
			{
				string prependString;
				if (i == 0)
				{
					prependString = this.propertyLabels[k];
				}
				else
				{
					if (text == null)
					{
						text = new string(' ', this.propertyLabelsDisplayLength);
					}
					prependString = text;
				}
				this.WriteSingleLineHelper(prependString, array[i], lo);
			}
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x00126950 File Offset: 0x00124B50
		private void WriteSingleLineHelper(string prependString, string line, LineOutput lo)
		{
			if (line == null)
			{
				line = "";
			}
			int num = this.columnWidth - this.propertyLabelsDisplayLength;
			StringCollection stringCollection = StringManipulationHelper.GenerateLines(lo.DisplayCells, line, num, num);
			string str = new string(' ', this.propertyLabelsDisplayLength);
			for (int i = 0; i < stringCollection.Count; i++)
			{
				if (i == 0)
				{
					lo.WriteLine(prependString + stringCollection[i]);
				}
				else
				{
					lo.WriteLine(str + stringCollection[i]);
				}
			}
		}

		// Token: 0x04001BD6 RID: 7126
		private const string Separator = " : ";

		// Token: 0x04001BD7 RID: 7127
		private const int MinLabelWidth = 1;

		// Token: 0x04001BD8 RID: 7128
		private const int MinFieldWidth = 1;

		// Token: 0x04001BD9 RID: 7129
		private string[] propertyLabels;

		// Token: 0x04001BDA RID: 7130
		private int propertyLabelsDisplayLength;

		// Token: 0x04001BDB RID: 7131
		private int columnWidth;

		// Token: 0x04001BDC RID: 7132
		private bool disabled;
	}
}
