using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000094 RID: 148
	public abstract class NamingStrategy
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x00023D8C File Offset: 0x00021F8C
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x00023D94 File Offset: 0x00021F94
		public bool ProcessDictionaryKeys { get; set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x00023D9D File Offset: 0x00021F9D
		// (set) Token: 0x0600080C RID: 2060 RVA: 0x00023DA5 File Offset: 0x00021FA5
		public bool ProcessExtensionDataNames { get; set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x00023DAE File Offset: 0x00021FAE
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x00023DB6 File Offset: 0x00021FB6
		public bool OverrideSpecifiedNames { get; set; }

		// Token: 0x0600080F RID: 2063 RVA: 0x00023DBF File Offset: 0x00021FBF
		public virtual string GetPropertyName(string name, bool hasSpecifiedName)
		{
			if (hasSpecifiedName && !this.OverrideSpecifiedNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00023DD5 File Offset: 0x00021FD5
		public virtual string GetExtensionDataName(string name)
		{
			if (!this.ProcessExtensionDataNames)
			{
				return name;
			}
			return this.ResolvePropertyName(name);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00023DE8 File Offset: 0x00021FE8
		public virtual string GetDictionaryKey(string key)
		{
			if (!this.ProcessDictionaryKeys)
			{
				return key;
			}
			return this.ResolvePropertyName(key);
		}

		// Token: 0x06000812 RID: 2066
		protected abstract string ResolvePropertyName(string name);

		// Token: 0x06000813 RID: 2067 RVA: 0x00023DFC File Offset: 0x00021FFC
		public override int GetHashCode()
		{
			return ((base.GetType().GetHashCode() * 397 ^ this.ProcessDictionaryKeys.GetHashCode()) * 397 ^ this.ProcessExtensionDataNames.GetHashCode()) * 397 ^ this.OverrideSpecifiedNames.GetHashCode();
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00023E53 File Offset: 0x00022053
		public override bool Equals(object obj)
		{
			return this.Equals(obj as NamingStrategy);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00023E64 File Offset: 0x00022064
		protected bool Equals(NamingStrategy other)
		{
			return other != null && (base.GetType() == other.GetType() && this.ProcessDictionaryKeys == other.ProcessDictionaryKeys && this.ProcessExtensionDataNames == other.ProcessExtensionDataNames) && this.OverrideSpecifiedNames == other.OverrideSpecifiedNames;
		}
	}
}
