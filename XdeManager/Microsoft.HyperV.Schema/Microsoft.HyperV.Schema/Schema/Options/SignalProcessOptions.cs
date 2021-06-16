using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Options
{
	// Token: 0x0200008E RID: 142
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class SignalProcessOptions
	{
		// Token: 0x06000229 RID: 553 RVA: 0x00008497 File Offset: 0x00006697
		public static bool IsJsonDefault(SignalProcessOptions val)
		{
			return SignalProcessOptions._default.JsonEquals(val);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000084A4 File Offset: 0x000066A4
		public bool JsonEquals(object obj)
		{
			SignalProcessOptions graph = obj as SignalProcessOptions;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(SignalProcessOptions), new DataContractJsonSerializerSettings
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000854C File Offset: 0x0000674C
		// (set) Token: 0x0600022C RID: 556 RVA: 0x00008576 File Offset: 0x00006776
		[DataMember(EmitDefaultValue = false, Name = "Signal")]
		private string _Signal
		{
			get
			{
				if (this.Signal == ProcessSignal.CtrlC)
				{
					return null;
				}
				return this.Signal.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Signal = ProcessSignal.CtrlC;
				}
				this.Signal = (ProcessSignal)Enum.Parse(typeof(ProcessSignal), value, true);
			}
		}

		// Token: 0x040002FE RID: 766
		private static readonly SignalProcessOptions _default = new SignalProcessOptions();

		// Token: 0x040002FF RID: 767
		public ProcessSignal Signal;
	}
}
