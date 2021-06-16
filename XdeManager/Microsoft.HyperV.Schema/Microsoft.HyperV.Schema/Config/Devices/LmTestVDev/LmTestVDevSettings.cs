using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.LmTestVDev
{
	// Token: 0x0200014B RID: 331
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class LmTestVDevSettings
	{
		// Token: 0x0600053B RID: 1339 RVA: 0x000110CC File Offset: 0x0000F2CC
		public static bool IsJsonDefault(LmTestVDevSettings val)
		{
			return LmTestVDevSettings._default.JsonEquals(val);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000110DC File Offset: 0x0000F2DC
		public bool JsonEquals(object obj)
		{
			LmTestVDevSettings graph = obj as LmTestVDevSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(LmTestVDevSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x040006D2 RID: 1746
		private static readonly LmTestVDevSettings _default = new LmTestVDevSettings();

		// Token: 0x040006D3 RID: 1747
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x040006D4 RID: 1748
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x040006D5 RID: 1749
		[DataMember]
		public string SourceHost;

		// Token: 0x040006D6 RID: 1750
		[DataMember]
		public Guid VmId;

		// Token: 0x040006D7 RID: 1751
		[DataMember]
		public uint SourceMethod;

		// Token: 0x040006D8 RID: 1752
		[DataMember]
		public uint DestMethod;
	}
}
