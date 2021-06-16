using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200012E RID: 302
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class IdeStorageDevice
	{
		// Token: 0x060004BD RID: 1213 RVA: 0x0000F8E0 File Offset: 0x0000DAE0
		public static bool IsJsonDefault(IdeStorageDevice val)
		{
			return IdeStorageDevice._default.JsonEquals(val);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000F8F0 File Offset: 0x0000DAF0
		public bool JsonEquals(object obj)
		{
			IdeStorageDevice graph = obj as IdeStorageDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(IdeStorageDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x04000632 RID: 1586
		private static readonly IdeStorageDevice _default = new IdeStorageDevice();

		// Token: 0x04000633 RID: 1587
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000634 RID: 1588
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x04000635 RID: 1589
		[DataMember(Name = "controller")]
		public IdeController[] Controllers;
	}
}
