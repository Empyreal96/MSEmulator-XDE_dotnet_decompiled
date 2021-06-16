using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000E5 RID: 229
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class PowerPolicies
	{
		// Token: 0x0600035F RID: 863 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
		public static bool IsJsonDefault(PowerPolicies val)
		{
			return PowerPolicies._default.JsonEquals(val);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000BEF8 File Offset: 0x0000A0F8
		public bool JsonEquals(object obj)
		{
			PowerPolicies graph = obj as PowerPolicies;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(PowerPolicies), new DataContractJsonSerializerSettings
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

		// Token: 0x04000463 RID: 1123
		private static readonly PowerPolicies _default = new PowerPolicies();

		// Token: 0x04000464 RID: 1124
		[DataMember(Name = "host_shutdown")]
		public HostShutdownPolicy ShutdownAction;

		// Token: 0x04000465 RID: 1125
		[DataMember(Name = "stop_on_reset")]
		public bool StopOnReset;
	}
}
