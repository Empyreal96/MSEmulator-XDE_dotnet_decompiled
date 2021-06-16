using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200002C RID: 44
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class EventAttribute : Attribute
	{
		// Token: 0x06000192 RID: 402 RVA: 0x0000B1F0 File Offset: 0x000093F0
		public EventAttribute(int eventId)
		{
			this.EventId = eventId;
			this.Level = EventLevel.Informational;
			this.m_opcodeSet = false;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000B20D File Offset: 0x0000940D
		// (set) Token: 0x06000194 RID: 404 RVA: 0x0000B215 File Offset: 0x00009415
		public int EventId { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000B21E File Offset: 0x0000941E
		// (set) Token: 0x06000196 RID: 406 RVA: 0x0000B226 File Offset: 0x00009426
		public EventLevel Level { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000B22F File Offset: 0x0000942F
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000B237 File Offset: 0x00009437
		public EventKeywords Keywords { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000B240 File Offset: 0x00009440
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000B248 File Offset: 0x00009448
		public EventOpcode Opcode
		{
			get
			{
				return this.m_opcode;
			}
			set
			{
				this.m_opcode = value;
				this.m_opcodeSet = true;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000B258 File Offset: 0x00009458
		internal bool IsOpcodeSet
		{
			get
			{
				return this.m_opcodeSet;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000B260 File Offset: 0x00009460
		// (set) Token: 0x0600019D RID: 413 RVA: 0x0000B268 File Offset: 0x00009468
		public EventTask Task { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000B271 File Offset: 0x00009471
		// (set) Token: 0x0600019F RID: 415 RVA: 0x0000B279 File Offset: 0x00009479
		public EventChannel Channel { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000B282 File Offset: 0x00009482
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x0000B28A File Offset: 0x0000948A
		public byte Version { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000B293 File Offset: 0x00009493
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x0000B29B File Offset: 0x0000949B
		public string Message { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000B2A4 File Offset: 0x000094A4
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x0000B2AC File Offset: 0x000094AC
		public EventTags Tags { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000B2B5 File Offset: 0x000094B5
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x0000B2BD File Offset: 0x000094BD
		public EventActivityOptions ActivityOptions { get; set; }

		// Token: 0x040000DA RID: 218
		private EventOpcode m_opcode;

		// Token: 0x040000DB RID: 219
		private bool m_opcodeSet;
	}
}
