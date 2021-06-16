using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Provider;
using System.Reflection;
using System.Security;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000461 RID: 1121
	public class ProviderInfo
	{
		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06003155 RID: 12629 RVA: 0x0010CC48 File Offset: 0x0010AE48
		public Type ImplementingType
		{
			get
			{
				return this.implementingType;
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06003156 RID: 12630 RVA: 0x0010CC50 File Offset: 0x0010AE50
		public string HelpFile
		{
			get
			{
				return this.helpFile;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06003157 RID: 12631 RVA: 0x0010CC58 File Offset: 0x0010AE58
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06003158 RID: 12632 RVA: 0x0010CC60 File Offset: 0x0010AE60
		internal string FullName
		{
			get
			{
				string result = this.Name;
				if (!string.IsNullOrEmpty(this.PSSnapInName))
				{
					result = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", new object[]
					{
						this.PSSnapInName,
						this.Name
					});
				}
				else if (!string.IsNullOrEmpty(this.ModuleName))
				{
					result = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", new object[]
					{
						this.ModuleName,
						this.Name
					});
				}
				return result;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06003159 RID: 12633 RVA: 0x0010CCE5 File Offset: 0x0010AEE5
		public PSSnapInInfo PSSnapIn
		{
			get
			{
				return this.pssnapin;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x0600315A RID: 12634 RVA: 0x0010CCF0 File Offset: 0x0010AEF0
		internal string PSSnapInName
		{
			get
			{
				string result = null;
				if (this.pssnapin != null)
				{
					result = this.pssnapin.Name;
				}
				return result;
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x0600315B RID: 12635 RVA: 0x0010CD14 File Offset: 0x0010AF14
		internal string ApplicationBase
		{
			get
			{
				string result = null;
				try
				{
					result = Utils.GetApplicationBase(Utils.DefaultPowerShellShellID);
				}
				catch (SecurityException)
				{
					result = null;
				}
				return result;
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x0600315C RID: 12636 RVA: 0x0010CD48 File Offset: 0x0010AF48
		public string ModuleName
		{
			get
			{
				if (this.pssnapin != null)
				{
					return this.pssnapin.Name;
				}
				if (this._module != null)
				{
					return this._module.Name;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x0600315D RID: 12637 RVA: 0x0010CD77 File Offset: 0x0010AF77
		public PSModuleInfo Module
		{
			get
			{
				return this._module;
			}
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x0010CD7F File Offset: 0x0010AF7F
		internal void SetModule(PSModuleInfo module)
		{
			this._module = module;
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x0600315F RID: 12639 RVA: 0x0010CD88 File Offset: 0x0010AF88
		// (set) Token: 0x06003160 RID: 12640 RVA: 0x0010CD90 File Offset: 0x0010AF90
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06003161 RID: 12641 RVA: 0x0010CD9C File Offset: 0x0010AF9C
		public ProviderCapabilities Capabilities
		{
			get
			{
				if (!this.capabilitiesRead)
				{
					try
					{
						Type type = this.ImplementingType;
						IEnumerable<CmdletProviderAttribute> customAttributes = type.GetCustomAttributes(false);
						CmdletProviderAttribute[] array = (customAttributes as CmdletProviderAttribute[]) ?? customAttributes.ToArray<CmdletProviderAttribute>();
						if (array.Length == 1)
						{
							this.capabilities = array[0].ProviderCapabilities;
							this.capabilitiesRead = true;
						}
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
				}
				return this.capabilities;
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06003162 RID: 12642 RVA: 0x0010CE10 File Offset: 0x0010B010
		// (set) Token: 0x06003163 RID: 12643 RVA: 0x0010CE18 File Offset: 0x0010B018
		public string Home
		{
			get
			{
				return this.home;
			}
			set
			{
				this.home = value;
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06003164 RID: 12644 RVA: 0x0010CE21 File Offset: 0x0010B021
		public Collection<PSDriveInfo> Drives
		{
			get
			{
				return this.sessionState.Drive.GetAllForProvider(this.FullName);
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06003165 RID: 12645 RVA: 0x0010CE39 File Offset: 0x0010B039
		internal PSDriveInfo HiddenDrive
		{
			get
			{
				return this.hiddenDrive;
			}
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x0010CE41 File Offset: 0x0010B041
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x0010CE4C File Offset: 0x0010B04C
		protected ProviderInfo(ProviderInfo providerInfo)
		{
			if (providerInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("providerInfo");
			}
			this.name = providerInfo.Name;
			this.implementingType = providerInfo.ImplementingType;
			this.capabilities = providerInfo.capabilities;
			this.description = providerInfo.description;
			this.hiddenDrive = providerInfo.hiddenDrive;
			this.home = providerInfo.home;
			this.helpFile = providerInfo.helpFile;
			this.pssnapin = providerInfo.pssnapin;
			this.sessionState = providerInfo.sessionState;
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x0010CEE4 File Offset: 0x0010B0E4
		internal ProviderInfo(SessionState sessionState, Type implementingType, string name, string helpFile, PSSnapInInfo psSnapIn) : this(sessionState, implementingType, name, string.Empty, string.Empty, helpFile, psSnapIn)
		{
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x0010CF00 File Offset: 0x0010B100
		internal ProviderInfo(SessionState sessionState, Type implementingType, string name, string description, string home, string helpFile, PSSnapInInfo psSnapIn)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			if (implementingType == null)
			{
				throw PSTraceSource.NewArgumentNullException("implementingType");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.sessionState = sessionState;
			this.name = name;
			this.description = description;
			this.home = home;
			this.implementingType = implementingType;
			this.helpFile = helpFile;
			this.pssnapin = psSnapIn;
			this.hiddenDrive = new PSDriveInfo(this.FullName, this, "", "", null);
			this.hiddenDrive.Hidden = true;
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x0010CFC4 File Offset: 0x0010B1C4
		internal bool NameEquals(string providerName)
		{
			PSSnapinQualifiedName instance = PSSnapinQualifiedName.GetInstance(providerName);
			bool result = false;
			if (instance != null)
			{
				if (string.IsNullOrEmpty(instance.PSSnapInName) || string.Equals(instance.PSSnapInName, this.PSSnapInName, StringComparison.OrdinalIgnoreCase) || string.Equals(instance.PSSnapInName, this.ModuleName, StringComparison.OrdinalIgnoreCase))
				{
					result = string.Equals(instance.ShortName, this.Name, StringComparison.OrdinalIgnoreCase);
				}
			}
			else
			{
				result = string.Equals(providerName, this.Name, StringComparison.OrdinalIgnoreCase);
			}
			return result;
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x0010D038 File Offset: 0x0010B238
		internal bool IsMatch(string providerName)
		{
			PSSnapinQualifiedName instance = PSSnapinQualifiedName.GetInstance(providerName);
			WildcardPattern namePattern = null;
			if (instance != null && WildcardPattern.ContainsWildcardCharacters(instance.ShortName))
			{
				namePattern = new WildcardPattern(instance.ShortName, WildcardOptions.IgnoreCase);
			}
			return this.IsMatch(namePattern, instance);
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x0010D074 File Offset: 0x0010B274
		internal bool IsMatch(WildcardPattern namePattern, PSSnapinQualifiedName psSnapinQualifiedName)
		{
			bool result = false;
			if (psSnapinQualifiedName == null)
			{
				result = true;
			}
			else if (namePattern == null)
			{
				if (string.Equals(this.Name, psSnapinQualifiedName.ShortName, StringComparison.OrdinalIgnoreCase) && this.IsPSSnapinNameMatch(psSnapinQualifiedName))
				{
					result = true;
				}
			}
			else if (namePattern.IsMatch(this.Name) && this.IsPSSnapinNameMatch(psSnapinQualifiedName))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x0010D0C8 File Offset: 0x0010B2C8
		private bool IsPSSnapinNameMatch(PSSnapinQualifiedName psSnapinQualifiedName)
		{
			bool result = false;
			if (string.IsNullOrEmpty(psSnapinQualifiedName.PSSnapInName) || string.Equals(psSnapinQualifiedName.PSSnapInName, this.PSSnapInName, StringComparison.OrdinalIgnoreCase))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x0010D0FC File Offset: 0x0010B2FC
		internal CmdletProvider CreateInstance()
		{
			object obj = null;
			Exception ex = null;
			try
			{
				obj = Activator.CreateInstance(this.ImplementingType);
			}
			catch (TargetInvocationException ex2)
			{
				ex = ex2.InnerException;
			}
			catch (MissingMethodException)
			{
			}
			catch (MemberAccessException)
			{
			}
			catch (ArgumentException)
			{
			}
			if (obj == null)
			{
				ProviderNotFoundException ex3;
				if (ex != null)
				{
					ex3 = new ProviderNotFoundException(this.Name, SessionStateCategory.CmdletProvider, "ProviderCtorException", SessionStateStrings.ProviderCtorException, new object[]
					{
						ex.Message
					});
				}
				else
				{
					ex3 = new ProviderNotFoundException(this.Name, SessionStateCategory.CmdletProvider, "ProviderNotFoundInAssembly", SessionStateStrings.ProviderNotFoundInAssembly, new object[0]);
				}
				throw ex3;
			}
			CmdletProvider cmdletProvider = obj as CmdletProvider;
			cmdletProvider.SetProviderInformation(this);
			return cmdletProvider;
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x0010D1C8 File Offset: 0x0010B3C8
		internal void GetOutputTypes(string cmdletname, List<PSTypeName> listToAppend)
		{
			if (this.providerOutputType == null)
			{
				this.providerOutputType = new Dictionary<string, List<PSTypeName>>();
				foreach (OutputTypeAttribute outputTypeAttribute in this.implementingType.GetCustomAttributes(false))
				{
					if (!string.IsNullOrEmpty(outputTypeAttribute.ProviderCmdlet))
					{
						List<PSTypeName> list;
						if (!this.providerOutputType.TryGetValue(outputTypeAttribute.ProviderCmdlet, out list))
						{
							list = new List<PSTypeName>();
							this.providerOutputType[outputTypeAttribute.ProviderCmdlet] = list;
						}
						list.AddRange(outputTypeAttribute.Type);
					}
				}
			}
			List<PSTypeName> collection = null;
			if (this.providerOutputType.TryGetValue(cmdletname, out collection))
			{
				listToAppend.AddRange(collection);
			}
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x0010D28C File Offset: 0x0010B48C
		internal PSNoteProperty GetNotePropertyForProviderCmdlets(string name)
		{
			if (this._noteProperty == null)
			{
				Interlocked.CompareExchange<PSNoteProperty>(ref this._noteProperty, new PSNoteProperty(name, this), null);
			}
			return this._noteProperty;
		}

		// Token: 0x04001A4D RID: 6733
		private Type implementingType;

		// Token: 0x04001A4E RID: 6734
		private string helpFile = "";

		// Token: 0x04001A4F RID: 6735
		private SessionState sessionState;

		// Token: 0x04001A50 RID: 6736
		private string name;

		// Token: 0x04001A51 RID: 6737
		private PSSnapInInfo pssnapin;

		// Token: 0x04001A52 RID: 6738
		private PSModuleInfo _module;

		// Token: 0x04001A53 RID: 6739
		private string description;

		// Token: 0x04001A54 RID: 6740
		private ProviderCapabilities capabilities;

		// Token: 0x04001A55 RID: 6741
		private bool capabilitiesRead;

		// Token: 0x04001A56 RID: 6742
		private string home;

		// Token: 0x04001A57 RID: 6743
		private PSDriveInfo hiddenDrive;

		// Token: 0x04001A58 RID: 6744
		private Dictionary<string, List<PSTypeName>> providerOutputType;

		// Token: 0x04001A59 RID: 6745
		private PSNoteProperty _noteProperty;
	}
}
