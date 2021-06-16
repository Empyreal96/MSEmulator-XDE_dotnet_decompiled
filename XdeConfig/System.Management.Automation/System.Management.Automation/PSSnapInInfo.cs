using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace System.Management.Automation
{
	// Token: 0x0200084A RID: 2122
	public class PSSnapInInfo
	{
		// Token: 0x06005193 RID: 20883 RVA: 0x001B27A8 File Offset: 0x001B09A8
		internal PSSnapInInfo(string name, bool isDefault, string applicationBase, string assemblyName, string moduleName, Version psVersion, Version version, Collection<string> types, Collection<string> formats, string descriptionFallback, string vendorFallback, string customPSSnapInType)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (string.IsNullOrEmpty(applicationBase))
			{
				throw PSTraceSource.NewArgumentNullException("applicationBase");
			}
			if (string.IsNullOrEmpty(assemblyName))
			{
				throw PSTraceSource.NewArgumentNullException("assemblyName");
			}
			if (string.IsNullOrEmpty(moduleName))
			{
				throw PSTraceSource.NewArgumentNullException("moduleName");
			}
			if (psVersion == null)
			{
				throw PSTraceSource.NewArgumentNullException("psVersion");
			}
			if (version == null)
			{
				version = new Version("0.0");
			}
			if (types == null)
			{
				types = new Collection<string>();
			}
			if (formats == null)
			{
				formats = new Collection<string>();
			}
			if (descriptionFallback == null)
			{
				descriptionFallback = string.Empty;
			}
			if (vendorFallback == null)
			{
				vendorFallback = string.Empty;
			}
			this._name = name;
			this._isDefault = isDefault;
			this._applicationBase = applicationBase;
			this._assemblyName = assemblyName;
			this._moduleName = moduleName;
			this._psVersion = psVersion;
			this._version = version;
			this._types = types;
			this._formats = formats;
			this._customPSSnapInType = customPSSnapInType;
			this._descriptionFallback = descriptionFallback;
			this._vendorFallback = vendorFallback;
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x001B28D4 File Offset: 0x001B0AD4
		internal PSSnapInInfo(string name, bool isDefault, string applicationBase, string assemblyName, string moduleName, Version psVersion, Version version, Collection<string> types, Collection<string> formats, string description, string descriptionFallback, string vendor, string vendorFallback, string customPSSnapInType) : this(name, isDefault, applicationBase, assemblyName, moduleName, psVersion, version, types, formats, descriptionFallback, vendorFallback, customPSSnapInType)
		{
			this._description = description;
			this._vendor = vendor;
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x001B290C File Offset: 0x001B0B0C
		internal PSSnapInInfo(string name, bool isDefault, string applicationBase, string assemblyName, string moduleName, Version psVersion, Version version, Collection<string> types, Collection<string> formats, string description, string descriptionFallback, string descriptionIndirect, string vendor, string vendorFallback, string vendorIndirect, string customPSSnapInType) : this(name, isDefault, applicationBase, assemblyName, moduleName, psVersion, version, types, formats, description, descriptionFallback, vendor, vendorFallback, customPSSnapInType)
		{
			if (isDefault)
			{
				this._descriptionIndirect = descriptionIndirect;
				this._vendorIndirect = vendorIndirect;
			}
		}

		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x06005196 RID: 20886 RVA: 0x001B294B File Offset: 0x001B0B4B
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x06005197 RID: 20887 RVA: 0x001B2953 File Offset: 0x001B0B53
		public bool IsDefault
		{
			get
			{
				return this._isDefault;
			}
		}

		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x06005198 RID: 20888 RVA: 0x001B295B File Offset: 0x001B0B5B
		public string ApplicationBase
		{
			get
			{
				return this._applicationBase;
			}
		}

		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x06005199 RID: 20889 RVA: 0x001B2963 File Offset: 0x001B0B63
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x0600519A RID: 20890 RVA: 0x001B296B File Offset: 0x001B0B6B
		public string ModuleName
		{
			get
			{
				return this._moduleName;
			}
		}

		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x0600519B RID: 20891 RVA: 0x001B2974 File Offset: 0x001B0B74
		internal string AbsoluteModulePath
		{
			get
			{
				if (string.IsNullOrEmpty(this._moduleName) || Path.IsPathRooted(this._moduleName))
				{
					return this._moduleName;
				}
				if (!File.Exists(Path.Combine(this._applicationBase, this._moduleName)))
				{
					return Path.GetFileNameWithoutExtension(this._moduleName);
				}
				return Path.Combine(this._applicationBase, this._moduleName);
			}
		}

		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x0600519C RID: 20892 RVA: 0x001B29D7 File Offset: 0x001B0BD7
		internal string CustomPSSnapInType
		{
			get
			{
				return this._customPSSnapInType;
			}
		}

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x0600519D RID: 20893 RVA: 0x001B29DF File Offset: 0x001B0BDF
		public Version PSVersion
		{
			get
			{
				return this._psVersion;
			}
		}

		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x0600519E RID: 20894 RVA: 0x001B29E7 File Offset: 0x001B0BE7
		public Version Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x0600519F RID: 20895 RVA: 0x001B29EF File Offset: 0x001B0BEF
		public Collection<string> Types
		{
			get
			{
				return this._types;
			}
		}

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x060051A0 RID: 20896 RVA: 0x001B29F7 File Offset: 0x001B0BF7
		public Collection<string> Formats
		{
			get
			{
				return this._formats;
			}
		}

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x060051A1 RID: 20897 RVA: 0x001B29FF File Offset: 0x001B0BFF
		public string Description
		{
			get
			{
				if (this._description == null)
				{
					this.LoadIndirectResources();
				}
				return this._description;
			}
		}

		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x060051A2 RID: 20898 RVA: 0x001B2A15 File Offset: 0x001B0C15
		public string Vendor
		{
			get
			{
				if (this._vendor == null)
				{
					this.LoadIndirectResources();
				}
				return this._vendor;
			}
		}

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x060051A3 RID: 20899 RVA: 0x001B2A2B File Offset: 0x001B0C2B
		// (set) Token: 0x060051A4 RID: 20900 RVA: 0x001B2A33 File Offset: 0x001B0C33
		public bool LogPipelineExecutionDetails
		{
			get
			{
				return this._logPipelineExecutionDetails;
			}
			set
			{
				this._logPipelineExecutionDetails = value;
			}
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x001B2A3C File Offset: 0x001B0C3C
		public override string ToString()
		{
			return this._name;
		}

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x060051A6 RID: 20902 RVA: 0x001B2A44 File Offset: 0x001B0C44
		internal RegistryKey MshSnapinKey
		{
			get
			{
				RegistryKey result = null;
				try
				{
					result = PSSnapInReader.GetMshSnapinKey(this._name, this._psVersion.Major.ToString(CultureInfo.InvariantCulture));
				}
				catch (ArgumentException)
				{
				}
				catch (SecurityException)
				{
				}
				catch (IOException)
				{
				}
				return result;
			}
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x001B2AAC File Offset: 0x001B0CAC
		internal void LoadIndirectResources()
		{
			using (RegistryStringResourceIndirect resourceIndirectReader = RegistryStringResourceIndirect.GetResourceIndirectReader())
			{
				this.LoadIndirectResources(resourceIndirectReader);
			}
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x001B2AE4 File Offset: 0x001B0CE4
		internal void LoadIndirectResources(RegistryStringResourceIndirect resourceReader)
		{
			if (this.IsDefault)
			{
				this._description = resourceReader.GetResourceStringIndirect(this._assemblyName, this._moduleName, this._descriptionIndirect);
				this._vendor = resourceReader.GetResourceStringIndirect(this._assemblyName, this._moduleName, this._vendorIndirect);
			}
			else
			{
				RegistryKey mshSnapinKey = this.MshSnapinKey;
				if (mshSnapinKey != null)
				{
					this._description = resourceReader.GetResourceStringIndirect(mshSnapinKey, "DescriptionIndirect", this._assemblyName, this._moduleName);
					this._vendor = resourceReader.GetResourceStringIndirect(mshSnapinKey, "VendorIndirect", this._assemblyName, this._moduleName);
				}
			}
			if (string.IsNullOrEmpty(this._description))
			{
				this._description = this._descriptionFallback;
			}
			if (string.IsNullOrEmpty(this._vendor))
			{
				this._vendor = this._vendorFallback;
			}
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x001B2BB0 File Offset: 0x001B0DB0
		internal PSSnapInInfo Clone()
		{
			return new PSSnapInInfo(this._name, this._isDefault, this._applicationBase, this._assemblyName, this._moduleName, this._psVersion, this._version, new Collection<string>(this.Types), new Collection<string>(this.Formats), this._description, this._descriptionFallback, this._descriptionIndirect, this._vendor, this._vendorFallback, this._vendorIndirect, this._customPSSnapInType);
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x001B2C2E File Offset: 0x001B0E2E
		internal static bool IsPSSnapinIdValid(string psSnapinId)
		{
			return !string.IsNullOrEmpty(psSnapinId) && Regex.IsMatch(psSnapinId, "^[A-Za-z0-9-_.]*$");
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x001B2C48 File Offset: 0x001B0E48
		internal static void VerifyPSSnapInFormatThrowIfError(string psSnapinId)
		{
			if (!PSSnapInInfo.IsPSSnapinIdValid(psSnapinId))
			{
				throw PSTraceSource.NewArgumentException("mshSnapInId", MshSnapInCmdletResources.InvalidPSSnapInName, new object[]
				{
					psSnapinId
				});
			}
		}

		// Token: 0x040029F4 RID: 10740
		private string _name;

		// Token: 0x040029F5 RID: 10741
		private bool _isDefault;

		// Token: 0x040029F6 RID: 10742
		private string _applicationBase;

		// Token: 0x040029F7 RID: 10743
		private string _assemblyName;

		// Token: 0x040029F8 RID: 10744
		private string _moduleName;

		// Token: 0x040029F9 RID: 10745
		private string _customPSSnapInType;

		// Token: 0x040029FA RID: 10746
		private Version _psVersion;

		// Token: 0x040029FB RID: 10747
		private Version _version;

		// Token: 0x040029FC RID: 10748
		private Collection<string> _types;

		// Token: 0x040029FD RID: 10749
		private Collection<string> _formats;

		// Token: 0x040029FE RID: 10750
		private string _descriptionIndirect;

		// Token: 0x040029FF RID: 10751
		private string _descriptionFallback = string.Empty;

		// Token: 0x04002A00 RID: 10752
		private string _description;

		// Token: 0x04002A01 RID: 10753
		private string _vendorIndirect;

		// Token: 0x04002A02 RID: 10754
		private string _vendorFallback = string.Empty;

		// Token: 0x04002A03 RID: 10755
		private string _vendor;

		// Token: 0x04002A04 RID: 10756
		private bool _logPipelineExecutionDetails;
	}
}
