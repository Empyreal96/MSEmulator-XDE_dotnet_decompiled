using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000519 RID: 1305
	internal sealed class ComplexWriter
	{
		// Token: 0x060036E7 RID: 14055 RVA: 0x001288A5 File Offset: 0x00126AA5
		internal void Initialize(LineOutput lineOutput, int numberOfTextColumns)
		{
			this.lo = lineOutput;
			this.textColumns = numberOfTextColumns;
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x001288B5 File Offset: 0x00126AB5
		internal void WriteString(string s)
		{
			this.indentationManager.Clear();
			this.AddToBuffer(s);
			this.WriteToScreen();
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x001288D0 File Offset: 0x00126AD0
		internal void WriteObject(List<FormatValue> formatValueList)
		{
			this.indentationManager.Clear();
			foreach (FormatValue formatValue in formatValueList)
			{
				FormatEntry fe = (FormatEntry)formatValue;
				this.GenerateFormatEntryDisplay(fe, 0);
			}
			this.WriteToScreen();
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x00128938 File Offset: 0x00126B38
		private void GenerateFormatEntryDisplay(FormatEntry fe, int currentDepth)
		{
			foreach (object obj in fe.formatValueList)
			{
				FormatEntry formatEntry = obj as FormatEntry;
				if (formatEntry != null)
				{
					if (currentDepth < 50)
					{
						if (formatEntry.frameInfo != null)
						{
							using (this.indentationManager.StackFrame(formatEntry.frameInfo))
							{
								this.GenerateFormatEntryDisplay(formatEntry, currentDepth + 1);
								continue;
							}
						}
						this.GenerateFormatEntryDisplay(formatEntry, currentDepth + 1);
					}
				}
				else if (obj is FormatNewLine)
				{
					this.WriteToScreen();
				}
				else
				{
					FormatTextField formatTextField = obj as FormatTextField;
					if (formatTextField != null)
					{
						this.AddToBuffer(formatTextField.text);
					}
					else
					{
						FormatPropertyField formatPropertyField = obj as FormatPropertyField;
						if (formatPropertyField != null)
						{
							this.AddToBuffer(formatPropertyField.propertyValue);
						}
					}
				}
			}
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x00128A24 File Offset: 0x00126C24
		private void AddToBuffer(string s)
		{
			this.stringBuffer.Append(s);
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x00128A34 File Offset: 0x00126C34
		private void WriteToScreen()
		{
			int leftIndentation = this.indentationManager.LeftIndentation;
			int rightIndentation = this.indentationManager.RightIndentation;
			int num = this.indentationManager.FirstLineIndentation;
			int num2 = this.textColumns - rightIndentation - leftIndentation;
			if (num2 <= 0)
			{
				this.stringBuffer = new StringBuilder();
			}
			int num3 = (num > 0) ? num : (-num);
			if (num3 >= num2)
			{
				num = 0;
			}
			int num4 = this.textColumns - rightIndentation - leftIndentation;
			int num5 = num4;
			if (num >= 0)
			{
				num4 -= num;
			}
			else
			{
				num5 += num;
			}
			StringCollection stringCollection = StringManipulationHelper.GenerateLines(this.lo.DisplayCells, this.stringBuffer.ToString(), num4, num5);
			int num6 = leftIndentation;
			int num7 = leftIndentation;
			if (num >= 0)
			{
				num6 += num;
			}
			else
			{
				num7 -= num;
			}
			bool flag = true;
			foreach (string val in stringCollection)
			{
				if (flag)
				{
					flag = false;
					this.lo.WriteLine(StringManipulationHelper.PadLeft(val, num6));
				}
				else
				{
					this.lo.WriteLine(StringManipulationHelper.PadLeft(val, num7));
				}
			}
			this.stringBuffer = new StringBuilder();
		}

		// Token: 0x04001C19 RID: 7193
		private const int maxRecursionDepth = 50;

		// Token: 0x04001C1A RID: 7194
		private IndentationManager indentationManager = new IndentationManager();

		// Token: 0x04001C1B RID: 7195
		private StringBuilder stringBuffer = new StringBuilder();

		// Token: 0x04001C1C RID: 7196
		private LineOutput lo;

		// Token: 0x04001C1D RID: 7197
		private int textColumns;
	}
}
