using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Vpci
{
	// Token: 0x020000A6 RID: 166
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Allocation
	{
		// Token: 0x06000289 RID: 649 RVA: 0x0000968C File Offset: 0x0000788C
		public static bool IsJsonDefault(Allocation val)
		{
			return Allocation._default.JsonEquals(val);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000969C File Offset: 0x0000789C
		public bool JsonEquals(object obj)
		{
			Allocation graph = obj as Allocation;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Allocation), new DataContractJsonSerializerSettings
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

		// Token: 0x04000358 RID: 856
		private static readonly Allocation _default = new Allocation();

		// Token: 0x04000359 RID: 857
		[DataMember]
		public DeviceInstance[] Devices;
	}
}
