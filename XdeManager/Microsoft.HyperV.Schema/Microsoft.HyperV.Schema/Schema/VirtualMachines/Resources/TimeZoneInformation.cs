using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000028 RID: 40
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class TimeZoneInformation
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00003D9C File Offset: 0x00001F9C
		public static bool IsJsonDefault(TimeZoneInformation val)
		{
			return TimeZoneInformation._default.JsonEquals(val);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003DAC File Offset: 0x00001FAC
		public bool JsonEquals(object obj)
		{
			TimeZoneInformation graph = obj as TimeZoneInformation;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(TimeZoneInformation), new DataContractJsonSerializerSettings
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

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003E54 File Offset: 0x00002054
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00003E5C File Offset: 0x0000205C
		[DataMember(Name = "StandardDate")]
		private SystemTime _StandardDate
		{
			get
			{
				return this.StandardDate;
			}
			set
			{
				if (value != null)
				{
					this.StandardDate = value;
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003E68 File Offset: 0x00002068
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003E70 File Offset: 0x00002070
		[DataMember(Name = "DaylightDate")]
		private SystemTime _DaylightDate
		{
			get
			{
				return this.DaylightDate;
			}
			set
			{
				if (value != null)
				{
					this.DaylightDate = value;
				}
			}
		}

		// Token: 0x040000B7 RID: 183
		private static readonly TimeZoneInformation _default = new TimeZoneInformation();

		// Token: 0x040000B8 RID: 184
		[DataMember]
		public int Bias;

		// Token: 0x040000B9 RID: 185
		[DataMember]
		public string StandardName;

		// Token: 0x040000BA RID: 186
		public SystemTime StandardDate = new SystemTime();

		// Token: 0x040000BB RID: 187
		[DataMember]
		public int StandardBias;

		// Token: 0x040000BC RID: 188
		[DataMember]
		public string DaylightName;

		// Token: 0x040000BD RID: 189
		public SystemTime DaylightDate = new SystemTime();

		// Token: 0x040000BE RID: 190
		[DataMember]
		public int DaylightBias;
	}
}
