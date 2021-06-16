using System;
using System.Collections.Generic;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200006F RID: 111
	internal sealed class KeyValuePairTypeInfo<K, V> : TraceLoggingTypeInfo<KeyValuePair<K, V>>
	{
		// Token: 0x06000297 RID: 663 RVA: 0x0000E555 File Offset: 0x0000C755
		public KeyValuePairTypeInfo(List<Type> recursionCheck)
		{
			this.keyInfo = TraceLoggingTypeInfo<K>.GetInstance(recursionCheck);
			this.valueInfo = TraceLoggingTypeInfo<V>.GetInstance(recursionCheck);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000E578 File Offset: 0x0000C778
		public override void WriteMetadata(TraceLoggingMetadataCollector collector, string name, EventFieldFormat format)
		{
			TraceLoggingMetadataCollector collector2 = collector.AddGroup(name);
			this.keyInfo.WriteMetadata(collector2, "Key", EventFieldFormat.Default);
			this.valueInfo.WriteMetadata(collector2, "Value", format);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000E5B4 File Offset: 0x0000C7B4
		public override void WriteData(TraceLoggingDataCollector collector, ref KeyValuePair<K, V> value)
		{
			K key = value.Key;
			V value2 = value.Value;
			this.keyInfo.WriteData(collector, ref key);
			this.valueInfo.WriteData(collector, ref value2);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000E5EC File Offset: 0x0000C7EC
		public override object GetData(object value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			KeyValuePair<K, V> keyValuePair = (KeyValuePair<K, V>)value;
			dictionary.Add("Key", this.keyInfo.GetData(keyValuePair.Key));
			dictionary.Add("Value", this.valueInfo.GetData(keyValuePair.Value));
			return dictionary;
		}

		// Token: 0x04000136 RID: 310
		private readonly TraceLoggingTypeInfo<K> keyInfo;

		// Token: 0x04000137 RID: 311
		private readonly TraceLoggingTypeInfo<V> valueInfo;
	}
}
