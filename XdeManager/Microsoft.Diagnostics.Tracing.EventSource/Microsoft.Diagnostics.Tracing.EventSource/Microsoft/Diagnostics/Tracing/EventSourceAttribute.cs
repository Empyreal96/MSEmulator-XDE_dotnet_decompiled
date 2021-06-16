using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200002B RID: 43
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class EventSourceAttribute : Attribute
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000B1B5 File Offset: 0x000093B5
		// (set) Token: 0x0600018C RID: 396 RVA: 0x0000B1BD File Offset: 0x000093BD
		public string Name { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000B1C6 File Offset: 0x000093C6
		// (set) Token: 0x0600018E RID: 398 RVA: 0x0000B1CE File Offset: 0x000093CE
		public string Guid { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600018F RID: 399 RVA: 0x0000B1D7 File Offset: 0x000093D7
		// (set) Token: 0x06000190 RID: 400 RVA: 0x0000B1DF File Offset: 0x000093DF
		public string LocalizationResources { get; set; }
	}
}
