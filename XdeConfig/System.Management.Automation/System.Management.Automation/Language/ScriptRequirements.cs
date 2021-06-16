using System;
using System.Collections.ObjectModel;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Language
{
	// Token: 0x0200053F RID: 1343
	public class ScriptRequirements
	{
		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x060037BA RID: 14266 RVA: 0x0012B037 File Offset: 0x00129237
		// (set) Token: 0x060037BB RID: 14267 RVA: 0x0012B03F File Offset: 0x0012923F
		public string RequiredApplicationId { get; internal set; }

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x060037BC RID: 14268 RVA: 0x0012B048 File Offset: 0x00129248
		// (set) Token: 0x060037BD RID: 14269 RVA: 0x0012B050 File Offset: 0x00129250
		public Version RequiredPSVersion { get; internal set; }

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x0012B059 File Offset: 0x00129259
		// (set) Token: 0x060037BF RID: 14271 RVA: 0x0012B061 File Offset: 0x00129261
		public ReadOnlyCollection<ModuleSpecification> RequiredModules { get; internal set; }

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x060037C0 RID: 14272 RVA: 0x0012B06A File Offset: 0x0012926A
		// (set) Token: 0x060037C1 RID: 14273 RVA: 0x0012B072 File Offset: 0x00129272
		public ReadOnlyCollection<PSSnapInSpecification> RequiresPSSnapIns { get; internal set; }

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x060037C2 RID: 14274 RVA: 0x0012B07B File Offset: 0x0012927B
		// (set) Token: 0x060037C3 RID: 14275 RVA: 0x0012B083 File Offset: 0x00129283
		public ReadOnlyCollection<string> RequiredAssemblies { get; internal set; }

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x060037C4 RID: 14276 RVA: 0x0012B08C File Offset: 0x0012928C
		// (set) Token: 0x060037C5 RID: 14277 RVA: 0x0012B094 File Offset: 0x00129294
		public bool IsElevationRequired { get; internal set; }

		// Token: 0x04001C71 RID: 7281
		internal static readonly ReadOnlyCollection<PSSnapInSpecification> EmptySnapinCollection = new ReadOnlyCollection<PSSnapInSpecification>(new PSSnapInSpecification[0]);

		// Token: 0x04001C72 RID: 7282
		internal static readonly ReadOnlyCollection<string> EmptyAssemblyCollection = new ReadOnlyCollection<string>(new string[0]);

		// Token: 0x04001C73 RID: 7283
		internal static readonly ReadOnlyCollection<ModuleSpecification> EmptyModuleCollection = new ReadOnlyCollection<ModuleSpecification>(new ModuleSpecification[0]);
	}
}
