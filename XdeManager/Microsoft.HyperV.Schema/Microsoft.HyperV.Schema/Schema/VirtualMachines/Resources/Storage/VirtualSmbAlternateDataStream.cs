using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x0200003A RID: 58
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtualSmbAlternateDataStream
	{
		// Token: 0x060000DF RID: 223 RVA: 0x000047B8 File Offset: 0x000029B8
		public static bool IsJsonDefault(VirtualSmbAlternateDataStream val)
		{
			return VirtualSmbAlternateDataStream._default.JsonEquals(val);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000047C8 File Offset: 0x000029C8
		public bool JsonEquals(object obj)
		{
			VirtualSmbAlternateDataStream graph = obj as VirtualSmbAlternateDataStream;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtualSmbAlternateDataStream), new DataContractJsonSerializerSettings
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

		// Token: 0x0400012B RID: 299
		private static readonly VirtualSmbAlternateDataStream _default = new VirtualSmbAlternateDataStream();

		// Token: 0x0400012C RID: 300
		[DataMember]
		public string Name;

		// Token: 0x0400012D RID: 301
		[DataMember(EmitDefaultValue = false)]
		public byte[] Contents;
	}
}
