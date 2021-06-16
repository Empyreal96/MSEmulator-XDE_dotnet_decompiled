using System;

namespace CommandLine
{
	// Token: 0x02000035 RID: 53
	public sealed class MutuallyExclusiveSetError : NamedError
	{
		// Token: 0x0600012F RID: 303 RVA: 0x00005300 File Offset: 0x00003500
		internal MutuallyExclusiveSetError(NameInfo nameInfo, string setName) : base(ErrorType.MutuallyExclusiveSetError, nameInfo)
		{
			this.setName = setName;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00005311 File Offset: 0x00003511
		public string SetName
		{
			get
			{
				return this.setName;
			}
		}

		// Token: 0x0400005F RID: 95
		private readonly string setName;
	}
}
