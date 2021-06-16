using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources
{
	// Token: 0x02000018 RID: 24
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Uefi
	{
		// Token: 0x06000061 RID: 97 RVA: 0x0000300B File Offset: 0x0000120B
		public static bool IsJsonDefault(Uefi val)
		{
			return Uefi._default.JsonEquals(val);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003018 File Offset: 0x00001218
		public bool JsonEquals(object obj)
		{
			Uefi graph = obj as Uefi;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Uefi), new DataContractJsonSerializerSettings
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000030C0 File Offset: 0x000012C0
		// (set) Token: 0x06000064 RID: 100 RVA: 0x000030EA File Offset: 0x000012EA
		[DataMember(EmitDefaultValue = false, Name = "Console")]
		private string _Console
		{
			get
			{
				if (this.Console == SerialConsole.Default)
				{
					return null;
				}
				return this.Console.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Console = SerialConsole.Default;
				}
				this.Console = (SerialConsole)Enum.Parse(typeof(SerialConsole), value, true);
			}
		}

		// Token: 0x0400007F RID: 127
		private static readonly Uefi _default = new Uefi();

		// Token: 0x04000080 RID: 128
		[DataMember(EmitDefaultValue = false)]
		public bool EnableDebugger;

		// Token: 0x04000081 RID: 129
		[DataMember(EmitDefaultValue = false)]
		public Guid? SecureBootTemplateId;

		// Token: 0x04000082 RID: 130
		[DataMember(EmitDefaultValue = false)]
		public UefiBootEntry BootThis;

		// Token: 0x04000083 RID: 131
		public SerialConsole Console;

		// Token: 0x04000084 RID: 132
		[DataMember(EmitDefaultValue = false)]
		public bool StopOnBootFailure;
	}
}
