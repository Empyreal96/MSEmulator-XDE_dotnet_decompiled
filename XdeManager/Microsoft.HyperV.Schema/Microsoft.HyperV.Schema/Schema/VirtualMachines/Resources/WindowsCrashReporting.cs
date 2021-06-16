using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000023 RID: 35
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class WindowsCrashReporting
	{
		// Token: 0x06000091 RID: 145 RVA: 0x0000394E File Offset: 0x00001B4E
		public static bool IsJsonDefault(WindowsCrashReporting val)
		{
			return WindowsCrashReporting._default.JsonEquals(val);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000395C File Offset: 0x00001B5C
		public bool JsonEquals(object obj)
		{
			WindowsCrashReporting graph = obj as WindowsCrashReporting;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(WindowsCrashReporting), new DataContractJsonSerializerSettings
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

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003A04 File Offset: 0x00001C04
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00003A2E File Offset: 0x00001C2E
		[DataMember(EmitDefaultValue = false, Name = "DumpType")]
		private string _DumpType
		{
			get
			{
				if (this.DumpType == WindowsCrashDumpType.Disabled)
				{
					return null;
				}
				return this.DumpType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.DumpType = WindowsCrashDumpType.Disabled;
				}
				this.DumpType = (WindowsCrashDumpType)Enum.Parse(typeof(WindowsCrashDumpType), value, true);
			}
		}

		// Token: 0x040000A4 RID: 164
		private static readonly WindowsCrashReporting _default = new WindowsCrashReporting();

		// Token: 0x040000A5 RID: 165
		[DataMember]
		public string DumpFileName;

		// Token: 0x040000A6 RID: 166
		[DataMember(EmitDefaultValue = false)]
		public long MaxDumpSize;

		// Token: 0x040000A7 RID: 167
		public WindowsCrashDumpType DumpType;
	}
}
