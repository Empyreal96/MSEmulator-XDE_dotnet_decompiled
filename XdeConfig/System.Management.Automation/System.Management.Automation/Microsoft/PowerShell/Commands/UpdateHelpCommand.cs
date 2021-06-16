using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Help;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020001E1 RID: 481
	[Cmdlet("Update", "Help", DefaultParameterSetName = "Path", SupportsShouldProcess = true, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=210614")]
	public sealed class UpdateHelpCommand : UpdatableHelpCommandBase
	{
		// Token: 0x0600160F RID: 5647 RVA: 0x0008C333 File Offset: 0x0008A533
		public UpdateHelpCommand() : base(UpdatableHelpCommandType.UpdateHelpCommand)
		{
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001610 RID: 5648 RVA: 0x0008C33C File Offset: 0x0008A53C
		// (set) Token: 0x06001611 RID: 5649 RVA: 0x0008C344 File Offset: 0x0008A544
		[Alias(new string[]
		{
			"Name"
		})]
		[Parameter(Position = 0, ParameterSetName = "Path", ValueFromPipelineByPropertyName = true)]
		[ValidateNotNull]
		[Parameter(Position = 0, ParameterSetName = "LiteralPath", ValueFromPipelineByPropertyName = true)]
		public string[] Module
		{
			get
			{
				return this._module;
			}
			set
			{
				this._module = value;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001612 RID: 5650 RVA: 0x0008C34D File Offset: 0x0008A54D
		// (set) Token: 0x06001613 RID: 5651 RVA: 0x0008C355 File Offset: 0x0008A555
		[Parameter(ParameterSetName = "LiteralPath", ValueFromPipelineByPropertyName = true)]
		[ValidateNotNull]
		[Parameter(ParameterSetName = "Path", ValueFromPipelineByPropertyName = true)]
		public ModuleSpecification[] FullyQualifiedModule { get; set; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001614 RID: 5652 RVA: 0x0008C35E File Offset: 0x0008A55E
		// (set) Token: 0x06001615 RID: 5653 RVA: 0x0008C366 File Offset: 0x0008A566
		[Parameter(Position = 1, ParameterSetName = "Path")]
		[ValidateNotNull]
		public string[] SourcePath
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001616 RID: 5654 RVA: 0x0008C36F File Offset: 0x0008A56F
		// (set) Token: 0x06001617 RID: 5655 RVA: 0x0008C377 File Offset: 0x0008A577
		[ValidateNotNull]
		[Parameter(ParameterSetName = "LiteralPath", ValueFromPipelineByPropertyName = true)]
		[Alias(new string[]
		{
			"PSPath"
		})]
		public string[] LiteralPath
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
				this._isLiteralPath = true;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001618 RID: 5656 RVA: 0x0008C387 File Offset: 0x0008A587
		// (set) Token: 0x06001619 RID: 5657 RVA: 0x0008C394 File Offset: 0x0008A594
		[Parameter]
		public SwitchParameter Recurse
		{
			get
			{
				return this._recurse;
			}
			set
			{
				this._recurse = value;
			}
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x0008C3A4 File Offset: 0x0008A5A4
		protected override void BeginProcessing()
		{
			UpdatableHelpSystem.SetDisablePromptToUpdateHelp();
			if (this._path == null)
			{
				string defaultSourcePath = this._helpSystem.GetDefaultSourcePath();
				if (defaultSourcePath != null)
				{
					this._path = new string[]
					{
						defaultSourcePath
					};
				}
			}
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x0008C3E0 File Offset: 0x0008A5E0
		protected override void ProcessRecord()
		{
			try
			{
				if (this.Module != null && this.FullyQualifiedModule != null)
				{
					string message = StringUtil.Format(SessionStateStrings.GetContent_TailAndHeadCannotCoexist, "Module", "FullyQualifiedModule");
					ErrorRecord errorRecord = new ErrorRecord(new InvalidOperationException(message), "ModuleAndFullyQualifiedModuleCannotBeSpecifiedTogether", ErrorCategory.InvalidOperation, null);
					base.ThrowTerminatingError(errorRecord);
				}
				if (!this.isInitialized)
				{
					if (this._path == null && this.Recurse.IsPresent)
					{
						PSArgumentException ex = new PSArgumentException(StringUtil.Format(HelpDisplayStrings.CannotSpecifyRecurseWithoutPath, new object[0]));
						base.ThrowTerminatingError(ex.ErrorRecord);
					}
					this.isInitialized = true;
				}
				base.Process(this._module, this.FullyQualifiedModule);
				foreach (object obj in base.Context.HelpSystem.HelpProviders)
				{
					HelpProvider helpProvider = (HelpProvider)obj;
					if (this._stopping)
					{
						break;
					}
					helpProvider.Reset();
				}
			}
			finally
			{
				base.WriteProgress(new ProgressRecord(this.activityId, HelpDisplayStrings.UpdateProgressActivityForModule, HelpDisplayStrings.UpdateProgressInstalling)
				{
					PercentComplete = 100,
					RecordType = ProgressRecordType.Completed
				});
			}
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x0008C52C File Offset: 0x0008A72C
		internal override bool ProcessModuleWithCulture(UpdatableHelpModuleInfo module, string culture)
		{
			UpdatableHelpInfo updatableHelpInfo = null;
			UpdatableHelpInfo updatableHelpInfo2 = null;
			string text = UpdatableHelpSystem.LoadStringFromPath(this, base.SessionState.Path.Combine(module.ModuleBase, module.GetHelpInfoName()), null);
			if (text != null)
			{
				updatableHelpInfo = this._helpSystem.CreateHelpInfo(text, module.ModuleName, module.ModuleGuid, null, null, false, false, this._force);
			}
			if (!this.alreadyCheckedOncePerDayPerModule && !base.CheckOncePerDayPerModule(module.ModuleName, module.ModuleBase, module.GetHelpInfoName(), DateTime.UtcNow, this._force))
			{
				return true;
			}
			this.alreadyCheckedOncePerDayPerModule = true;
			if (this._path != null)
			{
				using (null)
				{
					try
					{
						Collection<string> collection = new Collection<string>();
						foreach (string text2 in this._path)
						{
							if (string.IsNullOrEmpty(text2))
							{
								PSArgumentException ex = new PSArgumentException(StringUtil.Format(HelpDisplayStrings.PathNullOrEmpty, new object[0]));
								base.WriteError(ex.ErrorRecord);
								return false;
							}
							try
							{
								string path2 = text2;
								if (this._credential != null)
								{
									UpdatableHelpSystemDrive updatableHelpSystemDrive2 = new UpdatableHelpSystemDrive(this, text2, this._credential);
									path2 = updatableHelpSystemDrive2.DriveName;
								}
								foreach (string item in base.ResolvePath(path2, this._recurse, this._isLiteralPath))
								{
									collection.Add(item);
								}
							}
							catch (System.Management.Automation.DriveNotFoundException e)
							{
								this.ThrowPathMustBeValidContainersException(text2, e);
							}
							catch (ItemNotFoundException e2)
							{
								this.ThrowPathMustBeValidContainersException(text2, e2);
							}
						}
						if (collection.Count == 0)
						{
							return true;
						}
						foreach (string text3 in collection)
						{
							string path3 = base.SessionState.Path.Combine(text3, module.GetHelpInfoName());
							text = UpdatableHelpSystem.LoadStringFromPath(this, path3, this._credential);
							if (text != null)
							{
								updatableHelpInfo2 = this._helpSystem.CreateHelpInfo(text, module.ModuleName, module.ModuleGuid, culture, text3, false, true, false);
								break;
							}
						}
					}
					catch (Exception ex2)
					{
						CommandProcessorBase.CheckForSevereException(ex2);
						throw new UpdatableHelpSystemException("UnableToRetrieveHelpInfoXml", StringUtil.Format(HelpDisplayStrings.UnableToRetrieveHelpInfoXml, culture), ErrorCategory.ResourceUnavailable, null, ex2);
					}
					goto IL_27E;
				}
			}
			string resolvedUri = this._helpSystem.GetHelpInfoUri(module, null).ResolvedUri;
			string uri = resolvedUri + module.GetHelpInfoName();
			updatableHelpInfo2 = this._helpSystem.GetHelpInfo(UpdatableHelpCommandType.UpdateHelpCommand, uri, module.ModuleName, module.ModuleGuid, culture);
			IL_27E:
			if (updatableHelpInfo2 == null)
			{
				throw new UpdatableHelpSystemException("UnableToRetrieveHelpInfoXml", StringUtil.Format(HelpDisplayStrings.UnableToRetrieveHelpInfoXml, culture), ErrorCategory.ResourceUnavailable, null, null);
			}
			bool result = false;
			foreach (UpdatableHelpUri updatableHelpUri in updatableHelpInfo2.HelpContentUriCollection)
			{
				Version version = (updatableHelpInfo != null) ? updatableHelpInfo.GetCultureVersion(updatableHelpUri.Culture) : null;
				string target = string.Format(CultureInfo.InvariantCulture, HelpDisplayStrings.UpdateHelpShouldProcessActionMessage, new object[]
				{
					module.ModuleName,
					(version != null) ? version.ToString() : "0.0.0.0",
					updatableHelpInfo2.GetCultureVersion(updatableHelpUri.Culture),
					updatableHelpUri.Culture
				});
				if (base.ShouldProcess(target, "Update-Help"))
				{
					if (Utils.IsUnderProductFolder(module.ModuleBase) && !Utils.IsAdministrator())
					{
						string message = StringUtil.Format(HelpErrors.UpdatableHelpRequiresElevation, new object[0]);
						base.ProcessException(module.ModuleName, null, new UpdatableHelpSystemException("UpdatableHelpSystemRequiresElevation", message, ErrorCategory.InvalidOperation, null, null));
						return false;
					}
					if (!base.IsUpdateNecessary(module, this._force ? null : updatableHelpInfo, updatableHelpInfo2, updatableHelpUri.Culture, this._force))
					{
						base.WriteVerbose(StringUtil.Format(HelpDisplayStrings.SuccessfullyUpdatedHelpContent, new object[]
						{
							module.ModuleName,
							HelpDisplayStrings.NewestContentAlreadyInstalled,
							updatableHelpUri.Culture.Name,
							updatableHelpInfo2.GetCultureVersion(updatableHelpUri.Culture)
						}));
						result = true;
					}
					else
					{
						try
						{
							string resolvedUri2 = updatableHelpUri.ResolvedUri;
							string xsdPath = base.SessionState.Path.Combine(Utils.GetApplicationBase(base.Context.ShellID), "Schemas\\PSMaml\\maml.xsd");
							Collection<string> collection2 = new Collection<string>();
							collection2.Add(module.ModuleBase);
							if (UpdatableHelpCommandBase.IsSystemModule(module.ModuleName) && ClrFacade.Is64BitOperatingSystem())
							{
								string item2 = Utils.GetApplicationBase(Utils.DefaultPowerShellShellID).Replace("System32", "SysWOW64");
								collection2.Add(item2);
							}
							Collection<string> collection3;
							if (Directory.Exists(resolvedUri2))
							{
								if (this._credential != null)
								{
									string helpContentName = module.GetHelpContentName(updatableHelpUri.Culture);
									string text4 = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
									try
									{
										using (UpdatableHelpSystemDrive updatableHelpSystemDrive3 = new UpdatableHelpSystemDrive(this, resolvedUri2, this._credential))
										{
											if (!Directory.Exists(text4))
											{
												Directory.CreateDirectory(text4);
											}
											base.InvokeProvider.Item.Copy(new string[]
											{
												Path.Combine(updatableHelpSystemDrive3.DriveName, helpContentName)
											}, Path.Combine(text4, helpContentName), false, CopyContainers.CopyTargetContainer, true, true);
											this._helpSystem.InstallHelpContent(UpdatableHelpCommandType.UpdateHelpCommand, base.Context, text4, collection2, module.GetHelpContentName(updatableHelpUri.Culture), Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName())), updatableHelpUri.Culture, xsdPath, out collection3);
										}
										goto IL_60D;
									}
									catch (Exception ex3)
									{
										CommandProcessorBase.CheckForSevereException(ex3);
										throw new UpdatableHelpSystemException("HelpContentNotFound", StringUtil.Format(HelpDisplayStrings.HelpContentNotFound, new object[0]), ErrorCategory.ResourceUnavailable, null, ex3);
									}
								}
								this._helpSystem.InstallHelpContent(UpdatableHelpCommandType.UpdateHelpCommand, base.Context, resolvedUri2, collection2, module.GetHelpContentName(updatableHelpUri.Culture), Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName())), updatableHelpUri.Culture, xsdPath, out collection3);
							}
							else if (!this._helpSystem.DownloadAndInstallHelpContent(UpdatableHelpCommandType.UpdateHelpCommand, base.Context, collection2, module.GetHelpContentName(updatableHelpUri.Culture), updatableHelpUri.Culture, resolvedUri2, xsdPath, out collection3))
							{
								result = false;
								continue;
							}
							IL_60D:
							this._helpSystem.GenerateHelpInfo(module.ModuleName, module.ModuleGuid, updatableHelpInfo2.UnresolvedUri, updatableHelpUri.Culture.Name, updatableHelpInfo2.GetCultureVersion(updatableHelpUri.Culture), module.ModuleBase, module.GetHelpInfoName(), this._force);
							foreach (string o in collection3)
							{
								base.WriteVerbose(StringUtil.Format(HelpDisplayStrings.SuccessfullyUpdatedHelpContent, new object[]
								{
									module.ModuleName,
									StringUtil.Format(HelpDisplayStrings.UpdatedHelpContent, o),
									updatableHelpUri.Culture.Name,
									updatableHelpInfo2.GetCultureVersion(updatableHelpUri.Culture)
								}));
							}
							base.LogMessage(StringUtil.Format(HelpDisplayStrings.UpdateHelpCompleted, new object[0]));
							result = true;
						}
						catch (Exception e3)
						{
							CommandProcessorBase.CheckForSevereException(e3);
							base.ProcessException(module.ModuleName, updatableHelpUri.Culture.Name, e3);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x0008CD7C File Offset: 0x0008AF7C
		private void ThrowPathMustBeValidContainersException(string path, Exception e)
		{
			throw new UpdatableHelpSystemException("PathMustBeValidContainers", StringUtil.Format(HelpDisplayStrings.PathMustBeValidContainers, path), ErrorCategory.InvalidArgument, null, e);
		}

		// Token: 0x04000968 RID: 2408
		private bool alreadyCheckedOncePerDayPerModule;

		// Token: 0x04000969 RID: 2409
		private string[] _module;

		// Token: 0x0400096A RID: 2410
		private string[] _path;

		// Token: 0x0400096B RID: 2411
		private bool _isLiteralPath;

		// Token: 0x0400096C RID: 2412
		private bool _recurse;

		// Token: 0x0400096D RID: 2413
		private bool isInitialized;
	}
}
