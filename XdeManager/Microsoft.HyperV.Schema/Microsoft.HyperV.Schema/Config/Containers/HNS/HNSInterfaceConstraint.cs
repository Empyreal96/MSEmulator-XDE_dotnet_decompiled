using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers.HNS
{
	// Token: 0x0200018C RID: 396
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HNSInterfaceConstraint
	{
		// Token: 0x06000657 RID: 1623 RVA: 0x000142D0 File Offset: 0x000124D0
		public static bool IsJsonDefault(HNSInterfaceConstraint val)
		{
			return HNSInterfaceConstraint._default.JsonEquals(val);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x000142E0 File Offset: 0x000124E0
		public bool JsonEquals(object obj)
		{
			HNSInterfaceConstraint graph = obj as HNSInterfaceConstraint;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HNSInterfaceConstraint), new DataContractJsonSerializerSettings
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

		// Token: 0x04000882 RID: 2178
		private static readonly HNSInterfaceConstraint _default = new HNSInterfaceConstraint();

		// Token: 0x04000883 RID: 2179
		[DataMember(EmitDefaultValue = false)]
		public Guid InterfaceGuid;

		// Token: 0x04000884 RID: 2180
		[DataMember(EmitDefaultValue = false)]
		public ulong InterfaceLuid;

		// Token: 0x04000885 RID: 2181
		[DataMember(EmitDefaultValue = false)]
		public uint InterfaceIndex;

		// Token: 0x04000886 RID: 2182
		[DataMember(EmitDefaultValue = false)]
		public uint InterfaceMediaType;

		// Token: 0x04000887 RID: 2183
		[DataMember(EmitDefaultValue = false)]
		public string InterfaceAlias;

		// Token: 0x04000888 RID: 2184
		[DataMember(EmitDefaultValue = false)]
		public string InterfaceDescription;
	}
}
