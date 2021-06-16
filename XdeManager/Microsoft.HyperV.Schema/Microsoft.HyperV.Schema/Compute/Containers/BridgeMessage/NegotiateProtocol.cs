using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001AF RID: 431
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class NegotiateProtocol
	{
		// Token: 0x060006F3 RID: 1779 RVA: 0x00015F7C File Offset: 0x0001417C
		public static bool IsJsonDefault(NegotiateProtocol val)
		{
			return NegotiateProtocol._default.JsonEquals(val);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00015F8C File Offset: 0x0001418C
		public bool JsonEquals(object obj)
		{
			NegotiateProtocol graph = obj as NegotiateProtocol;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(NegotiateProtocol), new DataContractJsonSerializerSettings
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

		// Token: 0x04000990 RID: 2448
		private static readonly NegotiateProtocol _default = new NegotiateProtocol();

		// Token: 0x04000991 RID: 2449
		[DataMember]
		public string ContainerId;

		// Token: 0x04000992 RID: 2450
		[DataMember]
		public Guid ActivityId;

		// Token: 0x04000993 RID: 2451
		[DataMember(IsRequired = true)]
		public uint MinimumVersion;

		// Token: 0x04000994 RID: 2452
		[DataMember(IsRequired = true)]
		public uint MaximumVersion;
	}
}
