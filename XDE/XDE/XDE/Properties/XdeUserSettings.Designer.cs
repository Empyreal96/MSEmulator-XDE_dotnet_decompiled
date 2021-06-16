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
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00010391 File Offset: 0x0000E591
		public static XdeUserSettings Default
		{
			get
			{
				return XdeUserSettings.defaultInstance;
			}
		}

		// Token: 0x04000173 RID: 371
		private static XdeUserSettings defaultInstance = (XdeUserSettings)SettingsBase.Synchronized(new XdeUserSettings());
	}
}
