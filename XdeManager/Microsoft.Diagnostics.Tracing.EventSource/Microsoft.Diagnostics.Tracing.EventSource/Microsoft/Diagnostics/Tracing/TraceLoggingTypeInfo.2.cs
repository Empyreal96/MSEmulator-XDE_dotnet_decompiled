using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000017 RID: 23
	internal abstract class TraceLoggingTypeInfo<DataType> : TraceLoggingTypeInfo
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x00009595 File Offset: 0x00007795
		protected TraceLoggingTypeInfo() : base(typeof(DataType))
		{
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000095A7 File Offset: 0x000077A7
		protected TraceLoggingTypeInfo(string name, EventLevel level, EventOpcode opcode, EventKeywords keywords, EventTags tags) : base(typeof(DataType), name, level, opcode, keywords, tags)
		{
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000095C0 File Offset: 0x000077C0
		public static TraceLoggingTypeInfo<DataType> Instance
		{
			get
			{
				return TraceLoggingTypeInfo<DataType>.instance ?? TraceLoggingTypeInfo<DataType>.InitInstance();
			}
		}

		// Token: 0x060000FA RID: 250
		public abstract void WriteData(TraceLoggingDataCollector collector, ref DataType value);

		// Token: 0x060000FB RID: 251 RVA: 0x000095D0 File Offset: 0x000077D0
		public override void WriteObjectData(TraceLoggingDataCollector collector, object value)
		{
			DataType dataType = (value == null) ? default(DataType) : ((DataType)((object)value));
			this.WriteData(collector, ref dataType);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000095FC File Offset: 0x000077FC
		internal static TraceLoggingTypeInfo<DataType> GetInstance(List<Type> recursionCheck)
		{
			if (TraceLoggingTypeInfo<DataType>.instance == null)
			{
				int count = recursionCheck.Count;
				TraceLoggingTypeInfo<DataType> value = Statics.CreateDefaultTypeInfo<DataType>(recursionCheck);
				Interlocked.CompareExchange<TraceLoggingTypeInfo<DataType>>(ref TraceLoggingTypeInfo<DataType>.instance, value, null);
				recursionCheck.RemoveRange(count, checked(recursionCheck.Count - count));
			}
			return TraceLoggingTypeInfo<DataType>.instance;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000963F File Offset: 0x0000783F
		private static TraceLoggingTypeInfo<DataType> InitInstance()
		{
			return TraceLoggingTypeInfo<DataType>.GetInstance(new List<Type>());
		}

		// Token: 0x04000086 RID: 134
		private static TraceLoggingTypeInfo<DataType> instance;
	}
}
