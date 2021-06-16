using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000FE RID: 254
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemoryParameters
	{
		// Token: 0x060003EB RID: 1003 RVA: 0x0000D5A0 File Offset: 0x0000B7A0
		public static bool IsJsonDefault(MemoryParameters val)
		{
			return MemoryParameters._default.JsonEquals(val);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000D5B0 File Offset: 0x0000B7B0
		public bool JsonEquals(object obj)
		{
			MemoryParameters graph = obj as MemoryParameters;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemoryParameters), new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true
			});
			bool result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					dataContractJsonSerializer.WriteObject(memoryStream, this);
					dataContractJsonSerializer.WriteObject(memoryStream2, graph);
					result = (Encoding.ASCII.GetString(memoryStream.ToArray()) == Encoding.ASCII.GetString(memoryStream2.ToArray()));
				}
			}
			return result;
		}

		// Token: 0x040004F3 RID: 1267
		private static readonly MemoryParameters _default = new MemoryParameters();

		// Token: 0x040004F4 RID: 1268
		[DataMember]
		public bool BalancingEnabled;

		// Token: 0x040004F5 RID: 1269
		[DataMember]
		public bool ConsolidationEnabled;

		// Token: 0x040004F6 RID: 1270
		[DataMember]
		public long MinPages;

		// Token: 0x040004F7 RID: 1271
		[DataMember]
		public long MaxPages;

		// Token: 0x040004F8 RID: 1272
		[DataMember]
		public int Priority;

		// Token: 0x040004F9 RID: 1273
		[DataMember]
		public int TargetMemoryBuffer;

		// Token: 0x040004FA RID: 1274
		[DataMember]
		public long AdditionalRootOverheadInPages;

		// Token: 0x040004FB RID: 1275
		[DataMember]
		public long WorkingSetLow;

		// Token: 0x040004FC RID: 1276
		[DataMember]
		public long WorkingSetHigh;

		// Token: 0x040004FD RID: 1277
		[DataMember]
		public long MemoryBackingType;
	}
}
