using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Config.Devices;
using HCS.Config.Devices.Manifest;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x02000103 RID: 259
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Configuration
	{
		// Token: 0x06000405 RID: 1029 RVA: 0x0000DA2B File Offset: 0x0000BC2B
		public static bool IsJsonDefault(Configuration val)
		{
			return Configuration._default.JsonEquals(val);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000DA38 File Offset: 0x0000BC38
		public bool JsonEquals(object obj)
		{
			Configuration graph = obj as Configuration;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Configuration), new DataContractJsonSerializerSettings
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

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0000DAE0 File Offset: 0x0000BCE0
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0000DAE8 File Offset: 0x0000BCE8
		[DataMember(Name = "properties")]
		private Properties _Properties
		{
			get
			{
				return this.Properties;
			}
			set
			{
				if (value != null)
				{
					this.Properties = value;
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0000DAFC File Offset: 0x0000BCFC
		[DataMember(Name = "global_settings")]
		private GlobalSettings _GlobalSettings
		{
			get
			{
				return this.GlobalSettings;
			}
			set
			{
				if (value != null)
				{
					this.GlobalSettings = value;
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000DB08 File Offset: 0x0000BD08
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x0000DB1F File Offset: 0x0000BD1F
		[DataMember(EmitDefaultValue = false, Name = "security")]
		private Security _Security
		{
			get
			{
				if (!Security.IsJsonDefault(this.Security))
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

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0000DB2B File Offset: 0x0000BD2B
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x0000DB33 File Offset: 0x0000BD33
		[DataMember(Name = "settings")]
		private Settings _Settings
		{
			get
			{
				return this.Settings;
			}
			set
			{
				if (value != null)
				{
					this.Settings = value;
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0000DB3F File Offset: 0x0000BD3F
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x0000DB47 File Offset: 0x0000BD47
		[DataMember(Name = "allocation_results")]
		private ResourceAllocations _Resources
		{
			get
			{
				return this.Resources;
			}
			set
			{
				if (value != null)
				{
					this.Resources = value;
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x0000DB53 File Offset: 0x0000BD53
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x0000DB5B File Offset: 0x0000BD5B
		[DataMember(Name = "manifest")]
		private Properties _VDevManifest
		{
			get
			{
				return this.VDevManifest;
			}
			set
			{
				if (value != null)
				{
					this.VDevManifest = value;
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x0000DB67 File Offset: 0x0000BD67
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x0000DB7E File Offset: 0x0000BD7E
		[DataMember(EmitDefaultValue = false, Name = "savedstate")]
		private SavedStateInfo _SavedStateInfo
		{
			get
			{
				if (!SavedStateInfo.IsJsonDefault(this.SavedStateInfo))
				{
					return this.SavedStateInfo;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.SavedStateInfo = value;
				}
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000DB8A File Offset: 0x0000BD8A
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x0000DBA1 File Offset: 0x0000BDA1
		[DataMember(EmitDefaultValue = false, Name = "WorkerProcessSettings")]
		private WorkerProcessSettings _WorkerProcessSettings
		{
			get
			{
				if (!WorkerProcessSettings.IsJsonDefault(this.WorkerProcessSettings))
				{
					return this.WorkerProcessSettings;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.WorkerProcessSettings = value;
				}
			}
		}

		// Token: 0x04000512 RID: 1298
		private static readonly Configuration _default = new Configuration();

		// Token: 0x04000513 RID: 1299
		public Properties Properties = new Properties();

		// Token: 0x04000514 RID: 1300
		public GlobalSettings GlobalSettings = new GlobalSettings();

		// Token: 0x04000515 RID: 1301
		public Security Security = new Security();

		// Token: 0x04000516 RID: 1302
		public Settings Settings = new Settings();

		// Token: 0x04000517 RID: 1303
		public ResourceAllocations Resources = new ResourceAllocations();

		// Token: 0x04000518 RID: 1304
		public Properties VDevManifest = new Properties();

		// Token: 0x04000519 RID: 1305
		public SavedStateInfo SavedStateInfo = new SavedStateInfo();

		// Token: 0x0400051A RID: 1306
		public WorkerProcessSettings WorkerProcessSettings = new WorkerProcessSettings();

		// Token: 0x0400051B RID: 1307
		[DataMember(Name = "_")]
		public Dictionary<Guid, Device> Devices;
	}
}
