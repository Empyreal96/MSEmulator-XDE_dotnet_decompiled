using System;
using System.Collections;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000351 RID: 849
	[Cmdlet("New", "PSRoleCapabilityFile", HelpUri = "http://go.microsoft.com/fwlink/?LinkId=623708")]
	public class NewPSRoleCapabilityFileCommand : PSCmdlet
	{
		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06002A30 RID: 10800 RVA: 0x000E9663 File Offset: 0x000E7863
		// (set) Token: 0x06002A31 RID: 10801 RVA: 0x000E966B File Offset: 0x000E786B
		[Parameter(Position = 0, Mandatory = true)]
		[ValidateNotNullOrEmpty]
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.path = value;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06002A32 RID: 10802 RVA: 0x000E9674 File Offset: 0x000E7874
		// (set) Token: 0x06002A33 RID: 10803 RVA: 0x000E967C File Offset: 0x000E787C
		[Parameter]
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
			set
			{
				this.guid = value;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06002A34 RID: 10804 RVA: 0x000E9685 File Offset: 0x000E7885
		// (set) Token: 0x06002A35 RID: 10805 RVA: 0x000E968D File Offset: 0x000E788D
		[Parameter]
		public string Author
		{
			get
			{
				return this.author;
			}
			set
			{
				this.author = value;
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06002A36 RID: 10806 RVA: 0x000E9696 File Offset: 0x000E7896
		// (set) Token: 0x06002A37 RID: 10807 RVA: 0x000E969E File Offset: 0x000E789E
		[Parameter]
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

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06002A38 RID: 10808 RVA: 0x000E96A7 File Offset: 0x000E78A7
		// (set) Token: 0x06002A39 RID: 10809 RVA: 0x000E96AF File Offset: 0x000E78AF
		[Parameter]
		public string CompanyName
		{
			get
			{
				return this.companyName;
			}
			set
			{
				this.companyName = value;
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06002A3A RID: 10810 RVA: 0x000E96B8 File Offset: 0x000E78B8
		// (set) Token: 0x06002A3B RID: 10811 RVA: 0x000E96C0 File Offset: 0x000E78C0
		[Parameter]
		public string Copyright
		{
			get
			{
				return this.copyright;
			}
			set
			{
				this.copyright = value;
			}
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06002A3C RID: 10812 RVA: 0x000E96C9 File Offset: 0x000E78C9
		// (set) Token: 0x06002A3D RID: 10813 RVA: 0x000E96D1 File Offset: 0x000E78D1
		[Parameter]
		public object[] ModulesToImport
		{
			get
			{
				return this.modulesToImport;
			}
			set
			{
				this.modulesToImport = value;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06002A3E RID: 10814 RVA: 0x000E96DA File Offset: 0x000E78DA
		// (set) Token: 0x06002A3F RID: 10815 RVA: 0x000E96E2 File Offset: 0x000E78E2
		[Parameter]
		public string[] VisibleAliases
		{
			get
			{
				return this.visibleAliases;
			}
			set
			{
				this.visibleAliases = value;
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06002A40 RID: 10816 RVA: 0x000E96EB File Offset: 0x000E78EB
		// (set) Token: 0x06002A41 RID: 10817 RVA: 0x000E96F3 File Offset: 0x000E78F3
		[Parameter]
		public object[] VisibleCmdlets
		{
			get
			{
				return this.visibleCmdlets;
			}
			set
			{
				this.visibleCmdlets = value;
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06002A42 RID: 10818 RVA: 0x000E96FC File Offset: 0x000E78FC
		// (set) Token: 0x06002A43 RID: 10819 RVA: 0x000E9704 File Offset: 0x000E7904
		[Parameter]
		public object[] VisibleFunctions
		{
			get
			{
				return this.visibleFunctions;
			}
			set
			{
				this.visibleFunctions = value;
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06002A44 RID: 10820 RVA: 0x000E970D File Offset: 0x000E790D
		// (set) Token: 0x06002A45 RID: 10821 RVA: 0x000E9715 File Offset: 0x000E7915
		[Parameter]
		public string[] VisibleExternalCommands
		{
			get
			{
				return this.visibleExternalCommands;
			}
			set
			{
				this.visibleExternalCommands = value;
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06002A46 RID: 10822 RVA: 0x000E971E File Offset: 0x000E791E
		// (set) Token: 0x06002A47 RID: 10823 RVA: 0x000E9726 File Offset: 0x000E7926
		[Parameter]
		public string[] VisibleProviders
		{
			get
			{
				return this.visibleProviders;
			}
			set
			{
				this.visibleProviders = value;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x000E972F File Offset: 0x000E792F
		// (set) Token: 0x06002A49 RID: 10825 RVA: 0x000E9737 File Offset: 0x000E7937
		[Parameter]
		public string[] ScriptsToProcess
		{
			get
			{
				return this.scriptsToProcess;
			}
			set
			{
				this.scriptsToProcess = value;
			}
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x000E9740 File Offset: 0x000E7940
		// (set) Token: 0x06002A4B RID: 10827 RVA: 0x000E9748 File Offset: 0x000E7948
		[Parameter]
		public IDictionary[] AliasDefinitions
		{
			get
			{
				return this.aliasDefinitions;
			}
			set
			{
				this.aliasDefinitions = value;
			}
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06002A4C RID: 10828 RVA: 0x000E9751 File Offset: 0x000E7951
		// (set) Token: 0x06002A4D RID: 10829 RVA: 0x000E9759 File Offset: 0x000E7959
		[Parameter]
		public IDictionary[] FunctionDefinitions
		{
			get
			{
				return this.functionDefinitions;
			}
			set
			{
				this.functionDefinitions = value;
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06002A4E RID: 10830 RVA: 0x000E9762 File Offset: 0x000E7962
		// (set) Token: 0x06002A4F RID: 10831 RVA: 0x000E976A File Offset: 0x000E796A
		[Parameter]
		public object VariableDefinitions
		{
			get
			{
				return this.variableDefinitions;
			}
			set
			{
				this.variableDefinitions = value;
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06002A50 RID: 10832 RVA: 0x000E9773 File Offset: 0x000E7973
		// (set) Token: 0x06002A51 RID: 10833 RVA: 0x000E977B File Offset: 0x000E797B
		[Parameter]
		public IDictionary EnvironmentVariables
		{
			get
			{
				return this.environmentVariables;
			}
			set
			{
				this.environmentVariables = value;
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06002A52 RID: 10834 RVA: 0x000E9784 File Offset: 0x000E7984
		// (set) Token: 0x06002A53 RID: 10835 RVA: 0x000E978C File Offset: 0x000E798C
		[Parameter]
		public string[] TypesToProcess
		{
			get
			{
				return this.typesToProcess;
			}
			set
			{
				this.typesToProcess = value;
			}
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06002A54 RID: 10836 RVA: 0x000E9795 File Offset: 0x000E7995
		// (set) Token: 0x06002A55 RID: 10837 RVA: 0x000E979D File Offset: 0x000E799D
		[Parameter]
		public string[] FormatsToProcess
		{
			get
			{
				return this.formatsToProcess;
			}
			set
			{
				this.formatsToProcess = value;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06002A56 RID: 10838 RVA: 0x000E97A6 File Offset: 0x000E79A6
		// (set) Token: 0x06002A57 RID: 10839 RVA: 0x000E97AE File Offset: 0x000E79AE
		[Parameter]
		public string[] AssembliesToLoad
		{
			get
			{
				return this.assembliesToLoad;
			}
			set
			{
				this.assembliesToLoad = value;
			}
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000E97B8 File Offset: 0x000E79B8
		protected override void ProcessRecord()
		{
			ProviderInfo providerInfo = null;
			PSDriveInfo psdriveInfo;
			string unresolvedProviderPathFromPSPath = base.SessionState.Path.GetUnresolvedProviderPathFromPSPath(this.path, out providerInfo, out psdriveInfo);
			if (!providerInfo.NameEquals(base.Context.ProviderNames.FileSystem) || !unresolvedProviderPathFromPSPath.EndsWith(".psrc", StringComparison.OrdinalIgnoreCase))
			{
				string message = StringUtil.Format(RemotingErrorIdStrings.InvalidRoleCapabilityFilePath, this.path);
				InvalidOperationException exception = new InvalidOperationException(message);
				ErrorRecord errorRecord = new ErrorRecord(exception, "InvalidRoleCapabilityFilePath", ErrorCategory.InvalidArgument, this.path);
				base.ThrowTerminatingError(errorRecord);
			}
			FileStream fileStream;
			StreamWriter streamWriter;
			FileInfo fileInfo;
			PathUtils.MasterStreamOpen(this, unresolvedProviderPathFromPSPath, "unicode", false, false, false, false, out fileStream, out streamWriter, out fileInfo, false);
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("@{");
				stringBuilder.Append(streamWriter.NewLine);
				stringBuilder.Append(streamWriter.NewLine);
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.Guid, RemotingErrorIdStrings.DISCGUIDComment, SessionConfigurationUtils.QuoteName(this.guid), streamWriter, false));
				if (string.IsNullOrEmpty(this.author))
				{
					this.author = Environment.UserName;
				}
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.Author, RemotingErrorIdStrings.DISCAuthorComment, SessionConfigurationUtils.QuoteName(this.author), streamWriter, false));
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.Description, RemotingErrorIdStrings.DISCDescriptionComment, SessionConfigurationUtils.QuoteName(this.description), streamWriter, string.IsNullOrEmpty(this.description)));
				if (string.IsNullOrEmpty(this.companyName))
				{
					this.companyName = Modules.DefaultCompanyName;
				}
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.CompanyName, RemotingErrorIdStrings.DISCCompanyNameComment, SessionConfigurationUtils.QuoteName(this.companyName), streamWriter, false));
				if (string.IsNullOrEmpty(this.copyright))
				{
					this.copyright = StringUtil.Format(Modules.DefaultCopyrightMessage, DateTime.Now.Year, this.author);
				}
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.Copyright, RemotingErrorIdStrings.DISCCopyrightComment, SessionConfigurationUtils.QuoteName(this.copyright), streamWriter, false));
				if (this.modulesToImport == null)
				{
					string value = "'MyCustomModule', @{ ModuleName = 'MyCustomModule'; ModuleVersion = '1.0.0.0'; GUID = '4d30d5f0-cb16-4898-812d-f20a6c596bdf' }";
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.ModulesToImport, RemotingErrorIdStrings.DISCModulesToImportComment, value, streamWriter, true));
				}
				else
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.ModulesToImport, RemotingErrorIdStrings.DISCModulesToImportComment, SessionConfigurationUtils.CombineHashTableOrStringArray(this.modulesToImport, streamWriter, this), streamWriter, false));
				}
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleAliases, RemotingErrorIdStrings.DISCVisibleAliasesComment, SessionConfigurationUtils.GetVisibilityDefault(this.visibleAliases, streamWriter, this), streamWriter, this.visibleAliases.Length == 0));
				if (this.visibleCmdlets == null || this.visibleCmdlets.Length == 0)
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleCmdlets, RemotingErrorIdStrings.DISCVisibleCmdletsComment, "'Invoke-Cmdlet1', @{ Name = 'Invoke-Cmdlet2'; Parameters = @{ Name = 'Parameter1'; ValidateSet = 'Item1', 'Item2' }, @{ Name = 'Parameter2'; ValidatePattern = 'L*' } }", streamWriter, true));
				}
				else
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleCmdlets, RemotingErrorIdStrings.DISCVisibleCmdletsComment, SessionConfigurationUtils.GetVisibilityDefault(this.visibleCmdlets, streamWriter, this), streamWriter, false));
				}
				if (this.visibleFunctions == null || this.visibleFunctions.Length == 0)
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleFunctions, RemotingErrorIdStrings.DISCVisibleFunctionsComment, "'Invoke-Function1', @{ Name = 'Invoke-Function2'; Parameters = @{ Name = 'Parameter1'; ValidateSet = 'Item1', 'Item2' }, @{ Name = 'Parameter2'; ValidatePattern = 'L*' } }", streamWriter, true));
				}
				else
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleFunctions, RemotingErrorIdStrings.DISCVisibleFunctionsComment, SessionConfigurationUtils.GetVisibilityDefault(this.visibleFunctions, streamWriter, this), streamWriter, this.visibleFunctions.Length == 0));
				}
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleExternalCommands, RemotingErrorIdStrings.DISCVisibleExternalCommandsComment, SessionConfigurationUtils.GetVisibilityDefault(this.visibleExternalCommands, streamWriter, this), streamWriter, this.visibleExternalCommands.Length == 0));
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VisibleProviders, RemotingErrorIdStrings.DISCVisibleProvidersComment, SessionConfigurationUtils.GetVisibilityDefault(this.visibleProviders, streamWriter, this), streamWriter, this.visibleProviders.Length == 0));
				string value2 = (this.scriptsToProcess.Length > 0) ? SessionConfigurationUtils.CombineStringArray(this.scriptsToProcess) : "'C:\\ConfigData\\InitScript1.ps1', 'C:\\ConfigData\\InitScript2.ps1'";
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.ScriptsToProcess, RemotingErrorIdStrings.DISCScriptsToProcessComment, value2, streamWriter, this.scriptsToProcess.Length == 0));
				if (this.aliasDefinitions == null || this.aliasDefinitions.Length == 0)
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.AliasDefinitions, RemotingErrorIdStrings.DISCAliasDefinitionsComment, "@{ Name = 'Alias1'; Value = 'Invoke-Alias1'}, @{ Name = 'Alias2'; Value = 'Invoke-Alias2'}", streamWriter, true));
				}
				else
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.AliasDefinitions, RemotingErrorIdStrings.DISCAliasDefinitionsComment, SessionConfigurationUtils.CombineHashtableArray(this.aliasDefinitions, streamWriter, new int?(0)), streamWriter, false));
				}
				if (this.functionDefinitions == null)
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.FunctionDefinitions, RemotingErrorIdStrings.DISCFunctionDefinitionsComment, "@{ Name = 'MyFunction'; ScriptBlock = { param($MyInput) $MyInput } }", streamWriter, true));
				}
				else
				{
					Hashtable[] array = DISCPowerShellConfiguration.TryGetHashtableArray(this.functionDefinitions);
					if (array != null)
					{
						stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.FunctionDefinitions, RemotingErrorIdStrings.DISCFunctionDefinitionsComment, SessionConfigurationUtils.CombineHashtableArray(array, streamWriter, new int?(0)), streamWriter, false));
						foreach (Hashtable hashtable in array)
						{
							if (!hashtable.ContainsKey(ConfigFileConstants.FunctionNameToken))
							{
								PSArgumentException ex = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
								{
									ConfigFileConstants.FunctionDefinitions,
									ConfigFileConstants.FunctionNameToken,
									this.path
								}));
								base.ThrowTerminatingError(ex.ErrorRecord);
							}
							if (!hashtable.ContainsKey(ConfigFileConstants.FunctionValueToken))
							{
								PSArgumentException ex2 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
								{
									ConfigFileConstants.FunctionDefinitions,
									ConfigFileConstants.FunctionValueToken,
									this.path
								}));
								base.ThrowTerminatingError(ex2.ErrorRecord);
							}
							if (!(hashtable[ConfigFileConstants.FunctionValueToken] is ScriptBlock))
							{
								PSArgumentException ex3 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCKeyMustBeScriptBlock, new object[]
								{
									ConfigFileConstants.FunctionValueToken,
									ConfigFileConstants.FunctionDefinitions,
									this.path
								}));
								base.ThrowTerminatingError(ex3.ErrorRecord);
							}
							foreach (object obj in hashtable.Keys)
							{
								string text = (string)obj;
								if (!string.Equals(text, ConfigFileConstants.FunctionNameToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text, ConfigFileConstants.FunctionValueToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text, ConfigFileConstants.FunctionOptionsToken, StringComparison.OrdinalIgnoreCase))
								{
									PSArgumentException ex4 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeContainsInvalidKey, new object[]
									{
										text,
										ConfigFileConstants.FunctionDefinitions,
										this.path
									}));
									base.ThrowTerminatingError(ex4.ErrorRecord);
								}
							}
						}
					}
					else
					{
						PSArgumentException ex5 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeHashtableArray, ConfigFileConstants.FunctionDefinitions, unresolvedProviderPathFromPSPath));
						base.ThrowTerminatingError(ex5.ErrorRecord);
					}
				}
				if (this.variableDefinitions == null)
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VariableDefinitions, RemotingErrorIdStrings.DISCVariableDefinitionsComment, "@{ Name = 'Variable1'; Value = { 'Dynamic' + 'InitialValue' } }, @{ Name = 'Variable2'; Value = 'StaticInitialValue' }", streamWriter, true));
				}
				else
				{
					string text2 = this.variableDefinitions as string;
					if (text2 != null)
					{
						stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VariableDefinitions, RemotingErrorIdStrings.DISCVariableDefinitionsComment, text2, streamWriter, false));
					}
					else
					{
						Hashtable[] array3 = DISCPowerShellConfiguration.TryGetHashtableArray(this.variableDefinitions);
						if (array3 != null)
						{
							stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.VariableDefinitions, RemotingErrorIdStrings.DISCVariableDefinitionsComment, SessionConfigurationUtils.CombineHashtableArray(array3, streamWriter, new int?(0)), streamWriter, false));
							foreach (Hashtable hashtable2 in array3)
							{
								if (!hashtable2.ContainsKey(ConfigFileConstants.VariableNameToken))
								{
									PSArgumentException ex6 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
									{
										ConfigFileConstants.VariableDefinitions,
										ConfigFileConstants.VariableNameToken,
										this.path
									}));
									base.ThrowTerminatingError(ex6.ErrorRecord);
								}
								if (!hashtable2.ContainsKey(ConfigFileConstants.VariableValueToken))
								{
									PSArgumentException ex7 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustContainKey, new object[]
									{
										ConfigFileConstants.VariableDefinitions,
										ConfigFileConstants.VariableValueToken,
										this.path
									}));
									base.ThrowTerminatingError(ex7.ErrorRecord);
								}
								foreach (object obj2 in hashtable2.Keys)
								{
									string text3 = (string)obj2;
									if (!string.Equals(text3, ConfigFileConstants.VariableNameToken, StringComparison.OrdinalIgnoreCase) && !string.Equals(text3, ConfigFileConstants.VariableValueToken, StringComparison.OrdinalIgnoreCase))
									{
										PSArgumentException ex8 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeContainsInvalidKey, new object[]
										{
											text3,
											ConfigFileConstants.VariableDefinitions,
											this.path
										}));
										base.ThrowTerminatingError(ex8.ErrorRecord);
									}
								}
							}
						}
						else
						{
							PSArgumentException ex9 = new PSArgumentException(StringUtil.Format(RemotingErrorIdStrings.DISCTypeMustBeHashtableArray, ConfigFileConstants.VariableDefinitions, unresolvedProviderPathFromPSPath));
							base.ThrowTerminatingError(ex9.ErrorRecord);
						}
					}
				}
				if (this.environmentVariables == null)
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.EnvironmentVariables, RemotingErrorIdStrings.DISCEnvironmentVariablesComment, "@{ Variable1 = 'Value1'; Variable2 = 'Value2' }", streamWriter, true));
				}
				else
				{
					stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.EnvironmentVariables, RemotingErrorIdStrings.DISCEnvironmentVariablesComment, SessionConfigurationUtils.CombineHashtable(this.environmentVariables, streamWriter, new int?(0)), streamWriter, false));
				}
				value2 = ((this.typesToProcess.Length > 0) ? SessionConfigurationUtils.CombineStringArray(this.typesToProcess) : "'C:\\ConfigData\\MyTypes.ps1xml', 'C:\\ConfigData\\OtherTypes.ps1xml'");
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.TypesToProcess, RemotingErrorIdStrings.DISCTypesToProcessComment, value2, streamWriter, this.typesToProcess.Length == 0));
				value2 = ((this.formatsToProcess.Length > 0) ? SessionConfigurationUtils.CombineStringArray(this.formatsToProcess) : "'C:\\ConfigData\\MyFormats.ps1xml', 'C:\\ConfigData\\OtherFormats.ps1xml'");
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.FormatsToProcess, RemotingErrorIdStrings.DISCFormatsToProcessComment, value2, streamWriter, this.formatsToProcess.Length == 0));
				bool isExample = false;
				if (this.assembliesToLoad == null || this.assembliesToLoad.Length == 0)
				{
					isExample = true;
					this.assembliesToLoad = new string[]
					{
						"System.Web",
						"System.OtherAssembly, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
					};
				}
				stringBuilder.Append(SessionConfigurationUtils.ConfigFragment(ConfigFileConstants.AssembliesToLoad, RemotingErrorIdStrings.DISCAssembliesToLoadComment, SessionConfigurationUtils.CombineStringArray(this.assembliesToLoad), streamWriter, isExample));
				stringBuilder.Append("}");
				streamWriter.Write(stringBuilder.ToString());
			}
			finally
			{
				streamWriter.Dispose();
			}
		}

		// Token: 0x040014DE RID: 5342
		private string path;

		// Token: 0x040014DF RID: 5343
		private Guid guid = Guid.NewGuid();

		// Token: 0x040014E0 RID: 5344
		private string author;

		// Token: 0x040014E1 RID: 5345
		private string description;

		// Token: 0x040014E2 RID: 5346
		private string companyName;

		// Token: 0x040014E3 RID: 5347
		private string copyright;

		// Token: 0x040014E4 RID: 5348
		private object[] modulesToImport;

		// Token: 0x040014E5 RID: 5349
		private string[] visibleAliases = new string[0];

		// Token: 0x040014E6 RID: 5350
		private object[] visibleCmdlets;

		// Token: 0x040014E7 RID: 5351
		private object[] visibleFunctions;

		// Token: 0x040014E8 RID: 5352
		private string[] visibleExternalCommands = new string[0];

		// Token: 0x040014E9 RID: 5353
		private string[] visibleProviders = new string[0];

		// Token: 0x040014EA RID: 5354
		private string[] scriptsToProcess = new string[0];

		// Token: 0x040014EB RID: 5355
		private IDictionary[] aliasDefinitions;

		// Token: 0x040014EC RID: 5356
		private IDictionary[] functionDefinitions;

		// Token: 0x040014ED RID: 5357
		private object variableDefinitions;

		// Token: 0x040014EE RID: 5358
		private IDictionary environmentVariables;

		// Token: 0x040014EF RID: 5359
		private string[] typesToProcess = new string[0];

		// Token: 0x040014F0 RID: 5360
		private string[] formatsToProcess = new string[0];

		// Token: 0x040014F1 RID: 5361
		private string[] assembliesToLoad;
	}
}
