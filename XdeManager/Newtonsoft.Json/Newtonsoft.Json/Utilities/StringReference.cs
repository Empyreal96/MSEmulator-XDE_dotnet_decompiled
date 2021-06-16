using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000063 RID: 99
	internal readonly struct StringReference
	{
		// Token: 0x170000CF RID: 207
		public char this[int i]
		{
			get
			{
				return this._chars[i];
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x000192E3 File Offset: 0x000174E3
		public char[] Chars
		{
			get
			{
				return this._chars;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x000192EB File Offset: 0x000174EB
		public int StartIndex
		{
			get
			{
				return this._startIndex;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x000192F3 File Offset: 0x000174F3
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000192FB File Offset: 0x000174FB
		public StringReference(char[] chars, int startIndex, int length)
		{
			this._chars = chars;
			this._startIndex = startIndex;
			this._length = length;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00019312 File Offset: 0x00017512
		public override string ToString()
		{
			return new string(this._chars, this._startIndex, this._length);
		}

		// Token: 0x040001FE RID: 510
		private readonly char[] _chars;

		// Token: 0x040001FF RID: 511
		private readonly int _startIndex;

		// Token: 0x04000200 RID: 512
		private readonly int _length;
	}
}
