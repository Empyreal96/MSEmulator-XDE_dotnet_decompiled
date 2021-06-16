using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Memory
{
	// Token: 0x020000CE RID: 206
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MemoryReserveResizeOperation
	{
		// Token: 0x06000321 RID: 801 RVA: 0x0000B34F File Offset: 0x0000954F
		public static bool IsJsonDefault(MemoryReserveResizeOperation val)
		{
			return MemoryReserveResizeOperation._default.JsonEquals(val);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000B35C File Offset: 0x0000955C
		public bool JsonEquals(object obj)
		{
			MemoryReserveResizeOperation graph = obj as MemoryReserveResizeOperation;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MemoryReserveResizeOperation), new DataContractJsonSerializerSettings
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

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000B404 File Offset: 0x00009604
		// (set) Token: 0x06000324 RID: 804 RVA: 0x0000B41E File Offset: 0x0000961E
		[DataMember(Name = "PageSize")]
		private string _PageSize
		{
			get
			{
				MemoryReservePageSize pageSize = this.PageSize;
				return this.PageSize.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.PageSize = MemoryReservePageSize.NoPreference;
				}
				this.PageSize = (MemoryReservePageSize)Enum.Parse(typeof(MemoryReservePageSize), value, true);
			}
		}

		// Token: 0x040003FA RID: 1018
		private static readonly MemoryReserveResizeOperation _default = new MemoryReserveResizeOperation();

		// Token: 0x040003FB RID: 1019
		[DataMember]
		public byte NumaNodeIndex;

		// Token: 0x040003FC RID: 1020
		[DataMember]
		public ulong PageCount;

		// Token: 0x040003FD RID: 1021
		public MemoryReservePageSize PageSize;

		// Token: 0x040003FE RID: 1022
		[DataMember]
		public bool AnyNodeOkay;

		// Token: 0x040003FF RID: 1023
		[DataMember]
		public bool Above4Gb;
	}
}
