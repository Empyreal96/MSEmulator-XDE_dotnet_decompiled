using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Network
{
	// Token: 0x020000C2 RID: 194
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NicConnectionResource
	{
		// Token: 0x060002F7 RID: 759 RVA: 0x0000AB7C File Offset: 0x00008D7C
		public static bool IsJsonDefault(NicConnectionResource val)
		{
			return NicConnectionResource._default.JsonEquals(val);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000AB8C File Offset: 0x00008D8C
		public bool JsonEquals(object obj)
		{
			NicConnectionResource graph = obj as NicConnectionResource;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NicConnectionResource), new DataContractJsonSerializerSettings
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

		// Token: 0x040003C6 RID: 966
		private static readonly NicConnectionResource _default = new NicConnectionResource();

		// Token: 0x040003C7 RID: 967
		[DataMember(EmitDefaultValue = false)]
		public string SwitchName;

		// Token: 0x040003C8 RID: 968
		[DataMember(EmitDefaultValue = false)]
		public string PortName;

		// Token: 0x040003C9 RID: 969
		[DataMember]
		public bool IsEnabled;

		// Token: 0x040003CA RID: 970
		[DataMember]
		public uint Flags;

		// Token: 0x040003CB RID: 971
		[DataMember]
		public byte CardId;

		// Token: 0x040003CC RID: 972
		[DataMember]
		public uint IovOffloadWeight;

		// Token: 0x040003CD RID: 973
		[DataMember]
		public uint IovQueuePairsRequested;

		// Token: 0x040003CE RID: 974
		[DataMember]
		public uint InterruptModeration;
	}
}
