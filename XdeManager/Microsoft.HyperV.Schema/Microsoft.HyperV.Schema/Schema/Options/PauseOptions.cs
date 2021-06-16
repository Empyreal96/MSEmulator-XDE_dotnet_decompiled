using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Options
{
	// Token: 0x0200008B RID: 139
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class PauseOptions
	{
		// Token: 0x06000217 RID: 535 RVA: 0x0000815B File Offset: 0x0000635B
		public static bool IsJsonDefault(PauseOptions val)
		{
			return PauseOptions._default.JsonEquals(val);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00008168 File Offset: 0x00006368
		public bool JsonEquals(object obj)
		{
			PauseOptions graph = obj as PauseOptions;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(PauseOptions), new DataContractJsonSerializerSettings
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00008210 File Offset: 0x00006410
		// (set) Token: 0x0600021A RID: 538 RVA: 0x0000823A File Offset: 0x0000643A
		[DataMember(EmitDefaultValue = false, Name = "SuspensionLevel")]
		private string _SuspensionLevel
		{
			get
			{
				if (this.SuspensionLevel == PauseSuspensionLevel.Suspend)
				{
					return null;
				}
				return this.SuspensionLevel.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.SuspensionLevel = PauseSuspensionLevel.Suspend;
				}
				this.SuspensionLevel = (PauseSuspensionLevel)Enum.Parse(typeof(PauseSuspensionLevel), value, true);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00008267 File Offset: 0x00006467
		// (set) Token: 0x0600021C RID: 540 RVA: 0x0000827E File Offset: 0x0000647E
		[DataMember(EmitDefaultValue = false, Name = "HostedNotification")]
		private PauseNotification _HostedNotification
		{
			get
			{
				if (!PauseNotification.IsJsonDefault(this.HostedNotification))
				{
					return this.HostedNotification;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.HostedNotification = value;
				}
			}
		}

		// Token: 0x040002F4 RID: 756
		private static readonly PauseOptions _default = new PauseOptions();

		// Token: 0x040002F5 RID: 757
		public PauseSuspensionLevel SuspensionLevel;

		// Token: 0x040002F6 RID: 758
		public PauseNotification HostedNotification = new PauseNotification();
	}
}
