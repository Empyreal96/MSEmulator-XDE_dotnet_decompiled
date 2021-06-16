using System;

namespace CommandLine
{
	// Token: 0x0200003B RID: 59
	public sealed class HelpVerbRequestedError : Error
	{
		// Token: 0x06000136 RID: 310 RVA: 0x0000534D File Offset: 0x0000354D
		internal HelpVerbRequestedError(string verb, Type type, bool matched) : base(ErrorType.HelpVerbRequestedError, true)
		{
			this.verb = verb;
			this.type = type;
			this.matched = matched;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000536D File Offset: 0x0000356D
		public string Verb
		{
			get
			{
				return this.verb;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00005375 File Offset: 0x00003575
		public Type Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000537D File Offset: 0x0000357D
		public bool Matched
		{
			get
			{
				return this.matched;
			}
		}

		// Token: 0x04000060 RID: 96
		private readonly string verb;

		// Token: 0x04000061 RID: 97
		private readonly Type type;

		// Token: 0x04000062 RID: 98
		private readonly bool matched;
	}
}
