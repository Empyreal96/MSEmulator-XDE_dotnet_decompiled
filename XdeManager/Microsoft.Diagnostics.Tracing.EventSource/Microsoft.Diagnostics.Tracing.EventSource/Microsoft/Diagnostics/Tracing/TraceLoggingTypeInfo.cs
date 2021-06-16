using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000016 RID: 22
	internal abstract class TraceLoggingTypeInfo
	{
		// Token: 0x060000EC RID: 236 RVA: 0x000094AC File Offset: 0x000076AC
		internal TraceLoggingTypeInfo(Type dataType)
		{
			if (dataType == null)
			{
				throw new ArgumentNullException("dataType");
			}
			this.name = dataType.Name;
			this.dataType = dataType;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000094EC File Offset: 0x000076EC
		internal TraceLoggingTypeInfo(Type dataType, string name, EventLevel level, EventOpcode opcode, EventKeywords keywords, EventTags tags)
		{
			if (dataType == null)
			{
				throw new ArgumentNullException("dataType");
			}
			if (name == null)
			{
				throw new ArgumentNullException("eventName");
			}
			Statics.CheckName(name);
			this.name = name;
			this.keywords = keywords;
			this.level = level;
			this.opcode = opcode;
			this.tags = tags;
			this.dataType = dataType;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00009562 File Offset: 0x00007762
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000EF RID: 239 RVA: 0x0000956A File Offset: 0x0000776A
		public EventLevel Level
		{
			get
			{
				return this.level;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00009572 File Offset: 0x00007772
		public EventOpcode Opcode
		{
			get
			{
				return this.opcode;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x0000957A File Offset: 0x0000777A
		public EventKeywords Keywords
		{
			get
			{
				return this.keywords;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00009582 File Offset: 0x00007782
		public EventTags Tags
		{
			get
			{
				return this.tags;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x0000958A File Offset: 0x0000778A
		internal Type DataType
		{
			get
			{
				return this.dataType;
			}
		}

		// Token: 0x060000F4 RID: 244
		public abstract void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format);

		// Token: 0x060000F5 RID: 245
		public abstract void WriteObjectData(TraceLoggingDataCollector collector, object value);

		// Token: 0x060000F6 RID: 246 RVA: 0x00009592 File Offset: 0x00007792
		public virtual object GetData(object value)
		{
			return value;
		}

		// Token: 0x04000080 RID: 128
		private readonly string name;

		// Token: 0x04000081 RID: 129
		private readonly EventKeywords keywords;

		// Token: 0x04000082 RID: 130
		private readonly EventLevel level = (EventLevel)(-1);

		// Token: 0x04000083 RID: 131
		private readonly EventOpcode opcode = (EventOpcode)(-1);

		// Token: 0x04000084 RID: 132
		private readonly EventTags tags;

		// Token: 0x04000085 RID: 133
		private readonly Type dataType;
	}
}
