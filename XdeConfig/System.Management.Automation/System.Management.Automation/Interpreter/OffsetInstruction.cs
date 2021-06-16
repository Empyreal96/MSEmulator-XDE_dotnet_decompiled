using System;
using System.Collections.Generic;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000681 RID: 1665
	internal abstract class OffsetInstruction : Instruction
	{
		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x060046BD RID: 18109 RVA: 0x0017A389 File Offset: 0x00178589
		public int Offset
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x060046BE RID: 18110
		public abstract Instruction[] Cache { get; }

		// Token: 0x060046BF RID: 18111 RVA: 0x0017A394 File Offset: 0x00178594
		public Instruction Fixup(int offset)
		{
			this._offset = offset;
			Instruction[] cache = this.Cache;
			if (cache != null && offset >= 0 && offset < cache.Length)
			{
				Instruction result;
				if ((result = cache[offset]) == null)
				{
					cache[offset] = this;
					result = this;
				}
				return result;
			}
			return this;
		}

		// Token: 0x060046C0 RID: 18112 RVA: 0x0017A3CC File Offset: 0x001785CC
		public override string ToDebugString(int instructionIndex, object cookie, Func<int, int> labelIndexer, IList<object> objects)
		{
			return this.ToString() + ((this._offset != int.MinValue) ? (" -> " + (instructionIndex + this._offset)) : "");
		}

		// Token: 0x060046C1 RID: 18113 RVA: 0x0017A404 File Offset: 0x00178604
		public override string ToString()
		{
			return this.InstructionName + ((this._offset == int.MinValue) ? "(?)" : ("(" + this._offset + ")"));
		}

		// Token: 0x040022BC RID: 8892
		internal const int Unknown = -2147483648;

		// Token: 0x040022BD RID: 8893
		internal const int CacheSize = 32;

		// Token: 0x040022BE RID: 8894
		protected int _offset = int.MinValue;
	}
}
