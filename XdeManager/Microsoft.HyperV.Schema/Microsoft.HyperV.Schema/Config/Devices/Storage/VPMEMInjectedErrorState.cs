using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x02000135 RID: 309
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMInjectedErrorState
	{
		// Token: 0x060004E1 RID: 1249 RVA: 0x0000FF6F File Offset: 0x0000E16F
		public static bool IsJsonDefault(VPMEMInjectedErrorState val)
		{
			return VPMEMInjectedErrorState._default.JsonEquals(val);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0000FF7C File Offset: 0x0000E17C
		public bool JsonEquals(object obj)
		{
			VPMEMInjectedErrorState graph = obj as VPMEMInjectedErrorState;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMInjectedErrorState), new DataContractJsonSerializerSettings
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

		// Token: 0x0400064F RID: 1615
		private static readonly VPMEMInjectedErrorState _default = new VPMEMInjectedErrorState();

		// Token: 0x04000650 RID: 1616
		[DataMember]
		public uint InjectedErrorsAsUlong;

		// Token: 0x04000651 RID: 1617
		[DataMember]
		public uint InjectedUnsafeShutdownCount;
	}
}
