using System;

namespace CommandLine.Text
{
	// Token: 0x0200005D RID: 93
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class UsageAttribute : Attribute
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000A162 File Offset: 0x00008362
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000A16A File Offset: 0x0000836A
		public string ApplicationAlias { get; set; }
	}
}
