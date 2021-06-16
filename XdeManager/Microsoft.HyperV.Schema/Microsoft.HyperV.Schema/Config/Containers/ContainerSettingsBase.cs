using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Containers
{
	// Token: 0x0200016F RID: 367
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerSettingsBase
	{
		// Token: 0x060005B9 RID: 1465 RVA: 0x0001271B File Offset: 0x0001091B
		public static bool IsJsonDefault(ContainerSettingsBase val)
		{
			return ContainerSettingsBase._default.JsonEquals(val);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00012728 File Offset: 0x00010928
		public bool JsonEquals(object obj)
		{
			ContainerSettingsBase graph = obj as ContainerSettingsBase;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerSettingsBase), new DataContractJsonSerializerSettings
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

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x000127D0 File Offset: 0x000109D0
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x000127EA File Offset: 0x000109EA
		[DataMember(IsRequired = true, Name = "SystemType")]
		private string _SystemType
		{
			get
			{
				SystemType systemType = this.SystemType;
				return this.SystemType.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.SystemType = SystemType.Container;
				}
				this.SystemType = (SystemType)Enum.Parse(typeof(SystemType), value, true);
			}
		}

		// Token: 0x040007B7 RID: 1975
		private static readonly ContainerSettingsBase _default = new ContainerSettingsBase();

		// Token: 0x040007B8 RID: 1976
		public SystemType SystemType;

		// Token: 0x040007B9 RID: 1977
		[DataMember]
		public string Name;

		// Token: 0x040007BA RID: 1978
		[DataMember(EmitDefaultValue = false)]
		public bool HvPartition;
	}
}
