using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Config.Containers;

namespace HCS.Compute.System
{
	// Token: 0x020001A0 RID: 416
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Query
	{
		// Token: 0x060006B1 RID: 1713 RVA: 0x000152D3 File Offset: 0x000134D3
		public static bool IsJsonDefault(Query val)
		{
			return Query._default.JsonEquals(val);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000152E0 File Offset: 0x000134E0
		public bool JsonEquals(object obj)
		{
			Query graph = obj as Query;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Query), new DataContractJsonSerializerSettings
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

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00015388 File Offset: 0x00013588
		// (set) Token: 0x060006B4 RID: 1716 RVA: 0x000153E0 File Offset: 0x000135E0
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

		// Token: 0x0400093B RID: 2363
		private static readonly Query _default = new Query();

		// Token: 0x0400093C RID: 2364
		[DataMember(EmitDefaultValue = false)]
		public string[] Ids;

		// Token: 0x0400093D RID: 2365
		[DataMember(EmitDefaultValue = false)]
		public string[] Names;

		// Token: 0x0400093E RID: 2366
		public SystemType[] Types;

		// Token: 0x0400093F RID: 2367
		[DataMember(EmitDefaultValue = false)]
		public string[] Owners;
	}
}
