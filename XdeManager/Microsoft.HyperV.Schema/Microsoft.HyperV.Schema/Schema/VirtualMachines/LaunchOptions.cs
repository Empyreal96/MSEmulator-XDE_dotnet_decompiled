using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines
{
	// Token: 0x0200000F RID: 15
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class LaunchOptions
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00002AA8 File Offset: 0x00000CA8
		public static bool IsJsonDefault(LaunchOptions val)
		{
			return LaunchOptions._default.JsonEquals(val);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public bool JsonEquals(object obj)
		{
			LaunchOptions graph = obj as LaunchOptions;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(LaunchOptions), new DataContractJsonSerializerSettings
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

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002B60 File Offset: 0x00000D60
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002B8A File Offset: 0x00000D8A
		[DataMember(EmitDefaultValue = false, Name = "Type")]
		private string _Type
		{
			get
			{
				if (this.Type == AppContainerLaunchType.Default)
				{
					return null;
				}
				return this.Type.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Type = AppContainerLaunchType.Default;
				}
				this.Type = (AppContainerLaunchType)Enum.Parse(typeof(AppContainerLaunchType), value, true);
			}
		}

		// Token: 0x04000057 RID: 87
		private static readonly LaunchOptions _default = new LaunchOptions();

		// Token: 0x04000058 RID: 88
		public AppContainerLaunchType Type;
	}
}
