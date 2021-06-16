using System;
using System.Globalization;

namespace System.Management.Automation.Host
{
	// Token: 0x02000224 RID: 548
	public struct BufferCell
	{
		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x0009ACF2 File Offset: 0x00098EF2
		// (set) Token: 0x060019D9 RID: 6617 RVA: 0x0009ACFA File Offset: 0x00098EFA
		public char Character
		{
			get
			{
				return this.character;
			}
			set
			{
				this.character = value;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060019DA RID: 6618 RVA: 0x0009AD03 File Offset: 0x00098F03
		// (set) Token: 0x060019DB RID: 6619 RVA: 0x0009AD0B File Offset: 0x00098F0B
		public ConsoleColor ForegroundColor
		{
			get
			{
				return this.foregroundColor;
			}
			set
			{
				this.foregroundColor = value;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060019DC RID: 6620 RVA: 0x0009AD14 File Offset: 0x00098F14
		// (set) Token: 0x060019DD RID: 6621 RVA: 0x0009AD1C File Offset: 0x00098F1C
		public ConsoleColor BackgroundColor
		{
			get
			{
				return this.backgroundColor;
			}
			set
			{
				this.backgroundColor = value;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060019DE RID: 6622 RVA: 0x0009AD25 File Offset: 0x00098F25
		// (set) Token: 0x060019DF RID: 6623 RVA: 0x0009AD2D File Offset: 0x00098F2D
		public BufferCellType BufferCellType
		{
			get
			{
				return this.bufferCellType;
			}
			set
			{
				this.bufferCellType = value;
			}
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x0009AD36 File Offset: 0x00098F36
		public BufferCell(char character, ConsoleColor foreground, ConsoleColor background, BufferCellType bufferCellType)
		{
			this.character = character;
			this.foregroundColor = foreground;
			this.backgroundColor = background;
			this.bufferCellType = bufferCellType;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x0009AD58 File Offset: 0x00098F58
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "'{0}' {1} {2} {3}", new object[]
			{
				this.Character,
				this.ForegroundColor,
				this.BackgroundColor,
				this.BufferCellType
			});
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x0009ADB4 File Offset: 0x00098FB4
		public override bool Equals(object obj)
		{
			bool result = false;
			if (obj is BufferCell)
			{
				result = (this == (BufferCell)obj);
			}
			return result;
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x0009ADE0 File Offset: 0x00098FE0
		public override int GetHashCode()
		{
			uint num = (uint)((uint)(this.ForegroundColor ^ this.BackgroundColor) << 16);
			return (num | (uint)this.Character).GetHashCode();
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x0009AE10 File Offset: 0x00099010
		public static bool operator ==(BufferCell first, BufferCell second)
		{
			return first.Character == second.Character && first.BackgroundColor == second.BackgroundColor && first.ForegroundColor == second.ForegroundColor && first.BufferCellType == second.BufferCellType;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0009AE62 File Offset: 0x00099062
		public static bool operator !=(BufferCell first, BufferCell second)
		{
			return !(first == second);
		}

		// Token: 0x04000A9F RID: 2719
		private const string StringsBaseName = "MshHostRawUserInterfaceStrings";

		// Token: 0x04000AA0 RID: 2720
		private char character;

		// Token: 0x04000AA1 RID: 2721
		private ConsoleColor foregroundColor;

		// Token: 0x04000AA2 RID: 2722
		private ConsoleColor backgroundColor;

		// Token: 0x04000AA3 RID: 2723
		private BufferCellType bufferCellType;
	}
}
