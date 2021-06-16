using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Responses.Service
{
	// Token: 0x02000069 RID: 105
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class EventData
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x00006B70 File Offset: 0x00004D70
		public static bool IsJsonDefault(EventData val)
		{
			return EventData._default.JsonEquals(val);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00006B80 File Offset: 0x00004D80
		public bool JsonEquals(object obj)
		{
			EventData graph = obj as EventData;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(EventData), new DataContractJsonSerializerSettings
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

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00006C28 File Offset: 0x00004E28
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00006C42 File Offset: 0x00004E42
		[DataMember(IsRequired = true, Name = "Type")]
		private string _Type
		{
			get
			{
				EventDataType type = this.Type;
				return this.Type.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Type = EventDataType.Empty;
				}
				this.Type = (EventDataType)Enum.Parse(typeof(EventDataType), value, true);
			}
		}

		// Token: 0x0400023A RID: 570
		private static readonly EventData _default = new EventData();

		// Token: 0x0400023B RID: 571
		public EventDataType Type;

		// Token: 0x0400023C RID: 572
		[DataMember(IsRequired = true)]
		public string Value;
	}
}
