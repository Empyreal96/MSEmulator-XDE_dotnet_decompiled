using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200007B RID: 123
	[AttributeUsage(AttributeTargets.Property)]
	public class EventFieldAttribute : Attribute
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600030B RID: 779 RVA: 0x0000FA3D File Offset: 0x0000DC3D
		// (set) Token: 0x0600030C RID: 780 RVA: 0x0000FA45 File Offset: 0x0000DC45
		public EventFieldTags Tags { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000FA4E File Offset: 0x0000DC4E
		// (set) Token: 0x0600030E RID: 782 RVA: 0x0000FA56 File Offset: 0x0000DC56
		internal string Name { get; set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000FA5F File Offset: 0x0000DC5F
		// (set) Token: 0x06000310 RID: 784 RVA: 0x0000FA67 File Offset: 0x0000DC67
		public EventFieldFormat Format { get; set; }
	}
}
