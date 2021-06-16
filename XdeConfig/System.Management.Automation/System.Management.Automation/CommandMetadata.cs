using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Reflection;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200042B RID: 1067
	[DebuggerDisplay("CommandName = {_commandName}; Type = {CommandType}")]
	public sealed class CommandMetadata
	{
		// Token: 0x06002EFE RID: 12030 RVA: 0x00101088 File Offset: 0x000FF288
		public CommandMetadata(Type commandType)
		{
			this.Init(null, null, commandType, false);
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x001010E6 File Offset: 0x000FF2E6
		public CommandMetadata(CommandInfo commandInfo) : this(commandInfo, false)
		{
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x001010F0 File Offset: 0x000FF2F0
		public CommandMetadata(CommandInfo commandInfo, bool shouldGenerateCommonParameters)
		{
			if (commandInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandInfo");
			}
			while (commandInfo is AliasInfo)
			{
				commandInfo = ((AliasInfo)commandInfo).ResolvedCommand;
				if (commandInfo == null)
				{
					throw PSTraceSource.NewNotSupportedException();
				}
			}
			CmdletInfo cmdletInfo;
			if ((cmdletInfo = (commandInfo as CmdletInfo)) != null)
			{
				this.Init(commandInfo.Name, cmdletInfo.FullName, cmdletInfo.ImplementingType, shouldGenerateCommonParameters);
				return;
			}
			ExternalScriptInfo externalScriptInfo;
			if ((externalScriptInfo = (commandInfo as ExternalScriptInfo)) != null)
			{
				this.Init(externalScriptInfo.ScriptBlock, externalScriptInfo.Path, shouldGenerateCommonParameters);
				this._wrappedCommandType = CommandTypes.ExternalScript;
				return;
			}
			FunctionInfo functionInfo;
			if ((functionInfo = (commandInfo as FunctionInfo)) != null)
			{
				this.Init(functionInfo.ScriptBlock, functionInfo.Name, shouldGenerateCommonParameters);
				this._wrappedCommandType = commandInfo.CommandType;
				return;
			}
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x001011EC File Offset: 0x000FF3EC
		public CommandMetadata(string path)
		{
			string fileName = Path.GetFileName(path);
			ExternalScriptInfo externalScriptInfo = new ExternalScriptInfo(fileName, path);
			this.Init(externalScriptInfo.ScriptBlock, path, false);
			this._wrappedCommandType = CommandTypes.ExternalScript;
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x00101268 File Offset: 0x000FF468
		public CommandMetadata(CommandMetadata other)
		{
			if (other == null)
			{
				throw PSTraceSource.NewArgumentNullException("other");
			}
			this._commandName = other._commandName;
			this._confirmImpact = other._confirmImpact;
			this._defaultParameterSetFlag = other._defaultParameterSetFlag;
			this._defaultParameterSetName = other._defaultParameterSetName;
			this._implementsDynamicParameters = other._implementsDynamicParameters;
			this._supportsShouldProcess = other._supportsShouldProcess;
			this._supportsPaging = other._supportsPaging;
			this._supportsTransactions = other._supportsTransactions;
			this.CommandType = other.CommandType;
			this._wrappedAnyCmdlet = other._wrappedAnyCmdlet;
			this._wrappedCommand = other._wrappedCommand;
			this._wrappedCommandType = other._wrappedCommandType;
			this._parameters = new Dictionary<string, ParameterMetadata>(other.Parameters.Count, StringComparer.OrdinalIgnoreCase);
			if (other.Parameters != null)
			{
				foreach (KeyValuePair<string, ParameterMetadata> keyValuePair in other.Parameters)
				{
					this._parameters.Add(keyValuePair.Key, new ParameterMetadata(keyValuePair.Value));
				}
			}
			if (other._otherAttributes == null)
			{
				this._otherAttributes = null;
			}
			else
			{
				this._otherAttributes = new Collection<Attribute>(new List<Attribute>(other._otherAttributes.Count));
				foreach (Attribute item in other._otherAttributes)
				{
					this._otherAttributes.Add(item);
				}
			}
			this.staticCommandParameterMetadata = null;
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x00101450 File Offset: 0x000FF650
		internal CommandMetadata(string name, CommandTypes commandType, bool isProxyForCmdlet, string defaultParameterSetName, bool supportsShouldProcess, ConfirmImpact confirmImpact, bool supportsPaging, bool supportsTransactions, bool positionalBinding, Dictionary<string, ParameterMetadata> parameters)
		{
			this._wrappedCommand = name;
			this._commandName = name;
			this._wrappedCommandType = commandType;
			this._wrappedAnyCmdlet = isProxyForCmdlet;
			this._defaultParameterSetName = defaultParameterSetName;
			this._supportsShouldProcess = supportsShouldProcess;
			this._supportsPaging = supportsPaging;
			this._confirmImpact = confirmImpact;
			this._supportsTransactions = supportsTransactions;
			this._positionalBinding = positionalBinding;
			this.Parameters = parameters;
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x001014FC File Offset: 0x000FF6FC
		private void Init(string name, string fullyQualifiedName, Type commandType, bool shouldGenerateCommonParameters)
		{
			this._commandName = name;
			this.CommandType = commandType;
			if (commandType != null)
			{
				this.ConstructCmdletMetadataUsingReflection();
				this._shouldGenerateCommonParameters = shouldGenerateCommonParameters;
			}
			this._wrappedCommand = ((!string.IsNullOrEmpty(fullyQualifiedName)) ? fullyQualifiedName : this._commandName);
			this._wrappedCommandType = CommandTypes.Cmdlet;
			this._wrappedAnyCmdlet = true;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x00101554 File Offset: 0x000FF754
		private void Init(ScriptBlock scriptBlock, string name, bool shouldGenerateCommonParameters)
		{
			if (scriptBlock.UsesCmdletBinding)
			{
				this._wrappedAnyCmdlet = true;
			}
			else
			{
				shouldGenerateCommonParameters = false;
			}
			CmdletBindingAttribute cmdletBindingAttribute = scriptBlock.CmdletBindingAttribute;
			if (cmdletBindingAttribute != null)
			{
				this.ProcessCmdletAttribute(cmdletBindingAttribute);
			}
			else if (scriptBlock.UsesCmdletBinding)
			{
				this._defaultParameterSetName = null;
			}
			this._obsolete = scriptBlock.ObsoleteAttribute;
			this._scriptBlock = scriptBlock;
			this._commandName = name;
			this._wrappedCommand = name;
			this._shouldGenerateCommonParameters = shouldGenerateCommonParameters;
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x001015C4 File Offset: 0x000FF7C4
		internal static CommandMetadata Get(string commandName, Type cmdletType, ExecutionContext context)
		{
			if (string.IsNullOrEmpty(commandName))
			{
				throw PSTraceSource.NewArgumentException("commandName");
			}
			CommandMetadata commandMetadata = null;
			if (context != null && cmdletType != null)
			{
				string assemblyQualifiedName = cmdletType.AssemblyQualifiedName;
				if (CommandMetadata.CommandMetadataCache.ContainsKey(assemblyQualifiedName))
				{
					commandMetadata = CommandMetadata.CommandMetadataCache[assemblyQualifiedName];
				}
			}
			if (commandMetadata == null)
			{
				commandMetadata = new CommandMetadata(commandName, cmdletType, context);
				if (context != null && cmdletType != null)
				{
					string assemblyQualifiedName2 = cmdletType.AssemblyQualifiedName;
					CommandMetadata.CommandMetadataCache.TryAdd(assemblyQualifiedName2, commandMetadata);
				}
			}
			return commandMetadata;
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x00101640 File Offset: 0x000FF840
		internal CommandMetadata(string commandName, Type cmdletType, ExecutionContext context)
		{
			if (string.IsNullOrEmpty(commandName))
			{
				throw PSTraceSource.NewArgumentException("commandName");
			}
			this._commandName = commandName;
			this.CommandType = cmdletType;
			if (cmdletType != null)
			{
				InternalParameterMetadata parameterMetadata = InternalParameterMetadata.Get(cmdletType, context, false);
				this.ConstructCmdletMetadataUsingReflection();
				this.staticCommandParameterMetadata = this.MergeParameterMetadata(context, parameterMetadata, true);
				this._defaultParameterSetFlag = this.staticCommandParameterMetadata.GenerateParameterSetMappingFromMetadata(this._defaultParameterSetName);
				this.staticCommandParameterMetadata.MakeReadOnly();
			}
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x00101700 File Offset: 0x000FF900
		internal CommandMetadata(ScriptBlock scriptblock, string commandName, ExecutionContext context)
		{
			if (scriptblock == null)
			{
				throw PSTraceSource.NewArgumentException("scriptblock");
			}
			CmdletBindingAttribute cmdletBindingAttribute = scriptblock.CmdletBindingAttribute;
			if (cmdletBindingAttribute != null)
			{
				this.ProcessCmdletAttribute(cmdletBindingAttribute);
			}
			else
			{
				this._defaultParameterSetName = null;
			}
			this._obsolete = scriptblock.ObsoleteAttribute;
			this._commandName = commandName;
			this.CommandType = typeof(PSScriptCmdlet);
			if (scriptblock.HasDynamicParameters)
			{
				this._implementsDynamicParameters = true;
			}
			InternalParameterMetadata parameterMetadata = InternalParameterMetadata.Get(scriptblock.RuntimeDefinedParameters, false, scriptblock.UsesCmdletBinding);
			this.staticCommandParameterMetadata = this.MergeParameterMetadata(context, parameterMetadata, scriptblock.UsesCmdletBinding);
			this._defaultParameterSetFlag = this.staticCommandParameterMetadata.GenerateParameterSetMappingFromMetadata(this._defaultParameterSetName);
			this.staticCommandParameterMetadata.MakeReadOnly();
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06002F09 RID: 12041 RVA: 0x001017F7 File Offset: 0x000FF9F7
		// (set) Token: 0x06002F0A RID: 12042 RVA: 0x001017FF File Offset: 0x000FF9FF
		public string Name
		{
			get
			{
				return this._commandName;
			}
			set
			{
				this._commandName = value;
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06002F0B RID: 12043 RVA: 0x00101808 File Offset: 0x000FFA08
		// (set) Token: 0x06002F0C RID: 12044 RVA: 0x00101810 File Offset: 0x000FFA10
		public Type CommandType { get; private set; }

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06002F0D RID: 12045 RVA: 0x00101819 File Offset: 0x000FFA19
		// (set) Token: 0x06002F0E RID: 12046 RVA: 0x00101821 File Offset: 0x000FFA21
		public string DefaultParameterSetName
		{
			get
			{
				return this._defaultParameterSetName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					value = "__AllParameterSets";
				}
				this._defaultParameterSetName = value;
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06002F0F RID: 12047 RVA: 0x00101839 File Offset: 0x000FFA39
		// (set) Token: 0x06002F10 RID: 12048 RVA: 0x00101841 File Offset: 0x000FFA41
		public bool SupportsShouldProcess
		{
			get
			{
				return this._supportsShouldProcess;
			}
			set
			{
				this._supportsShouldProcess = value;
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06002F11 RID: 12049 RVA: 0x0010184A File Offset: 0x000FFA4A
		// (set) Token: 0x06002F12 RID: 12050 RVA: 0x00101852 File Offset: 0x000FFA52
		public bool SupportsPaging
		{
			get
			{
				return this._supportsPaging;
			}
			set
			{
				this._supportsPaging = value;
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06002F13 RID: 12051 RVA: 0x0010185B File Offset: 0x000FFA5B
		// (set) Token: 0x06002F14 RID: 12052 RVA: 0x00101863 File Offset: 0x000FFA63
		public bool PositionalBinding
		{
			get
			{
				return this._positionalBinding;
			}
			set
			{
				this._positionalBinding = value;
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06002F15 RID: 12053 RVA: 0x0010186C File Offset: 0x000FFA6C
		// (set) Token: 0x06002F16 RID: 12054 RVA: 0x00101874 File Offset: 0x000FFA74
		public bool SupportsTransactions
		{
			get
			{
				return this._supportsTransactions;
			}
			set
			{
				this._supportsTransactions = value;
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06002F17 RID: 12055 RVA: 0x0010187D File Offset: 0x000FFA7D
		// (set) Token: 0x06002F18 RID: 12056 RVA: 0x00101885 File Offset: 0x000FFA85
		public string HelpUri
		{
			get
			{
				return this._helpUri;
			}
			set
			{
				this._helpUri = value;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06002F19 RID: 12057 RVA: 0x00101890 File Offset: 0x000FFA90
		// (set) Token: 0x06002F1A RID: 12058 RVA: 0x001018CF File Offset: 0x000FFACF
		public RemotingCapability RemotingCapability
		{
			get
			{
				RemotingCapability remotingCapability = this._remotingCapability;
				if (remotingCapability == RemotingCapability.PowerShell && this.Parameters != null && this.Parameters.ContainsKey("ComputerName"))
				{
					this._remotingCapability = RemotingCapability.SupportedByCommand;
				}
				return this._remotingCapability;
			}
			set
			{
				this._remotingCapability = value;
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06002F1B RID: 12059 RVA: 0x001018D8 File Offset: 0x000FFAD8
		// (set) Token: 0x06002F1C RID: 12060 RVA: 0x001018E0 File Offset: 0x000FFAE0
		public ConfirmImpact ConfirmImpact
		{
			get
			{
				return this._confirmImpact;
			}
			set
			{
				this._confirmImpact = value;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06002F1D RID: 12061 RVA: 0x001018EC File Offset: 0x000FFAEC
		// (set) Token: 0x06002F1E RID: 12062 RVA: 0x00101980 File Offset: 0x000FFB80
		public Dictionary<string, ParameterMetadata> Parameters
		{
			get
			{
				if (this._parameters == null)
				{
					if (this._scriptBlock != null)
					{
						InternalParameterMetadata parameterMetadata = InternalParameterMetadata.Get(this._scriptBlock.RuntimeDefinedParameters, false, this._scriptBlock.UsesCmdletBinding);
						MergedCommandParameterMetadata cmdParameterMetadata = this.MergeParameterMetadata(null, parameterMetadata, this._shouldGenerateCommonParameters);
						this._parameters = ParameterMetadata.GetParameterMetadata(cmdParameterMetadata);
					}
					else if (this.CommandType != null)
					{
						InternalParameterMetadata parameterMetadata2 = InternalParameterMetadata.Get(this.CommandType, null, false);
						MergedCommandParameterMetadata cmdParameterMetadata2 = this.MergeParameterMetadata(null, parameterMetadata2, this._shouldGenerateCommonParameters);
						this._parameters = ParameterMetadata.GetParameterMetadata(cmdParameterMetadata2);
					}
				}
				return this._parameters;
			}
			private set
			{
				this._parameters = value;
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06002F1F RID: 12063 RVA: 0x00101989 File Offset: 0x000FFB89
		// (set) Token: 0x06002F20 RID: 12064 RVA: 0x00101991 File Offset: 0x000FFB91
		internal ObsoleteAttribute Obsolete
		{
			get
			{
				return this._obsolete;
			}
			set
			{
				this._obsolete = value;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06002F21 RID: 12065 RVA: 0x0010199A File Offset: 0x000FFB9A
		internal MergedCommandParameterMetadata StaticCommandParameterMetadata
		{
			get
			{
				return this.staticCommandParameterMetadata;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06002F22 RID: 12066 RVA: 0x001019A2 File Offset: 0x000FFBA2
		internal bool ImplementsDynamicParameters
		{
			get
			{
				return this._implementsDynamicParameters;
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06002F23 RID: 12067 RVA: 0x001019AA File Offset: 0x000FFBAA
		// (set) Token: 0x06002F24 RID: 12068 RVA: 0x001019B2 File Offset: 0x000FFBB2
		internal uint DefaultParameterSetFlag
		{
			get
			{
				return this._defaultParameterSetFlag;
			}
			set
			{
				this._defaultParameterSetFlag = value;
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06002F25 RID: 12069 RVA: 0x001019BB File Offset: 0x000FFBBB
		internal bool WrappedAnyCmdlet
		{
			get
			{
				return this._wrappedAnyCmdlet;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06002F26 RID: 12070 RVA: 0x001019C3 File Offset: 0x000FFBC3
		internal CommandTypes WrappedCommandType
		{
			get
			{
				return this._wrappedCommandType;
			}
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x001019CC File Offset: 0x000FFBCC
		private void ConstructCmdletMetadataUsingReflection()
		{
			Type @interface = this.CommandType.GetInterface(typeof(IDynamicParameters).Name, true);
			if (@interface != null)
			{
				this._implementsDynamicParameters = true;
			}
			object[] customAttributes = this.CommandType.GetTypeInfo().GetCustomAttributes(false);
			foreach (Attribute attribute in customAttributes)
			{
				CmdletAttribute cmdletAttribute = attribute as CmdletAttribute;
				if (cmdletAttribute != null)
				{
					this.ProcessCmdletAttribute(cmdletAttribute);
					this.Name = cmdletAttribute.VerbName + "-" + cmdletAttribute.NounName;
				}
				else if (attribute is ObsoleteAttribute)
				{
					this._obsolete = (ObsoleteAttribute)attribute;
				}
				else
				{
					this._otherAttributes.Add(attribute);
				}
			}
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x00101A8C File Offset: 0x000FFC8C
		private void ProcessCmdletAttribute(CmdletCommonMetadataAttribute attribute)
		{
			if (attribute == null)
			{
				throw PSTraceSource.NewArgumentNullException("attribute");
			}
			this._defaultParameterSetName = attribute.DefaultParameterSetName;
			this._supportsShouldProcess = attribute.SupportsShouldProcess;
			this._confirmImpact = attribute.ConfirmImpact;
			this._supportsPaging = attribute.SupportsPaging;
			this._supportsTransactions = attribute.SupportsTransactions;
			this._helpUri = attribute.HelpUri;
			this._remotingCapability = attribute.RemotingCapability;
			CmdletBindingAttribute cmdletBindingAttribute = attribute as CmdletBindingAttribute;
			if (cmdletBindingAttribute != null)
			{
				this.PositionalBinding = cmdletBindingAttribute.PositionalBinding;
			}
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x00101B14 File Offset: 0x000FFD14
		private MergedCommandParameterMetadata MergeParameterMetadata(ExecutionContext context, InternalParameterMetadata parameterMetadata, bool shouldGenerateCommonParameters)
		{
			MergedCommandParameterMetadata mergedCommandParameterMetadata = new MergedCommandParameterMetadata();
			mergedCommandParameterMetadata.AddMetadataForBinder(parameterMetadata, ParameterBinderAssociation.DeclaredFormalParameters);
			if (shouldGenerateCommonParameters)
			{
				InternalParameterMetadata parameterMetadata2 = InternalParameterMetadata.Get(typeof(CommonParameters), context, false);
				mergedCommandParameterMetadata.AddMetadataForBinder(parameterMetadata2, ParameterBinderAssociation.CommonParameters);
				if (this.SupportsShouldProcess)
				{
					InternalParameterMetadata parameterMetadata3 = InternalParameterMetadata.Get(typeof(ShouldProcessParameters), context, false);
					mergedCommandParameterMetadata.AddMetadataForBinder(parameterMetadata3, ParameterBinderAssociation.ShouldProcessParameters);
				}
				if (this.SupportsPaging)
				{
					InternalParameterMetadata parameterMetadata4 = InternalParameterMetadata.Get(typeof(PagingParameters), context, false);
					mergedCommandParameterMetadata.AddMetadataForBinder(parameterMetadata4, ParameterBinderAssociation.PagingParameters);
				}
				if (this.SupportsTransactions)
				{
					InternalParameterMetadata parameterMetadata5 = InternalParameterMetadata.Get(typeof(TransactionParameters), context, false);
					mergedCommandParameterMetadata.AddMetadataForBinder(parameterMetadata5, ParameterBinderAssociation.TransactionParameters);
				}
			}
			return mergedCommandParameterMetadata;
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x00101BC0 File Offset: 0x000FFDC0
		internal string GetProxyCommand(string helpComment, bool generateDynamicParameters)
		{
			if (string.IsNullOrEmpty(helpComment))
			{
				helpComment = string.Format(CultureInfo.InvariantCulture, "\r\n.ForwardHelpTargetName {0}\r\n.ForwardHelpCategory {1}\r\n", new object[]
				{
					this._wrappedCommand,
					this._wrappedCommandType
				});
			}
			string text = string.Empty;
			if (generateDynamicParameters && this.ImplementsDynamicParameters)
			{
				text = string.Format(CultureInfo.InvariantCulture, "\r\ndynamicparam\r\n{{{0}}}\r\n\r\n", new object[]
				{
					this.GetDynamicParamBlock()
				});
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}\r\nparam({1})\r\n\r\n{2}begin\r\n{{{3}}}\r\n\r\nprocess\r\n{{{4}}}\r\n\r\nend\r\n{{{5}}}\r\n<#\r\n{6}\r\n#>\r\n", new object[]
			{
				this.GetDecl(),
				this.GetParamBlock(),
				text,
				this.GetBeginBlock(),
				this.GetProcessBlock(),
				this.GetEndBlock(),
				CodeGeneration.EscapeBlockCommentContent(helpComment)
			});
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x00101C94 File Offset: 0x000FFE94
		internal string GetDecl()
		{
			string result = "";
			string value = "";
			if (this._wrappedAnyCmdlet)
			{
				StringBuilder stringBuilder = new StringBuilder("[CmdletBinding(");
				if (!string.IsNullOrEmpty(this._defaultParameterSetName))
				{
					stringBuilder.Append(value);
					stringBuilder.Append("DefaultParameterSetName='");
					stringBuilder.Append(CodeGeneration.EscapeSingleQuotedStringContent(this._defaultParameterSetName));
					stringBuilder.Append("'");
					value = ", ";
				}
				if (this._supportsShouldProcess)
				{
					stringBuilder.Append(value);
					stringBuilder.Append("SupportsShouldProcess=$true");
					value = ", ";
					stringBuilder.Append(value);
					stringBuilder.Append("ConfirmImpact='");
					stringBuilder.Append(this._confirmImpact);
					stringBuilder.Append("'");
				}
				if (this._supportsPaging)
				{
					stringBuilder.Append(value);
					stringBuilder.Append("SupportsPaging=$true");
					value = ", ";
				}
				if (this._supportsTransactions)
				{
					stringBuilder.Append(value);
					stringBuilder.Append("SupportsTransactions=$true");
					value = ", ";
				}
				if (!this.PositionalBinding)
				{
					stringBuilder.Append(value);
					stringBuilder.Append("PositionalBinding=$false");
					value = ", ";
				}
				if (!string.IsNullOrEmpty(this._helpUri))
				{
					stringBuilder.Append(value);
					stringBuilder.Append("HelpUri='");
					stringBuilder.Append(CodeGeneration.EscapeSingleQuotedStringContent(this._helpUri));
					stringBuilder.Append("'");
					value = ", ";
				}
				if (this._remotingCapability != RemotingCapability.PowerShell)
				{
					stringBuilder.Append(value);
					stringBuilder.Append("RemotingCapability='");
					stringBuilder.Append(this._remotingCapability);
					stringBuilder.Append("'");
				}
				stringBuilder.Append(")]");
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x00101E5C File Offset: 0x0010005C
		internal string GetParamBlock()
		{
			if (this.Parameters.Keys.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string prefix = string.Format(CultureInfo.InvariantCulture, "{0}    ", new object[]
				{
					Environment.NewLine
				});
				string value = "";
				foreach (string text in this.Parameters.Keys)
				{
					ParameterMetadata parameterMetadata = this.Parameters[text];
					string proxyParameterData = parameterMetadata.GetProxyParameterData(prefix, text, this._wrappedAnyCmdlet);
					stringBuilder.Append(value);
					stringBuilder.Append(proxyParameterData);
					value = string.Format(CultureInfo.InvariantCulture, "{0}{1}", new object[]
					{
						",",
						Environment.NewLine
					});
				}
				return stringBuilder.ToString();
			}
			return "";
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x00101F5C File Offset: 0x0010015C
		internal string GetBeginBlock()
		{
			if (string.IsNullOrEmpty(this._wrappedCommand))
			{
				string commandMetadataMissingCommandName = ProxyCommandStrings.CommandMetadataMissingCommandName;
				throw new InvalidOperationException(commandMetadataMissingCommandName);
			}
			string text = "$myInvocation.CommandOrigin";
			if (this._wrappedCommandType == CommandTypes.Function)
			{
				text = "";
			}
			string result;
			if (this._wrappedAnyCmdlet)
			{
				result = string.Format(CultureInfo.InvariantCulture, "\r\n    try {{\r\n        $outBuffer = $null\r\n        if ($PSBoundParameters.TryGetValue('OutBuffer', [ref]$outBuffer))\r\n        {{\r\n            $PSBoundParameters['OutBuffer'] = 1\r\n        }}\r\n        $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('{0}', [System.Management.Automation.CommandTypes]::{1})\r\n        $scriptCmd = {{& $wrappedCmd @PSBoundParameters }}\r\n        $steppablePipeline = $scriptCmd.GetSteppablePipeline({2})\r\n        $steppablePipeline.Begin($PSCmdlet)\r\n    }} catch {{\r\n        throw\r\n    }}\r\n", new object[]
				{
					CodeGeneration.EscapeSingleQuotedStringContent(this._wrappedCommand),
					this._wrappedCommandType,
					text
				});
			}
			else
			{
				result = string.Format(CultureInfo.InvariantCulture, "\r\n    try {{\r\n        $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('{0}', [System.Management.Automation.CommandTypes]::{1})\r\n        $PSBoundParameters.Add('$args', $args)\r\n        $scriptCmd = {{& $wrappedCmd @PSBoundParameters }}\r\n        $steppablePipeline = $scriptCmd.GetSteppablePipeline({2})\r\n        $steppablePipeline.Begin($myInvocation.ExpectingInput, $ExecutionContext)\r\n    }} catch {{\r\n        throw\r\n    }}\r\n", new object[]
				{
					CodeGeneration.EscapeSingleQuotedStringContent(this._wrappedCommand),
					this._wrappedCommandType,
					text
				});
			}
			return result;
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x00102018 File Offset: 0x00100218
		internal string GetProcessBlock()
		{
			return "\r\n    try {\r\n        $steppablePipeline.Process($_)\r\n    } catch {\r\n        throw\r\n    }\r\n";
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x00102020 File Offset: 0x00100220
		internal string GetDynamicParamBlock()
		{
			return string.Format(CultureInfo.InvariantCulture, "\r\n    try {{\r\n        $targetCmd = $ExecutionContext.InvokeCommand.GetCommand('{0}', [System.Management.Automation.CommandTypes]::{1}, $PSBoundParameters)\r\n        $dynamicParams = @($targetCmd.Parameters.GetEnumerator() | Microsoft.PowerShell.Core\\Where-Object {{ $_.Value.IsDynamic }})\r\n        if ($dynamicParams.Length -gt 0)\r\n        {{\r\n            $paramDictionary = [Management.Automation.RuntimeDefinedParameterDictionary]::new()\r\n            foreach ($param in $dynamicParams)\r\n            {{\r\n                $param = $param.Value\r\n\r\n                if(-not $MyInvocation.MyCommand.Parameters.ContainsKey($param.Name))\r\n                {{\r\n                    $dynParam = [Management.Automation.RuntimeDefinedParameter]::new($param.Name, $param.ParameterType, $param.Attributes)\r\n                    $paramDictionary.Add($param.Name, $dynParam)\r\n                }}\r\n            }}\r\n            return $paramDictionary\r\n        }}\r\n    }} catch {{\r\n        throw\r\n    }}\r\n", new object[]
			{
				CodeGeneration.EscapeSingleQuotedStringContent(this._wrappedCommand),
				this._wrappedCommandType
			});
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x00102060 File Offset: 0x00100260
		internal string GetEndBlock()
		{
			return "\r\n    try {\r\n        $steppablePipeline.End()\r\n    } catch {\r\n        throw\r\n    }\r\n";
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x00102068 File Offset: 0x00100268
		private static CommandMetadata GetRestrictedCmdlet(string cmdletName, string defaultParameterSet, string helpUri, params ParameterMetadata[] parameters)
		{
			Dictionary<string, ParameterMetadata> dictionary = new Dictionary<string, ParameterMetadata>(StringComparer.OrdinalIgnoreCase);
			foreach (ParameterMetadata parameterMetadata in parameters)
			{
				dictionary.Add(parameterMetadata.Name, parameterMetadata);
			}
			return new CommandMetadata(cmdletName, CommandTypes.Cmdlet, true, defaultParameterSet, false, ConfirmImpact.None, false, false, true, dictionary)
			{
				HelpUri = helpUri
			};
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x001020C0 File Offset: 0x001002C0
		private static CommandMetadata GetRestrictedGetCommand()
		{
			ParameterMetadata parameterMetadata = new ParameterMetadata("Name", typeof(string[]));
			parameterMetadata.Attributes.Add(new ValidateLengthAttribute(0, 1000));
			parameterMetadata.Attributes.Add(new ValidateCountAttribute(0, 1000));
			ParameterMetadata parameterMetadata2 = new ParameterMetadata("Module", typeof(string[]));
			parameterMetadata2.Attributes.Add(new ValidateLengthAttribute(0, 1000));
			parameterMetadata2.Attributes.Add(new ValidateCountAttribute(0, 100));
			ParameterMetadata parameterMetadata3 = new ParameterMetadata("ArgumentList", typeof(object[]));
			parameterMetadata3.Attributes.Add(new ValidateCountAttribute(0, 100));
			ParameterMetadata parameterMetadata4 = new ParameterMetadata("CommandType", typeof(CommandTypes));
			ParameterMetadata parameterMetadata5 = new ParameterMetadata("ListImported", typeof(SwitchParameter));
			ParameterMetadata parameterMetadata6 = new ParameterMetadata("ShowCommandInfo", typeof(SwitchParameter));
			return CommandMetadata.GetRestrictedCmdlet("Get-Command", null, "http://go.microsoft.com/fwlink/?LinkID=113309", new ParameterMetadata[]
			{
				parameterMetadata,
				parameterMetadata2,
				parameterMetadata3,
				parameterMetadata4,
				parameterMetadata5,
				parameterMetadata6
			});
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x001021F0 File Offset: 0x001003F0
		private static CommandMetadata GetRestrictedGetFormatData()
		{
			return CommandMetadata.GetRestrictedCmdlet("Get-FormatData", null, "http://go.microsoft.com/fwlink/?LinkID=144303", new ParameterMetadata[]
			{
				new ParameterMetadata("TypeName", typeof(string[]))
				{
					Attributes = 
					{
						new ValidateLengthAttribute(0, 1000),
						new ValidateCountAttribute(0, 1000)
					}
				}
			});
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x0010225C File Offset: 0x0010045C
		private static CommandMetadata GetRestrictedGetHelp()
		{
			return CommandMetadata.GetRestrictedCmdlet("Get-Help", null, "http://go.microsoft.com/fwlink/?LinkID=113316", new ParameterMetadata[]
			{
				new ParameterMetadata("Name", typeof(string))
				{
					Attributes = 
					{
						new ValidatePatternAttribute("^[-._:\\\\\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Nd}\\p{Lm}]{1,100}$"),
						new ValidateLengthAttribute(0, 1000)
					}
				},
				new ParameterMetadata("Category", typeof(string[]))
				{
					Attributes = 
					{
						new ValidateSetAttribute(Enum.GetNames(typeof(HelpCategory))),
						new ValidateCountAttribute(0, 1)
					}
				}
			});
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x00102310 File Offset: 0x00100510
		private static CommandMetadata GetRestrictedSelectObject()
		{
			string[] array = new string[]
			{
				"ModuleName",
				"Namespace",
				"OutputType",
				"Count",
				"HelpUri",
				"Name",
				"CommandType",
				"ResolvedCommandName",
				"DefaultParameterSet",
				"CmdletBinding",
				"Parameters"
			};
			return CommandMetadata.GetRestrictedCmdlet("Select-Object", null, "http://go.microsoft.com/fwlink/?LinkID=113387", new ParameterMetadata[]
			{
				new ParameterMetadata("Property", typeof(string[]))
				{
					Attributes = 
					{
						new ValidateSetAttribute(array),
						new ValidateCountAttribute(1, array.Length)
					}
				},
				new ParameterMetadata("InputObject", typeof(object))
				{
					ParameterSets = 
					{
						{
							"__AllParameterSets",
							new ParameterSetMetadata(int.MinValue, ParameterSetMetadata.ParameterFlags.Mandatory | ParameterSetMetadata.ParameterFlags.ValueFromPipeline, null)
						}
					}
				}
			});
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x00102410 File Offset: 0x00100610
		private static CommandMetadata GetRestrictedMeasureObject()
		{
			return CommandMetadata.GetRestrictedCmdlet("Measure-Object", null, "http://go.microsoft.com/fwlink/?LinkID=113349", new ParameterMetadata[]
			{
				new ParameterMetadata("InputObject", typeof(object))
				{
					ParameterSets = 
					{
						{
							"__AllParameterSets",
							new ParameterSetMetadata(int.MinValue, ParameterSetMetadata.ParameterFlags.Mandatory | ParameterSetMetadata.ParameterFlags.ValueFromPipeline, null)
						}
					}
				}
			});
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x0010246C File Offset: 0x0010066C
		private static CommandMetadata GetRestrictedOutDefault()
		{
			return CommandMetadata.GetRestrictedCmdlet("Out-Default", null, "http://go.microsoft.com/fwlink/?LinkID=113362", new ParameterMetadata[]
			{
				new ParameterMetadata("InputObject", typeof(object))
				{
					ParameterSets = 
					{
						{
							"__AllParameterSets",
							new ParameterSetMetadata(int.MinValue, ParameterSetMetadata.ParameterFlags.Mandatory | ParameterSetMetadata.ParameterFlags.ValueFromPipeline, null)
						}
					}
				}
			});
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x001024C6 File Offset: 0x001006C6
		private static CommandMetadata GetRestrictedExitPSSession()
		{
			return CommandMetadata.GetRestrictedCmdlet("Exit-PSSession", null, "http://go.microsoft.com/fwlink/?LinkID=135210", new ParameterMetadata[0]);
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x001024E0 File Offset: 0x001006E0
		public static Dictionary<string, CommandMetadata> GetRestrictedCommands(SessionCapabilities sessionCapabilities)
		{
			List<CommandMetadata> list = new List<CommandMetadata>();
			if (SessionCapabilities.RemoteServer == (sessionCapabilities & SessionCapabilities.RemoteServer))
			{
				list.AddRange(CommandMetadata.GetRestrictedRemotingCommands());
			}
			if (SessionCapabilities.WorkflowServer == (sessionCapabilities & SessionCapabilities.WorkflowServer))
			{
				list.AddRange(CommandMetadata.GetRestrictedRemotingCommands());
				list.AddRange(CommandMetadata.GetRestrictedJobCommands());
			}
			Dictionary<string, CommandMetadata> dictionary = new Dictionary<string, CommandMetadata>(StringComparer.OrdinalIgnoreCase);
			foreach (CommandMetadata commandMetadata in list)
			{
				dictionary.Add(commandMetadata.Name, commandMetadata);
			}
			return dictionary;
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x00102574 File Offset: 0x00100774
		private static Collection<CommandMetadata> GetRestrictedRemotingCommands()
		{
			return new Collection<CommandMetadata>
			{
				CommandMetadata.GetRestrictedGetCommand(),
				CommandMetadata.GetRestrictedGetFormatData(),
				CommandMetadata.GetRestrictedSelectObject(),
				CommandMetadata.GetRestrictedGetHelp(),
				CommandMetadata.GetRestrictedMeasureObject(),
				CommandMetadata.GetRestrictedExitPSSession(),
				CommandMetadata.GetRestrictedOutDefault()
			};
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x001025D8 File Offset: 0x001007D8
		private static Collection<CommandMetadata> GetRestrictedJobCommands()
		{
			ParameterSetMetadata value = new ParameterSetMetadata(0, ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName, string.Empty);
			ParameterSetMetadata value2 = new ParameterSetMetadata(0, ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName, string.Empty);
			ParameterSetMetadata value3 = new ParameterSetMetadata(0, ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName, string.Empty);
			ParameterSetMetadata value4 = new ParameterSetMetadata(int.MinValue, ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName, string.Empty);
			ParameterSetMetadata value5 = new ParameterSetMetadata(int.MinValue, ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName, string.Empty);
			ParameterSetMetadata value6 = new ParameterSetMetadata(0, ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName, string.Empty);
			ParameterSetMetadata value7 = new ParameterSetMetadata(0, ParameterSetMetadata.ParameterFlags.Mandatory | ParameterSetMetadata.ParameterFlags.ValueFromPipeline | ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName, string.Empty);
			ParameterSetMetadata value8 = new ParameterSetMetadata(0, ParameterSetMetadata.ParameterFlags.Mandatory | ParameterSetMetadata.ParameterFlags.ValueFromPipeline | ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName, string.Empty);
			ParameterSetMetadata value9 = new ParameterSetMetadata(0, ParameterSetMetadata.ParameterFlags.Mandatory | ParameterSetMetadata.ParameterFlags.ValueFromPipeline | ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName, string.Empty);
			Dictionary<string, ParameterSetMetadata> dictionary = new Dictionary<string, ParameterSetMetadata>();
			dictionary.Add("NameParameterSet", value);
			Collection<string> aliases = new Collection<string>();
			ParameterMetadata parameterMetadata = new ParameterMetadata(aliases, false, "Name", dictionary, typeof(string[]));
			parameterMetadata.Attributes.Add(new ValidatePatternAttribute("^[-._:\\\\\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Nd}\\p{Lm}]{1,100}$"));
			parameterMetadata.Attributes.Add(new ValidateLengthAttribute(0, 1000));
			ParameterMetadata parameterMetadata2 = new ParameterMetadata(aliases, false, "InstanceId", new Dictionary<string, ParameterSetMetadata>
			{
				{
					"InstanceIdParameterSet",
					value2
				}
			}, typeof(Guid[]));
			parameterMetadata2.Attributes.Add(new ValidateNotNullOrEmptyAttribute());
			ParameterMetadata parameterMetadata3 = new ParameterMetadata(aliases, false, "Id", new Dictionary<string, ParameterSetMetadata>
			{
				{
					"SessionIdParameterSet",
					value3
				}
			}, typeof(int[]));
			parameterMetadata3.Attributes.Add(new ValidateNotNullOrEmptyAttribute());
			ParameterMetadata parameterMetadata4 = new ParameterMetadata(aliases, false, "State", new Dictionary<string, ParameterSetMetadata>
			{
				{
					"StateParameterSet",
					value4
				}
			}, typeof(JobState));
			ParameterMetadata parameterMetadata5 = new ParameterMetadata(aliases, false, "Command", new Dictionary<string, ParameterSetMetadata>
			{
				{
					"CommandParameterSet",
					value5
				}
			}, typeof(string[]));
			ParameterMetadata parameterMetadata6 = new ParameterMetadata(aliases, false, "Filter", new Dictionary<string, ParameterSetMetadata>
			{
				{
					"FilterParameterSet",
					value6
				}
			}, typeof(Hashtable));
			ParameterMetadata parameterMetadata7 = new ParameterMetadata(aliases, false, "Job", new Dictionary<string, ParameterSetMetadata>
			{
				{
					"Job",
					value7
				}
			}, typeof(Job[]));
			parameterMetadata7.Attributes.Add(new ValidateNotNullOrEmptyAttribute());
			ParameterMetadata parameterMetadata8 = new ParameterMetadata(aliases, false, "Job", new Dictionary<string, ParameterSetMetadata>
			{
				{
					"ComputerName",
					value8
				},
				{
					"Location",
					value9
				}
			}, typeof(Job[]));
			Collection<CommandMetadata> collection = new Collection<CommandMetadata>();
			ParameterMetadata parameterMetadata9 = new ParameterMetadata("PassThru", typeof(SwitchParameter));
			ParameterMetadata parameterMetadata10 = new ParameterMetadata("Any", typeof(SwitchParameter));
			CommandMetadata restrictedCmdlet = CommandMetadata.GetRestrictedCmdlet("Stop-Job", "SessionIdParameterSet", "http://go.microsoft.com/fwlink/?LinkID=113413", new ParameterMetadata[]
			{
				parameterMetadata,
				parameterMetadata2,
				parameterMetadata3,
				parameterMetadata4,
				parameterMetadata6,
				parameterMetadata7,
				parameterMetadata9
			});
			collection.Add(restrictedCmdlet);
			CommandMetadata restrictedCmdlet2 = CommandMetadata.GetRestrictedCmdlet("Wait-Job", "SessionIdParameterSet", "http://go.microsoft.com/fwlink/?LinkID=113422", new ParameterMetadata[]
			{
				parameterMetadata,
				parameterMetadata2,
				parameterMetadata3,
				parameterMetadata7,
				parameterMetadata4,
				parameterMetadata6,
				parameterMetadata10,
				new ParameterMetadata("Timeout", typeof(int))
				{
					Attributes = 
					{
						new ValidateRangeAttribute(-1, int.MaxValue)
					}
				}
			});
			collection.Add(restrictedCmdlet2);
			CommandMetadata restrictedCmdlet3 = CommandMetadata.GetRestrictedCmdlet("Get-Job", "SessionIdParameterSet", "http://go.microsoft.com/fwlink/?LinkID=113328", new ParameterMetadata[]
			{
				parameterMetadata,
				parameterMetadata2,
				parameterMetadata3,
				parameterMetadata4,
				parameterMetadata6,
				parameterMetadata5
			});
			collection.Add(restrictedCmdlet3);
			dictionary = new Dictionary<string, ParameterSetMetadata>();
			value8 = new ParameterSetMetadata(1, ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName, string.Empty);
			dictionary.Add("ComputerName", value8);
			ParameterMetadata parameterMetadata11 = new ParameterMetadata(aliases, false, "ComputerName", dictionary, typeof(string[]));
			parameterMetadata11.Attributes.Add(new ValidateLengthAttribute(0, 1000));
			parameterMetadata11.Attributes.Add(new ValidateNotNullOrEmptyAttribute());
			dictionary = new Dictionary<string, ParameterSetMetadata>();
			value9 = new ParameterSetMetadata(1, ParameterSetMetadata.ParameterFlags.ValueFromPipelineByPropertyName, string.Empty);
			dictionary.Add("Location", value9);
			ParameterMetadata parameterMetadata12 = new ParameterMetadata(aliases, false, "Location", dictionary, typeof(string[]));
			parameterMetadata12.Attributes.Add(new ValidateLengthAttribute(0, 1000));
			parameterMetadata12.Attributes.Add(new ValidateNotNullOrEmptyAttribute());
			ParameterMetadata parameterMetadata13 = new ParameterMetadata("NoRecurse", typeof(SwitchParameter));
			ParameterMetadata parameterMetadata14 = new ParameterMetadata("Keep", typeof(SwitchParameter));
			ParameterMetadata parameterMetadata15 = new ParameterMetadata("Wait", typeof(SwitchParameter));
			ParameterMetadata parameterMetadata16 = new ParameterMetadata("WriteEvents", typeof(SwitchParameter));
			ParameterMetadata parameterMetadata17 = new ParameterMetadata("WriteJobInResults", typeof(SwitchParameter));
			ParameterMetadata parameterMetadata18 = new ParameterMetadata("AutoRemoveJob", typeof(SwitchParameter));
			CommandMetadata restrictedCmdlet4 = CommandMetadata.GetRestrictedCmdlet("Receive-Job", "Location", "http://go.microsoft.com/fwlink/?LinkID=113372", new ParameterMetadata[]
			{
				parameterMetadata,
				parameterMetadata2,
				parameterMetadata3,
				parameterMetadata4,
				parameterMetadata8,
				parameterMetadata11,
				parameterMetadata12,
				parameterMetadata13,
				parameterMetadata14,
				parameterMetadata15,
				parameterMetadata16,
				parameterMetadata17,
				parameterMetadata18
			});
			collection.Add(restrictedCmdlet4);
			ParameterMetadata parameterMetadata19 = new ParameterMetadata("Force", typeof(SwitchParameter));
			CommandMetadata restrictedCmdlet5 = CommandMetadata.GetRestrictedCmdlet("Remove-Job", "SessionIdParameterSet", "http://go.microsoft.com/fwlink/?LinkID=113377", new ParameterMetadata[]
			{
				parameterMetadata,
				parameterMetadata2,
				parameterMetadata3,
				parameterMetadata4,
				parameterMetadata6,
				parameterMetadata7,
				parameterMetadata19
			});
			collection.Add(restrictedCmdlet5);
			CommandMetadata restrictedCmdlet6 = CommandMetadata.GetRestrictedCmdlet("Suspend-Job", "SessionIdParameterSet", "http://go.microsoft.com/fwlink/?LinkID=210613", new ParameterMetadata[]
			{
				parameterMetadata,
				parameterMetadata2,
				parameterMetadata3,
				parameterMetadata4,
				parameterMetadata6,
				parameterMetadata7,
				parameterMetadata9
			});
			collection.Add(restrictedCmdlet6);
			CommandMetadata restrictedCmdlet7 = CommandMetadata.GetRestrictedCmdlet("Resume-Job", "SessionIdParameterSet", "http://go.microsoft.com/fwlink/?LinkID=210611", new ParameterMetadata[]
			{
				parameterMetadata,
				parameterMetadata2,
				parameterMetadata3,
				parameterMetadata4,
				parameterMetadata6,
				parameterMetadata7,
				parameterMetadata9
			});
			collection.Add(restrictedCmdlet7);
			return collection;
		}

		// Token: 0x040018B5 RID: 6325
		internal const string isSafeNameOrIdentifierRegex = "^[-._:\\\\\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Nd}\\p{Lm}]{1,100}$";

		// Token: 0x040018B6 RID: 6326
		private string _commandName = string.Empty;

		// Token: 0x040018B7 RID: 6327
		private ScriptBlock _scriptBlock;

		// Token: 0x040018B8 RID: 6328
		private string _defaultParameterSetName = "__AllParameterSets";

		// Token: 0x040018B9 RID: 6329
		private bool _supportsShouldProcess;

		// Token: 0x040018BA RID: 6330
		private bool _supportsPaging;

		// Token: 0x040018BB RID: 6331
		private bool _positionalBinding = true;

		// Token: 0x040018BC RID: 6332
		private bool _supportsTransactions;

		// Token: 0x040018BD RID: 6333
		private string _helpUri = string.Empty;

		// Token: 0x040018BE RID: 6334
		private RemotingCapability _remotingCapability = RemotingCapability.PowerShell;

		// Token: 0x040018BF RID: 6335
		private ConfirmImpact _confirmImpact = ConfirmImpact.Medium;

		// Token: 0x040018C0 RID: 6336
		private Dictionary<string, ParameterMetadata> _parameters;

		// Token: 0x040018C1 RID: 6337
		private bool _shouldGenerateCommonParameters;

		// Token: 0x040018C2 RID: 6338
		private ObsoleteAttribute _obsolete;

		// Token: 0x040018C3 RID: 6339
		private readonly MergedCommandParameterMetadata staticCommandParameterMetadata;

		// Token: 0x040018C4 RID: 6340
		private bool _implementsDynamicParameters;

		// Token: 0x040018C5 RID: 6341
		private uint _defaultParameterSetFlag;

		// Token: 0x040018C6 RID: 6342
		private readonly Collection<Attribute> _otherAttributes = new Collection<Attribute>();

		// Token: 0x040018C7 RID: 6343
		private string _wrappedCommand;

		// Token: 0x040018C8 RID: 6344
		private CommandTypes _wrappedCommandType;

		// Token: 0x040018C9 RID: 6345
		private bool _wrappedAnyCmdlet;

		// Token: 0x040018CA RID: 6346
		private static ConcurrentDictionary<string, CommandMetadata> CommandMetadataCache = new ConcurrentDictionary<string, CommandMetadata>(StringComparer.OrdinalIgnoreCase);
	}
}
