using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000023 RID: 35
	internal class TraceLoggingEventTypes
	{
		// Token: 0x06000144 RID: 324 RVA: 0x0000A38C File Offset: 0x0000858C
		internal TraceLoggingEventTypes(string name, EventTags tags, params Type[] types) : this(tags, name, TraceLoggingEventTypes.MakeArray(types))
		{
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000A39C File Offset: 0x0000859C
		internal TraceLoggingEventTypes(string name, EventTags tags, params TraceLoggingTypeInfo[] typeInfos) : this(tags, name, TraceLoggingEventTypes.MakeArray(typeInfos))
		{
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000A3AC File Offset: 0x000085AC
		internal TraceLoggingEventTypes(string name, EventTags tags, ParameterInfo[] paramInfos)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.typeInfos = this.MakeArray(paramInfos);
			this.name = name;
			this.tags = tags;
			this.level = 5;
			TraceLoggingMetadataCollector traceLoggingMetadataCollector = new TraceLoggingMetadataCollector();
			checked
			{
				for (int i = 0; i < this.typeInfos.Length; i++)
				{
					TraceLoggingTypeInfo traceLoggingTypeInfo = this.typeInfos[i];
					this.level = Statics.Combine((int)traceLoggingTypeInfo.Level, this.level);
					this.opcode = Statics.Combine((int)traceLoggingTypeInfo.Opcode, this.opcode);
					this.keywords |= traceLoggingTypeInfo.Keywords;
					string fieldName = paramInfos[i].Name;
					if (Statics.ShouldOverrideFieldName(fieldName))
					{
						fieldName = traceLoggingTypeInfo.Name;
					}
					traceLoggingTypeInfo.WriteMetadata(traceLoggingMetadataCollector, fieldName, EventFieldFormat.Default);
				}
				this.typeMetadata = traceLoggingMetadataCollector.GetMetadata();
				this.scratchSize = traceLoggingMetadataCollector.ScratchSize;
				this.dataCount = traceLoggingMetadataCollector.DataCount;
				this.pinCount = traceLoggingMetadataCollector.PinCount;
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000A4A4 File Offset: 0x000086A4
		private TraceLoggingEventTypes(EventTags tags, string defaultName, TraceLoggingTypeInfo[] typeInfos)
		{
			if (defaultName == null)
			{
				throw new ArgumentNullException("defaultName");
			}
			this.typeInfos = typeInfos;
			this.name = defaultName;
			this.tags = tags;
			this.level = 5;
			TraceLoggingMetadataCollector traceLoggingMetadataCollector = new TraceLoggingMetadataCollector();
			foreach (TraceLoggingTypeInfo traceLoggingTypeInfo in typeInfos)
			{
				this.level = Statics.Combine((int)traceLoggingTypeInfo.Level, this.level);
				this.opcode = Statics.Combine((int)traceLoggingTypeInfo.Opcode, this.opcode);
				this.keywords |= traceLoggingTypeInfo.Keywords;
				traceLoggingTypeInfo.WriteMetadata(traceLoggingMetadataCollector, null, EventFieldFormat.Default);
			}
			this.typeMetadata = traceLoggingMetadataCollector.GetMetadata();
			this.scratchSize = traceLoggingMetadataCollector.ScratchSize;
			this.dataCount = traceLoggingMetadataCollector.DataCount;
			this.pinCount = traceLoggingMetadataCollector.PinCount;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000A575 File Offset: 0x00008775
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000A57D File Offset: 0x0000877D
		internal EventLevel Level
		{
			get
			{
				return (EventLevel)this.level;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0000A585 File Offset: 0x00008785
		internal EventOpcode Opcode
		{
			get
			{
				return (EventOpcode)this.opcode;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000A58D File Offset: 0x0000878D
		internal EventKeywords Keywords
		{
			get
			{
				return this.keywords;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000A595 File Offset: 0x00008795
		internal EventTags Tags
		{
			get
			{
				return this.tags;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000A5A0 File Offset: 0x000087A0
		internal NameInfo GetNameInfo(string name, EventTags tags)
		{
			NameInfo nameInfo = this.nameInfos.TryGet(new KeyValuePair<string, EventTags>(name, tags));
			if (nameInfo == null)
			{
				nameInfo = this.nameInfos.GetOrAdd(new NameInfo(name, tags, this.typeMetadata.Length));
			}
			return nameInfo;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000A5E0 File Offset: 0x000087E0
		private TraceLoggingTypeInfo[] MakeArray(ParameterInfo[] paramInfos)
		{
			if (paramInfos == null)
			{
				throw new ArgumentNullException("paramInfos");
			}
			List<Type> recursionCheck = new List<Type>(paramInfos.Length);
			TraceLoggingTypeInfo[] array = new TraceLoggingTypeInfo[paramInfos.Length];
			checked
			{
				for (int i = 0; i < paramInfos.Length; i++)
				{
					array[i] = Statics.GetTypeInfoInstance(paramInfos[i].ParameterType, recursionCheck);
				}
				return array;
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000A630 File Offset: 0x00008830
		private static TraceLoggingTypeInfo[] MakeArray(Type[] types)
		{
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			List<Type> recursionCheck = new List<Type>(types.Length);
			TraceLoggingTypeInfo[] array = new TraceLoggingTypeInfo[types.Length];
			checked
			{
				for (int i = 0; i < types.Length; i++)
				{
					array[i] = Statics.GetTypeInfoInstance(types[i], recursionCheck);
				}
				return array;
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000A678 File Offset: 0x00008878
		private static TraceLoggingTypeInfo[] MakeArray(TraceLoggingTypeInfo[] typeInfos)
		{
			if (typeInfos == null)
			{
				throw new ArgumentNullException("typeInfos");
			}
			return (TraceLoggingTypeInfo[])typeInfos.Clone();
		}

		// Token: 0x040000A2 RID: 162
		internal readonly TraceLoggingTypeInfo[] typeInfos;

		// Token: 0x040000A3 RID: 163
		internal readonly string name;

		// Token: 0x040000A4 RID: 164
		internal readonly EventTags tags;

		// Token: 0x040000A5 RID: 165
		internal readonly byte level;

		// Token: 0x040000A6 RID: 166
		internal readonly byte opcode;

		// Token: 0x040000A7 RID: 167
		internal readonly EventKeywords keywords;

		// Token: 0x040000A8 RID: 168
		internal readonly byte[] typeMetadata;

		// Token: 0x040000A9 RID: 169
		internal readonly int scratchSize;

		// Token: 0x040000AA RID: 170
		internal readonly int dataCount;

		// Token: 0x040000AB RID: 171
		internal readonly int pinCount;

		// Token: 0x040000AC RID: 172
		private ConcurrentSet<KeyValuePair<string, EventTags>, NameInfo> nameInfos;
	}
}
