using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.VirtualMachines.Resources.Storage;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200011F RID: 287
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtualSMB
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x0000EBF8 File Offset: 0x0000CDF8
		public static bool IsJsonDefault(VirtualSMB val)
		{
			return VirtualSMB._default.JsonEquals(val);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000EC08 File Offset: 0x0000CE08
		public bool JsonEquals(object obj)
		{
			VirtualSMB graph = obj as VirtualSMB;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtualSMB), new DataContractJsonSerializerSettings
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

		// Token: 0x040005B8 RID: 1464
		private static readonly VirtualSMB _default = new VirtualSMB();

		// Token: 0x040005B9 RID: 1465
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x040005BA RID: 1466
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x040005BB RID: 1467
		[DataMember]
		public VirtualSmbShare[] VSmbShares;

		// Token: 0x040005BC RID: 1468
		[DataMember]
		public Plan9Share[] Plan9Shares;

		// Token: 0x040005BD RID: 1469
		[DataMember]
		public bool VMBFSOnly;
	}
}
