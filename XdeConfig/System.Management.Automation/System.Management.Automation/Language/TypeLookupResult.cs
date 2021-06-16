using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x020005C6 RID: 1478
	internal class TypeLookupResult
	{
		// Token: 0x06003F96 RID: 16278 RVA: 0x0014FF56 File Offset: 0x0014E156
		public TypeLookupResult(TypeDefinitionAst type = null)
		{
			this.Type = type;
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06003F97 RID: 16279 RVA: 0x0014FF65 File Offset: 0x0014E165
		// (set) Token: 0x06003F98 RID: 16280 RVA: 0x0014FF6D File Offset: 0x0014E16D
		public TypeDefinitionAst Type { get; set; }

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06003F99 RID: 16281 RVA: 0x0014FF76 File Offset: 0x0014E176
		// (set) Token: 0x06003F9A RID: 16282 RVA: 0x0014FF7E File Offset: 0x0014E17E
		public List<string> ExternalNamespaces { get; set; }

		// Token: 0x06003F9B RID: 16283 RVA: 0x0014FF87 File Offset: 0x0014E187
		public bool IsAmbiguous()
		{
			return this.ExternalNamespaces != null && this.ExternalNamespaces.Count > 1;
		}
	}
}
