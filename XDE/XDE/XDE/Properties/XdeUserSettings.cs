using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Microsoft.Xde.Client.Properties
{
	// Token: 0x0200002F RID: 47
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.1.0.0")]
	internal sealed partial class XdeUserSettings : ApplicationSettingsBase
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x000102A5 File Offset: 0x0000E4A5
		private XdeUserSettings()
		{
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x000102B0 File Offset: 0x0000E4B0
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x000102D9 File Offset: 0x0000E4D9
		[UserScopedSetting]
		public List<VmUserSettings> VmSettings
		{
			get
			{
				List<VmUserSettings> list = (List<VmUserSettings>)this["VmSettings"];
				if (list == null)
				{
					this.VmSettings = list;
				}
				return list;
			}
			set
			{
				this["VmSettings"] = value;
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000102E8 File Offset: 0x0000E4E8
		public void AddVmSetting(VmUserSettings settings)
		{
			int num = this.VmSettings.FindIndex((VmUserSettings o) => o.Name == settings.Name);
			if (num != -1)
			{
				this.VmSettings.RemoveAt(num);
			}
			this.VmSettings.Add(settings);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001033C File Offset: 0x0000E53C
		public bool TryGetValue(string name, out VmUserSettings settings)
		{
			settings = this.VmSettings.FirstOrDefault((VmUserSettings o) => o.Name == name);
			return settings != null;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00010374 File Offset: 0x0000E574
		protected override void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e)
		{
			base.OnSettingsLoaded(sender, e);
			if (this.VmSettings == null)
			{
				this.VmSettings = new List<VmUserSettings>();
			}
		}
	}
}
