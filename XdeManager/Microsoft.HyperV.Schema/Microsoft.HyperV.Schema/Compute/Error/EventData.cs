using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Error
{
	// Token: 0x020001A5 RID: 421
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class EventData
	{
		// Token: 0x060006C5 RID: 1733 RVA: 0x00015780 File Offset: 0x00013980
		public static bool IsJsonDefault(EventData val)
		{
			return EventData._default.JsonEquals(val);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00015790 File Offset: 0x00013990
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

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00015838 File Offset: 0x00013A38
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x00015852 File Offset: 0x00013A52
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

		// Token: 0x04000959 RID: 2393
		private static readonly EventData _default = new EventData();

		// Token: 0x0400095A RID: 2394
		public EventDataType Type;

		// Token: 0x0400095B RID: 2395
		[DataMember(IsRequired = true)]
		public string Value;
	}
}
