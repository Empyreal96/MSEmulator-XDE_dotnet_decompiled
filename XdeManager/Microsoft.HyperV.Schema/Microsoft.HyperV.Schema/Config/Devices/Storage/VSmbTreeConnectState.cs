using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000126 RID: 294
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VSmbTreeConnectState
	{
		// Token: 0x06000495 RID: 1173 RVA: 0x0000F1C8 File Offset: 0x0000D3C8
		public static bool IsJsonDefault(VSmbTreeConnectState val)
		{
			return VSmbTreeConnectState._default.JsonEquals(val);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000F1D8 File Offset: 0x0000D3D8
		public bool JsonEquals(object obj)
		{
			VSmbTreeConnectState graph = obj as VSmbTreeConnectState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VSmbTreeConnectState), new DataContractJsonSerializerSettings
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

		// Token: 0x040005EA RID: 1514
		private static readonly VSmbTreeConnectState _default = new VSmbTreeConnectState();

		// Token: 0x040005EB RID: 1515
		[DataMember]
		public uint TreeId;

		// Token: 0x040005EC RID: 1516
		[DataMember]
		public string ShareName;

		// Token: 0x040005ED RID: 1517
		[DataMember]
		public uint ShareFlags;

		// Token: 0x040005EE RID: 1518
		[DataMember]
		public byte ShareType;
	}
}
