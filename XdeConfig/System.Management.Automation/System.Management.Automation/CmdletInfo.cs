using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200004D RID: 77
	public class CmdletInfo : CommandInfo
	{
		// Token: 0x060003FF RID: 1023 RVA: 0x0000E4D4 File Offset: 0x0000C6D4
		internal CmdletInfo(string name, Type implementingType, string helpFile, PSSnapInInfo PSSnapin, ExecutionContext context) : base(name, CommandTypes.Cmdlet, context)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (!CmdletInfo.SplitCmdletName(name, out this.verb, out this.noun))
			{
				throw PSTraceSource.NewArgumentException("name", DiscoveryExceptions.InvalidCmdletNameFormat, new object[]
				{
					name
				});
			}
			this.implementingType = implementingType;
			this.helpFilePath = helpFile;
			this._PSSnapin = PSSnapin;
			this.options = ScopedItemOptions.ReadOnly;
			base.DefiningLanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000E578 File Offset: 0x0000C778
		internal CmdletInfo(CmdletInfo other) : base(other)
		{
			this.verb = other.verb;
			this.noun = other.noun;
			this.implementingType = other.implementingType;
			this.helpFilePath = other.helpFilePath;
			this._PSSnapin = other._PSSnapin;
			this.options = ScopedItemOptions.ReadOnly;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000E5F0 File Offset: 0x0000C7F0
		internal override CommandInfo CreateGetCommandCopy(object[] arguments)
		{
			return new CmdletInfo(this)
			{
				IsGetCommandCopy = true,
				Arguments = arguments
			};
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000E614 File Offset: 0x0000C814
		public CmdletInfo(string name, Type implementingType) : base(name, CommandTypes.Cmdlet, null)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (implementingType == null)
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			if (!typeof(Cmdlet).IsAssignableFrom(implementingType))
			{
				throw PSTraceSource.NewInvalidOperationException(DiscoveryExceptions.CmdletDoesNotDeriveFromCmdletType, new object[]
				{
					"implementingType",
					implementingType.FullName
				});
			}
			if (!CmdletInfo.SplitCmdletName(name, out this.verb, out this.noun))
			{
				throw PSTraceSource.NewArgumentException("name", DiscoveryExceptions.InvalidCmdletNameFormat, new object[]
				{
					name
				});
			}
			this.implementingType = implementingType;
			this.helpFilePath = string.Empty;
			this._PSSnapin = null;
			this.options = ScopedItemOptions.ReadOnly;
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0000E6F8 File Offset: 0x0000C8F8
		public string Verb
		{
			get
			{
				return this.verb;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000E700 File Offset: 0x0000C900
		public string Noun
		{
			get
			{
				return this.noun;
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000E708 File Offset: 0x0000C908
		internal static bool SplitCmdletName(string name, out string verb, out string noun)
		{
			string empty;
			verb = (empty = string.Empty);
			noun = empty;
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < name.Length; i++)
			{
				if (SpecialCharacters.IsDash(name[i]))
				{
					num = i;
					break;
				}
			}
			if (num > 0)
			{
				verb = name.Substring(0, num);
				noun = name.Substring(num + 1);
				return true;
			}
			return false;
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000E76C File Offset: 0x0000C96C
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x0000E774 File Offset: 0x0000C974
		public string HelpFile
		{
			get
			{
				return this.helpFilePath;
			}
			internal set
			{
				this.helpFilePath = value;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0000E77D File Offset: 0x0000C97D
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Cmdlet;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000E780 File Offset: 0x0000C980
		public PSSnapInInfo PSSnapIn
		{
			get
			{
				return this._PSSnapin;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0000E788 File Offset: 0x0000C988
		internal string PSSnapInName
		{
			get
			{
				string result = null;
				if (this._PSSnapin != null)
				{
					result = this._PSSnapin.Name;
				}
				return result;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000E7AC File Offset: 0x0000C9AC
		public override Version Version
		{
			get
			{
				if (this._version == null)
				{
					if (base.Module != null)
					{
						this._version = base.Version;
					}
					else if (this._PSSnapin != null)
					{
						this._version = this._PSSnapin.Version;
					}
				}
				return this._version;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0000E7FC File Offset: 0x0000C9FC
		public Type ImplementingType
		{
			get
			{
				return this.implementingType;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0000E804 File Offset: 0x0000CA04
		public override string Definition
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.ImplementingType != null)
				{
					using (IEnumerator<CommandParameterSetInfo> enumerator = base.ParameterSets.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CommandParameterSetInfo commandParameterSetInfo = enumerator.Current;
							stringBuilder.AppendLine();
							stringBuilder.AppendLine(string.Format(CultureInfo.CurrentCulture, "{0}{1}{2} {3}", new object[]
							{
								this.verb,
								'-',
								this.noun,
								commandParameterSetInfo.ToString((base.CommandType & CommandTypes.Workflow) == CommandTypes.Workflow)
							}));
						}
						goto IL_DC;
					}
				}
				stringBuilder.AppendLine(string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", new object[]
				{
					this.verb,
					'-',
					this.noun
				}));
				IL_DC:
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000E904 File Offset: 0x0000CB04
		public string DefaultParameterSet
		{
			get
			{
				return this.CommandMetadata.DefaultParameterSetName;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0000E914 File Offset: 0x0000CB14
		public override ReadOnlyCollection<PSTypeName> OutputType
		{
			get
			{
				if (this._outputType == null)
				{
					this._outputType = new List<PSTypeName>();
					if (this.ImplementingType != null)
					{
						foreach (object obj in this.ImplementingType.GetTypeInfo().GetCustomAttributes(typeof(OutputTypeAttribute), false))
						{
							OutputTypeAttribute outputTypeAttribute = (OutputTypeAttribute)obj;
							this._outputType.AddRange(outputTypeAttribute.Type);
						}
					}
				}
				List<PSTypeName> list = new List<PSTypeName>();
				if (base.Context != null)
				{
					ProviderInfo providerInfo = null;
					if (base.Arguments != null)
					{
						for (int j = 0; j < base.Arguments.Length - 1; j++)
						{
							string text = base.Arguments[j] as string;
							if (text != null && (text.Equals("-Path", StringComparison.OrdinalIgnoreCase) || text.Equals("-LiteralPath", StringComparison.OrdinalIgnoreCase)))
							{
								string text2 = base.Arguments[j + 1] as string;
								if (text2 != null)
								{
									base.Context.SessionState.Path.GetResolvedProviderPathFromPSPath(text2, true, out providerInfo);
								}
							}
						}
					}
					if (providerInfo == null)
					{
						providerInfo = base.Context.SessionState.Path.CurrentLocation.Provider;
					}
					providerInfo.GetOutputTypes(base.Name, list);
					if (list.Count > 0)
					{
						list.InsertRange(0, this._outputType);
						return new ReadOnlyCollection<PSTypeName>(list);
					}
				}
				return new ReadOnlyCollection<PSTypeName>(this._outputType);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0000EA7A File Offset: 0x0000CC7A
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x0000EA82 File Offset: 0x0000CC82
		public ScopedItemOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.SetOptions(value, false);
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000EA8C File Offset: 0x0000CC8C
		internal void SetOptions(ScopedItemOptions newOptions, bool force)
		{
			if ((this.options & ScopedItemOptions.ReadOnly) != ScopedItemOptions.None)
			{
				SessionStateUnauthorizedAccessException ex = new SessionStateUnauthorizedAccessException(base.Name, SessionStateCategory.Cmdlet, "CmdletIsReadOnly", SessionStateStrings.CmdletIsReadOnly);
				throw ex;
			}
			this.options = newOptions;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000EAC4 File Offset: 0x0000CCC4
		private static string GetFullName(string moduleName, string cmdletName)
		{
			string text = cmdletName;
			if (!string.IsNullOrEmpty(moduleName))
			{
				text = moduleName + '\\' + text;
			}
			return text;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000EAEB File Offset: 0x0000CCEB
		private static string GetFullName(CmdletInfo cmdletInfo)
		{
			return CmdletInfo.GetFullName(cmdletInfo.ModuleName, cmdletInfo.Name);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000EB00 File Offset: 0x0000CD00
		internal static string GetFullName(PSObject psObject)
		{
			if (psObject.BaseObject is CmdletInfo)
			{
				CmdletInfo cmdletInfo = (CmdletInfo)psObject.BaseObject;
				return CmdletInfo.GetFullName(cmdletInfo);
			}
			PSPropertyInfo pspropertyInfo = psObject.Properties["Name"];
			PSPropertyInfo pspropertyInfo2 = psObject.Properties["PSSnapIn"];
			string cmdletName = (pspropertyInfo == null) ? "" : ((string)pspropertyInfo.Value);
			string moduleName = (pspropertyInfo2 == null) ? "" : ((string)pspropertyInfo2.Value);
			return CmdletInfo.GetFullName(moduleName, cmdletName);
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0000EB84 File Offset: 0x0000CD84
		internal string FullName
		{
			get
			{
				return CmdletInfo.GetFullName(this);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0000EB8C File Offset: 0x0000CD8C
		internal override CommandMetadata CommandMetadata
		{
			get
			{
				if (this.cmdletMetadata == null)
				{
					this.cmdletMetadata = CommandMetadata.Get(base.Name, this.ImplementingType, base.Context);
				}
				return this.cmdletMetadata;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0000EBB9 File Offset: 0x0000CDB9
		internal override bool ImplementsDynamicParameters
		{
			get
			{
				return this.ImplementingType != null && this.ImplementingType.GetInterface(typeof(IDynamicParameters).Name, true) != null;
			}
		}

		// Token: 0x04000176 RID: 374
		private string verb = string.Empty;

		// Token: 0x04000177 RID: 375
		private string noun = string.Empty;

		// Token: 0x04000178 RID: 376
		private string helpFilePath = string.Empty;

		// Token: 0x04000179 RID: 377
		private PSSnapInInfo _PSSnapin;

		// Token: 0x0400017A RID: 378
		private Version _version;

		// Token: 0x0400017B RID: 379
		private Type implementingType;

		// Token: 0x0400017C RID: 380
		private List<PSTypeName> _outputType;

		// Token: 0x0400017D RID: 381
		private ScopedItemOptions options;

		// Token: 0x0400017E RID: 382
		private CommandMetadata cmdletMetadata;
	}
}
