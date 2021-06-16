using System;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000003 RID: 3
	internal sealed class BlockBitmap
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000020F9 File Offset: 0x000002F9
		public BlockBitmap(byte[] data, int offset, int length)
		{
			this._data = data;
			this._offset = offset;
			this._length = length;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002118 File Offset: 0x00000318
		public int ContiguousSectors(int first, out bool state)
		{
			int num = 0;
			int num2 = first % 8;
			int i = first / 8;
			state = (((int)this._data[this._offset + i] & 1 << num2) != 0);
			byte b = state ? byte.MaxValue : 0;
			while (i < this._length)
			{
				if (this._data[this._offset + i] == b)
				{
					num += 8 - num2;
					i++;
					num2 = 0;
				}
				else
				{
					if (((int)this._data[this._offset + i] & 1 << num2) != 0 != state)
					{
						break;
					}
					num++;
					num2++;
					if (num2 == 8)
					{
						num2 = 0;
						i++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021B4 File Offset: 0x000003B4
		internal bool MarkSectorsPresent(int first, int count)
		{
			bool result = false;
			int i = 0;
			int num = first % 8;
			int num2 = first / 8;
			while (i < count)
			{
				if (num == 0 && count - i >= 8)
				{
					if (this._data[this._offset + num2] != 255)
					{
						this._data[this._offset + num2] = byte.MaxValue;
						result = true;
					}
					i += 8;
					num2++;
				}
				else
				{
					if (((int)this._data[this._offset + num2] & 1 << num) == 0)
					{
						byte[] data = this._data;
						int num3 = this._offset + num2;
						data[num3] |= (byte)(1 << num);
						result = true;
					}
					i++;
					num++;
					if (num == 8)
					{
						num = 0;
						num2++;
					}
				}
			}
			return result;
		}

		// Token: 0x04000002 RID: 2
		private readonly byte[] _data;

		// Token: 0x04000003 RID: 3
		private readonly int _length;

		// Token: 0x04000004 RID: 4
		private readonly int _offset;
	}
}
