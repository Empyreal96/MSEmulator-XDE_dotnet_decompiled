using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Error
{
	// Token: 0x020001A6 RID: 422
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ErrorEvent
	{
		// Token: 0x060006CB RID: 1739 RVA: 0x00015893 File Offset: 0x00013A93
		public static bool IsJsonDefault(ErrorEvent val)
		{
			return ErrorEvent._default.JsonEquals(val);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x000158A0 File Offset: 0x00013AA0
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

		// Token: 0x0400095C RID: 2396
		private static readonly ErrorEvent _default = new ErrorEvent();

		// Token: 0x0400095D RID: 2397
		[DataMember]
		public string Message;

		// Token: 0x0400095E RID: 2398
		[DataMember(EmitDefaultValue = false)]
		public string StackTrace;

		// Token: 0x0400095F RID: 2399
		[DataMember(IsRequired = true)]
		public Guid Provider;

		// Token: 0x04000960 RID: 2400
		[DataMember(IsRequired = true)]
		public ushort EventId;

		// Token: 0x04000961 RID: 2401
		[DataMember(EmitDefaultValue = false)]
		public uint Flags;

		// Token: 0x04000962 RID: 2402
		[DataMember(EmitDefaultValue = false)]
		public string Source;

		// Token: 0x04000963 RID: 2403
		[DataMember(EmitDefaultValue = false)]
		public EventData[] Data;
	}
}
