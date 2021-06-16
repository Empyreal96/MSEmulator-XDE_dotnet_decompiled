using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.Service
{
	// Token: 0x0200006A RID: 106
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ErrorEvent
	{
		// Token: 0x060001AB RID: 427 RVA: 0x00006C83 File Offset: 0x00004E83
		public static bool IsJsonDefault(ErrorEvent val)
		{
			return ErrorEvent._default.JsonEquals(val);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00006C90 File Offset: 0x00004E90
		public bool JsonEquals(object obj)
		{
			ErrorEvent graph = obj as ErrorEvent;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ErrorEvent), new DataContractJsonSerializerSettings
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

		// Token: 0x0400023D RID: 573
		private static readonly ErrorEvent _default = new ErrorEvent();

		// Token: 0x0400023E RID: 574
		[DataMember]
		public string Message;

		// Token: 0x0400023F RID: 575
		[DataMember(EmitDefaultValue = false)]
		public string StackTrace;

		// Token: 0x04000240 RID: 576
		[DataMember(IsRequired = true)]
		public Guid Provider;

		// Token: 0x04000241 RID: 577
		[DataMember(IsRequired = true)]
		public ushort EventId;

		// Token: 0x04000242 RID: 578
		[DataMember(EmitDefaultValue = false)]
		public uint Flags;

		// Token: 0x04000243 RID: 579
		[DataMember(EmitDefaultValue = false)]
		public string Source;

		// Token: 0x04000244 RID: 580
		[DataMember(EmitDefaultValue = false)]
		public EventData[] Data;
	}
}
