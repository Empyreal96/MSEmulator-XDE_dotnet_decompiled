using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Config.Devices.Rdp;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000ED RID: 237
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class GlobalSettings
	{
		// Token: 0x06000381 RID: 897 RVA: 0x0000C574 File Offset: 0x0000A774
		public static bool IsJsonDefault(GlobalSettings val)
		{
			return GlobalSettings._default.JsonEquals(val);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000C584 File Offset: 0x0000A784
		public bool JsonEquals(object obj)
		{
			GlobalSettings graph = obj as GlobalSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(GlobalSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0000C62C File Offset: 0x0000A82C
		// (set) Token: 0x06000384 RID: 900 RVA: 0x0000C634 File Offset: 0x0000A834
		[DataMember(Name = "power")]
		private PowerPolicies _PowerPolicies
		{
			get
			{
				return this.PowerPolicies;
			}
			set
			{
				if (value != null)
				{
					this.PowerPolicies = value;
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000385 RID: 901 RVA: 0x0000C640 File Offset: 0x0000A840
		// (set) Token: 0x06000386 RID: 902 RVA: 0x0000C648 File Offset: 0x0000A848
		[DataMember(Name = "critical_error")]
		private CriticalErrorPolicy _CriticalErrorPolicy
		{
			get
			{
				return this.CriticalErrorPolicy;
			}
			set
			{
				if (value != null)
				{
					this.CriticalErrorPolicy = value;
				}
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000387 RID: 903 RVA: 0x0000C654 File Offset: 0x0000A854
		// (set) Token: 0x06000388 RID: 904 RVA: 0x0000C65C File Offset: 0x0000A85C
		[DataMember(Name = "devices")]
		private GlobalDeviceSettings _Devices
		{
			get
			{
				return this.Devices;
			}
			set
			{
				if (value != null)
				{
					this.Devices = value;
				}
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0000C668 File Offset: 0x0000A868
		// (set) Token: 0x0600038A RID: 906 RVA: 0x0000C67F File Offset: 0x0000A87F
		[DataMember(EmitDefaultValue = false, Name = "security")]
		private GlobalSecurity _Security
		{
			get
			{
				if (!GlobalSecurity.IsJsonDefault(this.Security))
				{
					return this.Security;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Security = value;
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000C68B File Offset: 0x0000A88B
		// (set) Token: 0x0600038C RID: 908 RVA: 0x0000C6A2 File Offset: 0x0000A8A2
		[DataMember(EmitDefaultValue = false, Name = "debug")]
		private DebugSettings _DebugSettings
		{
			get
			{
				if (!DebugSettings.IsJsonDefault(this.DebugSettings))
				{
					return this.DebugSettings;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.DebugSettings = value;
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000C6AE File Offset: 0x0000A8AE
		// (set) Token: 0x0600038E RID: 910 RVA: 0x0000C6B6 File Offset: 0x0000A8B6
		[DataMember(Name = "storage_settings")]
		private StorageSettings _StorageSettings
		{
			get
			{
				return this.StorageSettings;
			}
			set
			{
				if (value != null)
				{
					this.StorageSettings = value;
				}
			}
		}

		// Token: 0x04000481 RID: 1153
		private static readonly GlobalSettings _default = new GlobalSettings();

		// Token: 0x04000482 RID: 1154
		public PowerPolicies PowerPolicies = new PowerPolicies();

		// Token: 0x04000483 RID: 1155
		[DataMember(EmitDefaultValue = false, Name = "unexpected_termination")]
		public CrashPolicy CrashPolicy;

		// Token: 0x04000484 RID: 1156
		public CriticalErrorPolicy CriticalErrorPolicy = new CriticalErrorPolicy();

		// Token: 0x04000485 RID: 1157
		public GlobalDeviceSettings Devices = new GlobalDeviceSettings();

		// Token: 0x04000486 RID: 1158
		public GlobalSecurity Security = new GlobalSecurity();

		// Token: 0x04000487 RID: 1159
		public DebugSettings DebugSettings = new DebugSettings();

		// Token: 0x04000488 RID: 1160
		public StorageSettings StorageSettings = new StorageSettings();

		// Token: 0x04000489 RID: 1161
		[DataMember(Name = "slp_data_root")]
		public string SlpDataRoot;

		// Token: 0x0400048A RID: 1162
		[DataMember(EmitDefaultValue = false, Name = "enhanced_session_transport_type")]
		public TransportType EnhancedSessionTransportType;
	}
}
