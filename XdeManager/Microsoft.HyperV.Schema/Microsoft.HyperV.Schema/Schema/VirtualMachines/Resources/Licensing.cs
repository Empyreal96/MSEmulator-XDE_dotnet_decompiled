using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000025 RID: 37
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Licensing
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00003B38 File Offset: 0x00001D38
		public static bool IsJsonDefault(Licensing val)
		{
			return Licensing._default.JsonEquals(val);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003B48 File Offset: 0x00001D48
		public bool JsonEquals(object obj)
		{
			Licensing graph = obj as Licensing;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Licensing), new DataContractJsonSerializerSettings
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

		// Token: 0x040000AA RID: 170
		private static readonly Licensing _default = new Licensing();

		// Token: 0x040000AB RID: 171
		[DataMember]
		public Guid ContainerID;

		// Token: 0x040000AC RID: 172
		[DataMember]
		public string[] PackageFamilyNames;
	}
}
