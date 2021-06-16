using System;

namespace CommandLine
{
	// Token: 0x02000049 RID: 73
	public abstract class ParserResult<T>
	{
		// Token: 0x06000180 RID: 384 RVA: 0x00006470 File Offset: 0x00004670
		internal ParserResult(ParserResultType tag, TypeInfo typeInfo)
		{
			this.tag = tag;
			this.typeInfo = typeInfo;
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00006486 File Offset: 0x00004686
		public ParserResultType Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000648E File Offset: 0x0000468E
		public TypeInfo TypeInfo
		{
			get
			{
				return this.typeInfo;
			}
		}

		// Token: 0x04000075 RID: 117
		private readonly ParserResultType tag;

		// Token: 0x04000076 RID: 118
		private readonly TypeInfo typeInfo;
	}
}
