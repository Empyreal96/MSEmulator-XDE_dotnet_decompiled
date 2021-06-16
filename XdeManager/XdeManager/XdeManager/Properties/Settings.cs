using System;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Xde.Common;

namespace XdeManager.Properties
{
	// Token: 0x0200000B RID: 11
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.2.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002B5E File Offset: 0x00000D5E
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002B70 File Offset: 0x00000D70
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public SerializableStringDictionary PerDeviceDownloadFolder
		{
			get
			{
				return (SerializableStringDictionary)this["PerDeviceDownloadFolder"];
			}
			set
			{
				this["PerDeviceDownloadFolder"] = value;
			}
		}
	}
}
