using System;
using DiscUtils.Streams;

namespace DiscUtils
{
	// Token: 0x0200000E RID: 14
	public sealed class DiskImageFileSpecification
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00002BC4 File Offset: 0x00000DC4
		internal DiskImageFileSpecification(string name, StreamBuilder builder)
		{
			this.Name = name;
			this._builder = builder;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00002BDA File Offset: 0x00000DDA
		public string Name { get; }

		// Token: 0x060000A6 RID: 166 RVA: 0x00002BE2 File Offset: 0x00000DE2
		public SparseStream OpenStream()
		{
			return this._builder.Build();
		}

		// Token: 0x0400001C RID: 28
		private readonly StreamBuilder _builder;
	}
}
