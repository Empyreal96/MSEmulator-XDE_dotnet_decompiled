using System;
using System.Collections.Generic;
using System.IO;

namespace System.Management.Automation
{
	// Token: 0x0200085B RID: 2139
	public abstract class PSSnapInInstaller : PSInstaller
	{
		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06005251 RID: 21073
		public abstract string Name { get; }

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x06005252 RID: 21074
		public abstract string Vendor { get; }

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x06005253 RID: 21075 RVA: 0x001B79D7 File Offset: 0x001B5BD7
		public virtual string VendorResource
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x06005254 RID: 21076
		public abstract string Description { get; }

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x06005255 RID: 21077 RVA: 0x001B79DA File Offset: 0x001B5BDA
		public virtual string DescriptionResource
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x06005256 RID: 21078 RVA: 0x001B79DD File Offset: 0x001B5BDD
		private string MshSnapinVersion
		{
			get
			{
				return base.GetType().Assembly.GetName().Version.ToString();
			}
		}

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06005257 RID: 21079 RVA: 0x001B79F9 File Offset: 0x001B5BF9
		private string PSVersion
		{
			get
			{
				if (this._psVersion == null)
				{
					this._psVersion = PSVersionInfo.FeatureVersionString;
				}
				return this._psVersion;
			}
		}

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06005258 RID: 21080 RVA: 0x001B7A14 File Offset: 0x001B5C14
		internal sealed override string RegKey
		{
			get
			{
				PSSnapInInfo.VerifyPSSnapInFormatThrowIfError(this.Name);
				return "PowerShellSnapIns\\" + this.Name;
			}
		}

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x06005259 RID: 21081 RVA: 0x001B7A34 File Offset: 0x001B5C34
		internal override Dictionary<string, object> RegValues
		{
			get
			{
				if (this._regValues == null)
				{
					this._regValues = new Dictionary<string, object>();
					this._regValues["PowerShellVersion"] = this.PSVersion;
					if (!string.IsNullOrEmpty(this.Vendor))
					{
						this._regValues["Vendor"] = this.Vendor;
					}
					if (!string.IsNullOrEmpty(this.Description))
					{
						this._regValues["Description"] = this.Description;
					}
					if (!string.IsNullOrEmpty(this.VendorResource))
					{
						this._regValues["VendorIndirect"] = this.VendorResource;
					}
					if (!string.IsNullOrEmpty(this.DescriptionResource))
					{
						this._regValues["DescriptionIndirect"] = this.DescriptionResource;
					}
					this._regValues["Version"] = this.MshSnapinVersion;
					this._regValues["ApplicationBase"] = Path.GetDirectoryName(base.GetType().Assembly.Location);
					this._regValues["AssemblyName"] = base.GetType().Assembly.FullName;
					this._regValues["ModuleName"] = base.GetType().Assembly.Location;
				}
				return this._regValues;
			}
		}

		// Token: 0x04002A3B RID: 10811
		private string _psVersion;

		// Token: 0x04002A3C RID: 10812
		private Dictionary<string, object> _regValues;
	}
}
