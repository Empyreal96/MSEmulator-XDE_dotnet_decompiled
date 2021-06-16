using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Bios
{
	// Token: 0x02000160 RID: 352
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Boot
	{
		// Token: 0x06000579 RID: 1401 RVA: 0x00011C78 File Offset: 0x0000FE78
		public static bool IsJsonDefault(Boot val)
		{
			return Boot._default.JsonEquals(val);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00011C88 File Offset: 0x0000FE88
		public bool JsonEquals(object obj)
		{
			Boot graph = obj as Boot;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Boot), new DataContractJsonSerializerSettings
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

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00011D30 File Offset: 0x0000FF30
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00011D88 File Offset: 0x0000FF88
		[DataMember(Name = "device")]
		private string[] _Order
		{
			get
			{
				if (this.Order == null)
				{
					return null;
				}
				string[] array = new string[this.Order.Length];
				for (int i = 0; i < array.Length; i++)
				{
					BootDevice bootDevice = this.Order[i];
					array[i] = this.Order[i].ToString();
				}
				return array;
			}
			set
			{
				if (value == null)
				{
					this.Order = null;
					return;
				}
				this.Order = new BootDevice[value.Length];
				for (int i = 0; i < value.Length; i++)
				{
					if (string.IsNullOrEmpty(value[i]))
					{
						this.Order[i] = BootDevice.Floppy;
					}
					else
					{
						this.Order[i] = (BootDevice)Enum.Parse(typeof(BootDevice), value[i], true);
					}
				}
			}
		}

		// Token: 0x0400073F RID: 1855
		private static readonly Boot _default = new Boot();

		// Token: 0x04000740 RID: 1856
		public BootDevice[] Order;
	}
}
