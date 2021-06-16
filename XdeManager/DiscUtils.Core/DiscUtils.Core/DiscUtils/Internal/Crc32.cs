using System;

namespace DiscUtils.Internal
{
	// Token: 0x0200006B RID: 107
	internal abstract class Crc32
	{
		// Token: 0x06000401 RID: 1025 RVA: 0x0000C125 File Offset: 0x0000A325
		protected Crc32(uint[] table)
		{
			this.Table = table;
			this._value = uint.MaxValue;
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0000C13B File Offset: 0x0000A33B
		public uint Value
		{
			get
			{
				return this._value ^ uint.MaxValue;
			}
		}

		// Token: 0x06000403 RID: 1027
		public abstract void Process(byte[] buffer, int offset, int count);

		// Token: 0x04000179 RID: 377
		protected readonly uint[] Table;

		// Token: 0x0400017A RID: 378
		protected uint _value;
	}
}
