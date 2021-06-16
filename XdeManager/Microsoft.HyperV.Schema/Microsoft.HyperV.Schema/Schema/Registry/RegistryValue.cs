using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Registry
{
	// Token: 0x0200007C RID: 124
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class RegistryValue
	{
		// Token: 0x060001EB RID: 491 RVA: 0x00007967 File Offset: 0x00005B67
		public static bool IsJsonDefault(RegistryValue val)
		{
			return RegistryValue._default.JsonEquals(val);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00007974 File Offset: 0x00005B74
		public bool JsonEquals(object obj)
		{
			RegistryValue graph = obj as RegistryValue;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(RegistryValue), new DataContractJsonSerializerSettings
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

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00007A1C File Offset: 0x00005C1C
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00007A24 File Offset: 0x00005C24
		[DataMember(Name = "Key")]
		private RegistryKey _Key
		{
			get
			{
				return this.Key;
			}
			set
			{
				if (value != null)
				{
					this.Key = value;
				}
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00007A30 File Offset: 0x00005C30
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00007A4A File Offset: 0x00005C4A
		[DataMember(Name = "Type")]
		private string _Type
		{
			get
			{
				RegistryValueType type = this.Type;
				return this.Type.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Type = RegistryValueType.None;
				}
				this.Type = (RegistryValueType)Enum.Parse(typeof(RegistryValueType), value, true);
			}
		}

		// Token: 0x040002AC RID: 684
		private static readonly RegistryValue _default = new RegistryValue();

		// Token: 0x040002AD RID: 685
		public RegistryKey Key = new RegistryKey();

		// Token: 0x040002AE RID: 686
		[DataMember]
		public string Name;

		// Token: 0x040002AF RID: 687
		public RegistryValueType Type;

		// Token: 0x040002B0 RID: 688
		[DataMember(EmitDefaultValue = false)]
		public string StringValue;

		// Token: 0x040002B1 RID: 689
		[DataMember(EmitDefaultValue = false)]
		public byte[] BinaryValue;

		// Token: 0x040002B2 RID: 690
		[DataMember(EmitDefaultValue = false)]
		public uint? DWordValue;

		// Token: 0x040002B3 RID: 691
		[DataMember(EmitDefaultValue = false)]
		public ulong? QWordValue;

		// Token: 0x040002B4 RID: 692
		[DataMember(EmitDefaultValue = false)]
		public uint? CustomType;
	}
}
