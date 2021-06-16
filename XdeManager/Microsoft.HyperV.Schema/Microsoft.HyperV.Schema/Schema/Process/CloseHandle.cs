using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Process
{
	// Token: 0x02000082 RID: 130
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CloseHandle
	{
		// Token: 0x060001FF RID: 511 RVA: 0x00007CF8 File Offset: 0x00005EF8
		public static bool IsJsonDefault(CloseHandle val)
		{
			return CloseHandle._default.JsonEquals(val);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00007D08 File Offset: 0x00005F08
		public bool JsonEquals(object obj)
		{
			CloseHandle graph = obj as CloseHandle;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CloseHandle), new DataContractJsonSerializerSettings
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00007DB0 File Offset: 0x00005FB0
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00007DCA File Offset: 0x00005FCA
		[DataMember(Name = "Handle")]
		private string _Handle
		{
			get
			{
				StdHandle handle = this.Handle;
				return this.Handle.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Handle = StdHandle.StdIn;
				}
				this.Handle = (StdHandle)Enum.Parse(typeof(StdHandle), value, true);
			}
		}

		// Token: 0x040002D3 RID: 723
		private static readonly CloseHandle _default = new CloseHandle();

		// Token: 0x040002D4 RID: 724
		public StdHandle Handle;
	}
}
