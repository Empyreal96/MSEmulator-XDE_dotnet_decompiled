using System;

namespace System.Management.Automation.Host
{
	// Token: 0x0200020B RID: 523
	public abstract class PSHostRawUserInterface
	{
		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001831 RID: 6193
		// (set) Token: 0x06001832 RID: 6194
		public abstract ConsoleColor ForegroundColor { get; set; }

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001833 RID: 6195
		// (set) Token: 0x06001834 RID: 6196
		public abstract ConsoleColor BackgroundColor { get; set; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001835 RID: 6197
		// (set) Token: 0x06001836 RID: 6198
		public abstract Coordinates CursorPosition { get; set; }

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001837 RID: 6199
		// (set) Token: 0x06001838 RID: 6200
		public abstract Coordinates WindowPosition { get; set; }

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001839 RID: 6201
		// (set) Token: 0x0600183A RID: 6202
		public abstract int CursorSize { get; set; }

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x0600183B RID: 6203
		// (set) Token: 0x0600183C RID: 6204
		public abstract Size BufferSize { get; set; }

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x0600183D RID: 6205
		// (set) Token: 0x0600183E RID: 6206
		public abstract Size WindowSize { get; set; }

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x0600183F RID: 6207
		public abstract Size MaxWindowSize { get; }

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001840 RID: 6208
		public abstract Size MaxPhysicalWindowSize { get; }

		// Token: 0x06001841 RID: 6209 RVA: 0x00094A58 File Offset: 0x00092C58
		public KeyInfo ReadKey()
		{
			return this.ReadKey(ReadKeyOptions.IncludeKeyDown);
		}

		// Token: 0x06001842 RID: 6210
		public abstract KeyInfo ReadKey(ReadKeyOptions options);

		// Token: 0x06001843 RID: 6211
		public abstract void FlushInputBuffer();

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001844 RID: 6212
		public abstract bool KeyAvailable { get; }

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001845 RID: 6213
		// (set) Token: 0x06001846 RID: 6214
		public abstract string WindowTitle { get; set; }

		// Token: 0x06001847 RID: 6215
		public abstract void SetBufferContents(Coordinates origin, BufferCell[,] contents);

		// Token: 0x06001848 RID: 6216
		public abstract void SetBufferContents(Rectangle rectangle, BufferCell fill);

		// Token: 0x06001849 RID: 6217
		public abstract BufferCell[,] GetBufferContents(Rectangle rectangle);

		// Token: 0x0600184A RID: 6218
		public abstract void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill);

		// Token: 0x0600184B RID: 6219 RVA: 0x00094A64 File Offset: 0x00092C64
		public virtual int LengthInBufferCells(string source, int offset)
		{
			if (source == null)
			{
				throw PSTraceSource.NewArgumentNullException("source");
			}
			string source2 = source.Substring(offset);
			return this.LengthInBufferCells(source2);
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x00094A8E File Offset: 0x00092C8E
		public virtual int LengthInBufferCells(string source)
		{
			if (source == null)
			{
				throw PSTraceSource.NewArgumentNullException("source");
			}
			return source.Length;
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x00094AA4 File Offset: 0x00092CA4
		public virtual int LengthInBufferCells(char source)
		{
			return 1;
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x00094AA8 File Offset: 0x00092CA8
		public BufferCell[,] NewBufferCellArray(string[] contents, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
		{
			if (contents == null)
			{
				throw PSTraceSource.NewArgumentNullException("contents");
			}
			byte[][] array = new byte[contents.Length][];
			int num = 0;
			for (int i = 0; i < contents.Length; i++)
			{
				if (!string.IsNullOrEmpty(contents[i]))
				{
					int num2 = 0;
					array[i] = new byte[contents[i].Length];
					for (int j = 0; j < contents[i].Length; j++)
					{
						array[i][j] = (byte)this.LengthInBufferCells(contents[i][j]);
						num2 += (int)array[i][j];
					}
					if (num < num2)
					{
						num = num2;
					}
				}
			}
			if (num <= 0)
			{
				throw PSTraceSource.NewArgumentException("contents", MshHostRawUserInterfaceStrings.AllNullOrEmptyStringsErrorTemplate, new object[0]);
			}
			BufferCell[,] array2 = new BufferCell[contents.Length, num];
			for (int k = 0; k < contents.Length; k++)
			{
				int l = 0;
				int m = 0;
				while (m < contents[k].Length)
				{
					if (array[k][m] == 1)
					{
						array2[k, l] = new BufferCell(contents[k][m], foregroundColor, backgroundColor, BufferCellType.Complete);
					}
					else if (array[k][m] == 2)
					{
						array2[k, l] = new BufferCell(contents[k][m], foregroundColor, backgroundColor, BufferCellType.Leading);
						l++;
						array2[k, l] = new BufferCell('\0', foregroundColor, backgroundColor, BufferCellType.Trailing);
					}
					m++;
					l++;
				}
				while (l < num)
				{
					array2[k, l] = new BufferCell(' ', foregroundColor, backgroundColor, BufferCellType.Complete);
					l++;
				}
			}
			return array2;
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x00094C40 File Offset: 0x00092E40
		public BufferCell[,] NewBufferCellArray(int width, int height, BufferCell contents)
		{
			if (width <= 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("width", width, MshHostRawUserInterfaceStrings.NonPositiveNumberErrorTemplate, new object[]
				{
					"width"
				});
			}
			if (height <= 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("height", height, MshHostRawUserInterfaceStrings.NonPositiveNumberErrorTemplate, new object[]
				{
					"height"
				});
			}
			BufferCell[,] array = new BufferCell[height, width];
			int num = this.LengthInBufferCells(contents.Character);
			if (num == 1)
			{
				for (int i = 0; i < array.GetLength(0); i++)
				{
					for (int j = 0; j < array.GetLength(1); j++)
					{
						array[i, j] = contents;
						array[i, j].BufferCellType = BufferCellType.Complete;
					}
				}
			}
			else if (num == 2)
			{
				int num2 = (width % 2 == 0) ? width : (width - 1);
				for (int k = 0; k < height; k++)
				{
					for (int l = 0; l < num2; l++)
					{
						array[k, l] = contents;
						array[k, l].BufferCellType = BufferCellType.Leading;
						l++;
						array[k, l] = new BufferCell('\0', contents.ForegroundColor, contents.BackgroundColor, BufferCellType.Trailing);
					}
					if (num2 < width)
					{
						array[k, num2] = contents;
						array[k, num2].BufferCellType = BufferCellType.Leading;
					}
				}
			}
			return array;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x00094DB1 File Offset: 0x00092FB1
		public BufferCell[,] NewBufferCellArray(Size size, BufferCell contents)
		{
			return this.NewBufferCellArray(size.Width, size.Height, contents);
		}
	}
}
