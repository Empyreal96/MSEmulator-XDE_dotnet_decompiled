using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000128 RID: 296
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VsmbDirectMapSectionState
	{
		// Token: 0x0600049D RID: 1181 RVA: 0x0000F360 File Offset: 0x0000D560
		public static bool IsJsonDefault(VsmbDirectMapSectionState val)
		{
			return VsmbDirectMapSectionState._default.JsonEquals(val);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000F370 File Offset: 0x0000D570
		public bool JsonEquals(object obj)
		{
			VsmbDirectMapSectionState graph = obj as VsmbDirectMapSectionState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VsmbDirectMapSectionState), new DataContractJsonSerializerSettings
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

		// Token: 0x040005FD RID: 1533
		private static readonly VsmbDirectMapSectionState _default = new VsmbDirectMapSectionState();

		// Token: 0x040005FE RID: 1534
		[DataMember]
		public ulong OnDiskFileId;

		// Token: 0x040005FF RID: 1535
		[DataMember]
		public ulong BaseAddress;

		// Token: 0x04000600 RID: 1536
		[DataMember]
		public ulong PageCount;

		// Token: 0x04000601 RID: 1537
		[DataMember]
		public ulong GpaPageIndex;

		// Token: 0x04000602 RID: 1538
		[DataMember]
		public ulong OriginalImageBase;

		// Token: 0x04000603 RID: 1539
		[DataMember]
		public byte VirtualNode;

		// Token: 0x04000604 RID: 1540
		[DataMember]
		public byte[] WorkingSetBitmap;
	}
}
