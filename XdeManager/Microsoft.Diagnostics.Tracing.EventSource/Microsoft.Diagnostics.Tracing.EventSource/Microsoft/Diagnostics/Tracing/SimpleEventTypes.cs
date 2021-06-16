using System;
using System.Threading;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000024 RID: 36
	internal class SimpleEventTypes<T> : TraceLoggingEventTypes
	{
		// Token: 0x06000151 RID: 337 RVA: 0x0000A694 File Offset: 0x00008894
		private SimpleEventTypes(TraceLoggingTypeInfo<T> typeInfo) : base(typeInfo.Name, typeInfo.Tags, new TraceLoggingTypeInfo[]
		{
			typeInfo
		})
		{
			this.typeInfo = typeInfo;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000A6C6 File Offset: 0x000088C6
		public static SimpleEventTypes<T> Instance
		{
			get
			{
				return SimpleEventTypes<T>.instance ?? SimpleEventTypes<T>.InitInstance();
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000A6D8 File Offset: 0x000088D8
		private static SimpleEventTypes<T> InitInstance()
		{
			SimpleEventTypes<T> value = new SimpleEventTypes<T>(TraceLoggingTypeInfo<T>.Instance);
			Interlocked.CompareExchange<SimpleEventTypes<T>>(ref SimpleEventTypes<T>.instance, value, null);
			return SimpleEventTypes<T>.instance;
		}

		// Token: 0x040000AD RID: 173
		private static SimpleEventTypes<T> instance;

		// Token: 0x040000AE RID: 174
		internal readonly TraceLoggingTypeInfo<T> typeInfo;
	}
}
