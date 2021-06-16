using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000FB RID: 251
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class IsolationSettings
	{
		// Token: 0x060003D1 RID: 977 RVA: 0x0000D238 File Offset: 0x0000B438
		public static bool IsJsonDefault(IsolationSettings val)
		{
			return IsolationSettings._default.JsonEquals(val);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000D248 File Offset: 0x0000B448
		public bool JsonEquals(object obj)
		{
			IsolationSettings graph = obj as IsolationSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(IsolationSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x040004E3 RID: 1251
		private static readonly IsolationSettings _default = new IsolationSettings();

		// Token: 0x040004E4 RID: 1252
		[DataMember(EmitDefaultValue = false, Name = "type")]
		public IsolationType Type;
	}
}
