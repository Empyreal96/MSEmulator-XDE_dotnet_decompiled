using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Compute.Error;

namespace HCS.Compute.System
{
	// Token: 0x020001A2 RID: 418
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ResultError
	{
		// Token: 0x060006BD RID: 1725 RVA: 0x000155E9 File Offset: 0x000137E9
		public static bool IsJsonDefault(ResultError val)
		{
			return ResultError._default.JsonEquals(val);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x000155F8 File Offset: 0x000137F8
		public bool JsonEquals(object obj)
		{
			ResultError graph = obj as ResultError;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ResultError), new DataContractJsonSerializerSettings
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

		// Token: 0x04000942 RID: 2370
		private static readonly ResultError _default = new ResultError();

		// Token: 0x04000943 RID: 2371
		[DataMember]
		public int Error;

		// Token: 0x04000944 RID: 2372
		[DataMember]
		public string ErrorMessage;

		// Token: 0x04000945 RID: 2373
		[DataMember(EmitDefaultValue = false)]
		public ErrorEvent[] ErrorEvents;
	}
}
