using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Video
{
	// Token: 0x02000116 RID: 278
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class S3Display
	{
		// Token: 0x0600046D RID: 1133 RVA: 0x0000EA62 File Offset: 0x0000CC62
		public static bool IsJsonDefault(S3Display val)
		{
			return S3Display._default.JsonEquals(val);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000EA70 File Offset: 0x0000CC70
		public bool JsonEquals(object obj)
		{
			S3Display graph = obj as S3Display;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(S3Display), new DataContractJsonSerializerSettings
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

		// Token: 0x04000585 RID: 1413
		private static readonly S3Display _default = new S3Display();

		// Token: 0x04000586 RID: 1414
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x04000587 RID: 1415
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x04000588 RID: 1416
		[DataMember(Name = "address")]
		public string Address;
	}
}
