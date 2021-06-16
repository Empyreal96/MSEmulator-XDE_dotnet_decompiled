using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Serial
{
	// Token: 0x0200013F RID: 319
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Port
	{
		// Token: 0x0600050F RID: 1295 RVA: 0x0001080B File Offset: 0x0000EA0B
		public static bool IsJsonDefault(Port val)
		{
			return Port._default.JsonEquals(val);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00010818 File Offset: 0x0000EA18
		public bool JsonEquals(object obj)
		{
			Port graph = obj as Port;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Port), new DataContractJsonSerializerSettings
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

		// Token: 0x04000680 RID: 1664
		private static readonly Port _default = new Port();

		// Token: 0x04000681 RID: 1665
		[DataMember(Name = "connection")]
		public string Connection;

		// Token: 0x04000682 RID: 1666
		[DataMember(EmitDefaultValue = false)]
		public bool? DebuggerMode;

		// Token: 0x04000683 RID: 1667
		[DataMember(EmitDefaultValue = false)]
		public bool ForceEnable;

		// Token: 0x04000684 RID: 1668
		[DataMember(EmitDefaultValue = false)]
		public uint InputBufferSize;

		// Token: 0x04000685 RID: 1669
		[DataMember(EmitDefaultValue = false)]
		public bool IsPipeServer;
	}
}
