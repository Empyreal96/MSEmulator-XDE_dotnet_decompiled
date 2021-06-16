using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.System
{
	// Token: 0x020001A3 RID: 419
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SiloJobCreatedNotification
	{
		// Token: 0x060006C1 RID: 1729 RVA: 0x000156B4 File Offset: 0x000138B4
		public static bool IsJsonDefault(SiloJobCreatedNotification val)
		{
			return SiloJobCreatedNotification._default.JsonEquals(val);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000156C4 File Offset: 0x000138C4
		public bool JsonEquals(object obj)
		{
			SiloJobCreatedNotification graph = obj as SiloJobCreatedNotification;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SiloJobCreatedNotification), new DataContractJsonSerializerSettings
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

		// Token: 0x04000946 RID: 2374
		private static readonly SiloJobCreatedNotification _default = new SiloJobCreatedNotification();

		// Token: 0x04000947 RID: 2375
		[DataMember]
		public string JobName;
	}
}
