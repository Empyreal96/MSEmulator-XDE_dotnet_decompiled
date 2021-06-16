using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Options
{
	// Token: 0x0200008A RID: 138
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class PauseNotification
	{
		// Token: 0x06000211 RID: 529 RVA: 0x0000803B File Offset: 0x0000623B
		public static bool IsJsonDefault(PauseNotification val)
		{
			return PauseNotification._default.JsonEquals(val);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00008048 File Offset: 0x00006248
		public bool JsonEquals(object obj)
		{
			PauseNotification graph = obj as PauseNotification;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(PauseNotification), new DataContractJsonSerializerSettings
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000213 RID: 531 RVA: 0x000080F0 File Offset: 0x000062F0
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000811A File Offset: 0x0000631A
		[DataMember(EmitDefaultValue = false, Name = "Reason")]
		private string _Reason
		{
			get
			{
				if (this.Reason == PauseReason.None)
				{
					return null;
				}
				return this.Reason.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Reason = PauseReason.None;
				}
				this.Reason = (PauseReason)Enum.Parse(typeof(PauseReason), value, true);
			}
		}

		// Token: 0x040002F2 RID: 754
		private static readonly PauseNotification _default = new PauseNotification();

		// Token: 0x040002F3 RID: 755
		public PauseReason Reason;
	}
}
