using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Responses.System;

namespace HCS.Schema.Requests.System
{
	// Token: 0x02000070 RID: 112
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SystemQuery
	{
		// Token: 0x060001BF RID: 447 RVA: 0x000070B5 File Offset: 0x000052B5
		public static bool IsJsonDefault(SystemQuery val)
		{
			return SystemQuery._default.JsonEquals(val);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000070C4 File Offset: 0x000052C4
		public bool JsonEquals(object obj)
		{
			SystemQuery graph = obj as SystemQuery;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SystemQuery), new DataContractJsonSerializerSettings
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

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000716C File Offset: 0x0000536C
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x000071C4 File Offset: 0x000053C4
		[DataMember(EmitDefaultValue = false, Name = "Types")]
		private string[] _Types
		{
			get
			{
				if (this.Types == null)
				{
					return null;
				}
				string[] array = new string[this.Types.Length];
				for (int i = 0; i < array.Length; i++)
				{
					SystemType systemType = this.Types[i];
					array[i] = this.Types[i].ToString();
				}
				return array;
			}
			set
			{
				if (value == null)
				{
					this.Types = null;
					return;
				}
				this.Types = new SystemType[value.Length];
				for (int i = 0; i < value.Length; i++)
				{
					if (string.IsNullOrEmpty(value[i]))
					{
						this.Types[i] = SystemType.Container;
					}
					else
					{
						this.Types[i] = (SystemType)Enum.Parse(typeof(SystemType), value[i], true);
					}
				}
			}
		}

		// Token: 0x04000263 RID: 611
		private static readonly SystemQuery _default = new SystemQuery();

		// Token: 0x04000264 RID: 612
		[DataMember(EmitDefaultValue = false)]
		public string[] Ids;

		// Token: 0x04000265 RID: 613
		[DataMember(EmitDefaultValue = false)]
		public string[] Names;

		// Token: 0x04000266 RID: 614
		public SystemType[] Types;

		// Token: 0x04000267 RID: 615
		[DataMember(EmitDefaultValue = false)]
		public string[] Owners;
	}
}
