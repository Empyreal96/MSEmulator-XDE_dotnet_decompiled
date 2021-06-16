using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000027 RID: 39
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SystemTime
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x00003CD0 File Offset: 0x00001ED0
		public static bool IsJsonDefault(SystemTime val)
		{
			return SystemTime._default.JsonEquals(val);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003CE0 File Offset: 0x00001EE0
		public bool JsonEquals(object obj)
		{
			SystemTime graph = obj as SystemTime;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SystemTime), new DataContractJsonSerializerSettings
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

		// Token: 0x040000AE RID: 174
		private static readonly SystemTime _default = new SystemTime();

		// Token: 0x040000AF RID: 175
		[DataMember]
		public ushort Year;

		// Token: 0x040000B0 RID: 176
		[DataMember]
		public ushort Month;

		// Token: 0x040000B1 RID: 177
		[DataMember]
		public ushort DayOfWeek;

		// Token: 0x040000B2 RID: 178
		[DataMember]
		public ushort Day;

		// Token: 0x040000B3 RID: 179
		[DataMember]
		public ushort Hour;

		// Token: 0x040000B4 RID: 180
		[DataMember]
		public ushort Minute;

		// Token: 0x040000B5 RID: 181
		[DataMember]
		public ushort Second;

		// Token: 0x040000B6 RID: 182
		[DataMember]
		public ushort Milliseconds;
	}
}
