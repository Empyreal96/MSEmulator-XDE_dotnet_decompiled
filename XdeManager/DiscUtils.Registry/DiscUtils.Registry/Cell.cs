using System;
using DiscUtils.Streams;

namespace DiscUtils.Registry
{
	// Token: 0x02000004 RID: 4
	internal abstract class Cell : IByteArraySerializable
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002697 File Offset: 0x00000897
		public Cell(int index)
		{
			this.Index = index;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000026A6 File Offset: 0x000008A6
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000026AE File Offset: 0x000008AE
		public int Index { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15
		public abstract int Size { get; }

		// Token: 0x06000010 RID: 16
		public abstract int ReadFrom(byte[] buffer, int offset);

		// Token: 0x06000011 RID: 17
		public abstract void WriteTo(byte[] buffer, int offset);

		// Token: 0x06000012 RID: 18 RVA: 0x000026B8 File Offset: 0x000008B8
		internal static Cell Parse(RegistryHive hive, int index, byte[] buffer, int pos)
		{
			string text = EndianUtilities.BytesToString(buffer, pos, 2);
			if (text != null)
			{
				uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
				Cell cell;
				if (num <= 1077001542U)
				{
					if (num != 1045020422U)
					{
						if (num != 1045564875U)
						{
							if (num != 1077001542U)
							{
								goto IL_10D;
							}
							if (!(text == "li"))
							{
								goto IL_10D;
							}
						}
						else
						{
							if (!(text == "sk"))
							{
								goto IL_10D;
							}
							cell = new SecurityCell(index);
							goto IL_123;
						}
					}
					else
					{
						if (!(text == "vk"))
						{
							goto IL_10D;
						}
						cell = new ValueCell(index);
						goto IL_123;
					}
				}
				else
				{
					if (num <= 1261555351U)
					{
						if (num != 1093779161U)
						{
							if (num != 1261555351U)
							{
								goto IL_10D;
							}
							if (!(text == "lf"))
							{
								goto IL_10D;
							}
						}
						else if (!(text == "lh"))
						{
							goto IL_10D;
						}
						cell = new SubKeyHashedListCell(hive, index);
						goto IL_123;
					}
					if (num != 1580624302U)
					{
						if (num != 1683261492U)
						{
							goto IL_10D;
						}
						if (!(text == "ri"))
						{
							goto IL_10D;
						}
					}
					else
					{
						if (!(text == "nk"))
						{
							goto IL_10D;
						}
						cell = new KeyNodeCell(index);
						goto IL_123;
					}
				}
				cell = new SubKeyIndirectListCell(hive, index);
				IL_123:
				cell.ReadFrom(buffer, pos);
				return cell;
			}
			IL_10D:
			throw new RegistryCorruptException("Unknown cell type '" + text + "'");
		}
	}
}
