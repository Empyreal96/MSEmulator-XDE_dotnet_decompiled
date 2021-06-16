using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000B7 RID: 183
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class LogicalProcessor
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x0000A277 File Offset: 0x00008477
		public static bool IsJsonDefault(LogicalProcessor val)
		{
			return LogicalProcessor._default.JsonEquals(val);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000A284 File Offset: 0x00008484
		public bool JsonEquals(object obj)
		{
			LogicalProcessor graph = obj as LogicalProcessor;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(LogicalProcessor), new DataContractJsonSerializerSettings
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

		// Token: 0x040003A3 RID: 931
		private static readonly LogicalProcessor _default = new LogicalProcessor();

		// Token: 0x040003A4 RID: 932
		[DataMember]
		public uint LpIndex;

		// Token: 0x040003A5 RID: 933
		[DataMember]
		public byte NodeNumber;

		// Token: 0x040003A6 RID: 934
		[DataMember]
		public uint PackageId;

		// Token: 0x040003A7 RID: 935
		[DataMember]
		public uint CoreId;

		// Token: 0x040003A8 RID: 936
		[DataMember]
		public int RootVpIndex;
	}
}
