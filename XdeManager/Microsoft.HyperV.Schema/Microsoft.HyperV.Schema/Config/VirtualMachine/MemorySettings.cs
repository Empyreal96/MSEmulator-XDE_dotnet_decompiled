using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VirtualMachine
{
	// Token: 0x02000104 RID: 260
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemorySettings
	{
		// Token: 0x06000419 RID: 1049 RVA: 0x0000DC27 File Offset: 0x0000BE27
		public static bool IsJsonDefault(MemorySettings val)
		{
			return MemorySettings._default.JsonEquals(val);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000DC34 File Offset: 0x0000BE34
		public bool JsonEquals(object obj)
		{
			MemorySettings graph = obj as MemorySettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemorySettings), new DataContractJsonSerializerSettings
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

		// Token: 0x0400051C RID: 1308
		private static readonly MemorySettings _default = new MemorySettings();

		// Token: 0x0400051D RID: 1309
		[DataMember]
		public bool NumaSpanningEnabled;

		// Token: 0x0400051E RID: 1310
		[DataMember]
		public byte AutoStartSlpProperty;

		// Token: 0x0400051F RID: 1311
		[DataMember(EmitDefaultValue = false)]
		public ulong MemoryUsedInMb;

		// Token: 0x04000520 RID: 1312
		[DataMember(EmitDefaultValue = false)]
		public ulong BallonedPageCount;

		// Token: 0x04000521 RID: 1313
		[DataMember(EmitDefaultValue = false)]
		public ulong InitialPageCount;

		// Token: 0x04000522 RID: 1314
		[DataMember(EmitDefaultValue = false)]
		public byte VirtualNodeCount;

		// Token: 0x04000523 RID: 1315
		[DataMember(EmitDefaultValue = false)]
		public ulong[] BallonedPagesByVirtualNode;

		// Token: 0x04000524 RID: 1316
		[DataMember(EmitDefaultValue = false)]
		public ulong[] ActualPagesByVirtualNode;

		// Token: 0x04000525 RID: 1317
		[DataMember(EmitDefaultValue = false)]
		public byte[] VirtualToPhysicalNode;
	}
}
