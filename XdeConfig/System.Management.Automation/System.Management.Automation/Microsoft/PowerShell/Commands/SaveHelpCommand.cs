using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Help;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020001E2 RID: 482
	[Cmdlet("Save", "Help", DefaultParameterSetName = "Path", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=210612")]
	public sealed class SaveHelpCommand : UpdatableHelpCommandBase
	{
		// Token: 0x0600161E RID: 5662 RVA: 0x0008CD96 File Offset: 0x0008AF96
		public SaveHelpCommand() : base(UpdatableHelpCommandType.SaveHelpCommand)
		{
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x0600161F RID: 5663 RVA: 0x0008CD9F File Offset: 0x0008AF9F
		// (set) Token: 0x06001620 RID: 5664 RVA: 0x0008CDA7 File Offset: 0x0008AFA7
		[ValidateNotNull]
		[Parameter(Mandatory = true, Position = 0, ParameterSetName = "Path")]
		public string[] DestinationPath
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

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001621 RID: 5665 RVA: 0x0008CDB0 File Offset: 0x0008AFB0
		// (set) Token: 0x06001622 RID: 5666 RVA: 0x0008CDB8 File Offset: 0x0008AFB8
		[Parameter(Mandatory = true, ParameterSetName = "LiteralPath")]
		[Alias(new string[]
		{
			"PSPath"
		})]
		[ValidateNotNull]
		public string[] LiteralPath
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
				this.isLiteralPath = true;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001623 RID: 5667 RVA: 0x0008CDC8 File Offset: 0x0008AFC8
		// (set) Token: 0x06001624 RID: 5668 RVA: 0x0008CDD0 File Offset: 0x0008AFD0
		[Alias(new string[]
		{
			"Name"
		})]
		[Parameter(Position = 1, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "LiteralPath")]
		[ArgumentToModuleTransformation]
		[ValidateNotNull]
		[Parameter(Position = 1, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "Path")]
		public PSModuleInfo[] Module { get; set; }

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001625 RID: 5669 RVA: 0x0008CDD9 File Offset: 0x0008AFD9
		// (set) Token: 0x06001626 RID: 5670 RVA: 0x0008CDE1 File Offset: 0x0008AFE1
		[ValidateNotNull]
		[Parameter(ParameterSetName = "Path", ValueFromPipelineByPropertyName = true)]
		[Parameter(ParameterSetName = "LiteralPath", ValueFromPipelineByPropertyName = true)]
		public ModuleSpecification[] FullyQualifiedModule { get; set; }

		// Token: 0x06001627 RID: 5671 RVA: 0x0008CDEC File Offset: 0x0008AFEC
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
				List<string> list = null;
				List<PSModuleInfo> list2 = null;
				if (this.Module != null)
				{
					list = new List<string>();
					list2 = new List<PSModuleInfo>();
					foreach (PSModuleInfo psmoduleInfo in this.Module)
					{
						if (string.IsNullOrEmpty(psmoduleInfo.ModuleBase))
						{
							list.Add(psmoduleInfo.Name);
						}
						else
						{
							list2.Add(psmoduleInfo);
						}
					}
				}
				base.Process(list, this.FullyQualifiedModule);
				base.Process(list2);
			}
			finally
			{
				base.WriteProgress(new ProgressRecord(this.activityId, HelpDisplayStrings.SaveProgressActivityForModule, HelpDisplayStrings.UpdateProgressInstalling)
				{
					PercentComplete = 100,
					RecordType = ProgressRecordType.Completed
				});
			}
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x0008CEF0 File Offset: 0x0008B0F0
		internal override bool ProcessModuleWithCulture(UpdatableHelpModuleInfo module, string culture)
		{
			Collection<string> collection = new Collection<string>();
			foreach (string text in this._path)
			{
				UpdatableHelpSystemDrive updatableHelpSystemDrive = null;
				try
				{
					if (string.IsNullOrEmpty(text))
					{
						PSArgumentException ex = new PSArgumentException(StringUtil.Format(HelpDisplayStrings.PathNullOrEmpty, new object[0]));
						base.WriteError(ex.ErrorRecord);
						return false;
					}
					string path2 = text;
					if (this._credential != null)
					{
						if (text.Contains("*"))
						{
							int num = text.IndexOf("*", StringComparison.OrdinalIgnoreCase);
							if (num == 0)
							{
								throw new UpdatableHelpSystemException("PathMustBeValidContainers", StringUtil.Format(HelpDisplayStrings.PathMustBeValidContainers, text), ErrorCategory.InvalidArgument, null, new ItemNotFoundException());
							}
							int num2 = num;
							while (num2 >= 0 && !text[num2].Equals('/') && !text[num2].Equals('\\'))
							{
								num2--;
							}
							if (num2 == 0)
							{
								throw new UpdatableHelpSystemException("PathMustBeValidContainers", StringUtil.Format(HelpDisplayStrings.PathMustBeValidContainers, text), ErrorCategory.InvalidArgument, null, new ItemNotFoundException());
							}
							updatableHelpSystemDrive = new UpdatableHelpSystemDrive(this, text.Substring(0, num2), this._credential);
							path2 = Path.Combine(updatableHelpSystemDrive.DriveName, text.Substring(num2 + 1, text.Length - (num2 + 1)));
						}
						else
						{
							updatableHelpSystemDrive = new UpdatableHelpSystemDrive(this, text, this._credential);
							path2 = updatableHelpSystemDrive.DriveName;
						}
					}
					if (this.isLiteralPath)
					{
						string unresolvedProviderPathFromPSPath = base.GetUnresolvedProviderPathFromPSPath(path2);
						if (!Directory.Exists(unresolvedProviderPathFromPSPath))
						{
							throw new UpdatableHelpSystemException("PathMustBeValidContainers", StringUtil.Format(HelpDisplayStrings.PathMustBeValidContainers, text), ErrorCategory.InvalidArgument, null, new ItemNotFoundException());
						}
						collection.Add(unresolvedProviderPathFromPSPath);
					}
					else
					{
						try
						{
							foreach (string item in base.ResolvePath(path2, false, false))
							{
								collection.Add(item);
							}
						}
						catch (ItemNotFoundException innerException)
						{
							throw new UpdatableHelpSystemException("PathMustBeValidContainers", StringUtil.Format(HelpDisplayStrings.PathMustBeValidContainers, text), ErrorCategory.InvalidArgument, null, innerException);
						}
					}
				}
				finally
				{
					if (updatableHelpSystemDrive != null)
					{
						updatableHelpSystemDrive.Dispose();
					}
				}
			}
			if (collection.Count == 0)
			{
				return true;
			}
			bool result = false;
			foreach (string text2 in collection)
			{
				UpdatableHelpInfo currentHelpInfo = null;
				UpdatableHelpInfo updatableHelpInfo = null;
				string text3 = this._force ? null : UpdatableHelpSystem.LoadStringFromPath(this, base.SessionState.Path.Combine(text2, module.GetHelpInfoName()), this._credential);
				if (text3 != null)
				{
					currentHelpInfo = this._helpSystem.CreateHelpInfo(text3, module.ModuleName, module.ModuleGuid, null, null, false, false, false);
				}
				if (!this.alreadyCheckedOncePerDayPerModule && !base.CheckOncePerDayPerModule(module.ModuleName, text2, module.GetHelpInfoName(), DateTime.UtcNow, this._force))
				{
					return true;
				}
				this.alreadyCheckedOncePerDayPerModule = true;
				string resolvedUri = this._helpSystem.GetHelpInfoUri(module, null).ResolvedUri;
				string uri = resolvedUri + module.GetHelpInfoName();
				updatableHelpInfo = this._helpSystem.GetHelpInfo(this._commandType, uri, module.ModuleName, module.ModuleGuid, culture);
				if (updatableHelpInfo == null)
				{
					throw new UpdatableHelpSystemException("UnableToRetrieveHelpInfoXml", StringUtil.Format(HelpDisplayStrings.UnableToRetrieveHelpInfoXml, culture), ErrorCategory.ResourceUnavailable, null, null);
				}
				string text4 = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
				foreach (UpdatableHelpUri updatableHelpUri in updatableHelpInfo.HelpContentUriCollection)
				{
					if (!base.IsUpdateNecessary(module, currentHelpInfo, updatableHelpInfo, updatableHelpUri.Culture, this._force))
					{
						base.WriteVerbose(StringUtil.Format(HelpDisplayStrings.SuccessfullyUpdatedHelpContent, new object[]
						{
							module.ModuleName,
							HelpDisplayStrings.NewestContentAlreadyDownloaded,
							updatableHelpUri.Culture.Name,
							updatableHelpInfo.GetCultureVersion(updatableHelpUri.Culture)
						}));
						result = true;
					}
					else
					{
						string resolvedUri2 = updatableHelpUri.ResolvedUri;
						string helpContentName = module.GetHelpContentName(updatableHelpUri.Culture);
						UpdatableHelpSystemDrive updatableHelpSystemDrive2 = null;
						try
						{
							if (Directory.Exists(resolvedUri2))
							{
								File.Copy(base.SessionState.Path.Combine(resolvedUri2, helpContentName), base.SessionState.Path.Combine(text2, helpContentName), true);
							}
							else
							{
								if (this._credential != null)
								{
									try
									{
										updatableHelpSystemDrive2 = new UpdatableHelpSystemDrive(this, text2, this._credential);
										if (!this._helpSystem.DownloadHelpContent(this._commandType, text4, resolvedUri2, helpContentName, culture))
										{
											result = false;
											continue;
										}
										base.InvokeProvider.Item.Copy(new string[]
										{
											text4
										}, updatableHelpSystemDrive2.DriveName, true, CopyContainers.CopyChildrenOfTargetContainer, true, true);
										goto IL_4CF;
									}
									catch (Exception e)
									{
										CommandProcessorBase.CheckForSevereException(e);
										base.ProcessException(module.ModuleName, updatableHelpUri.Culture.Name, e);
										result = false;
										continue;
									}
								}
								if (!this._helpSystem.DownloadHelpContent(this._commandType, text2, resolvedUri2, helpContentName, culture))
								{
									result = false;
									continue;
								}
							}
							IL_4CF:
							if (this._credential != null)
							{
								this._helpSystem.GenerateHelpInfo(module.ModuleName, module.ModuleGuid, updatableHelpInfo.UnresolvedUri, updatableHelpUri.Culture.Name, updatableHelpInfo.GetCultureVersion(updatableHelpUri.Culture), text4, module.GetHelpInfoName(), this._force);
								base.InvokeProvider.Item.Copy(new string[]
								{
									Path.Combine(text4, module.GetHelpInfoName())
								}, Path.Combine(updatableHelpSystemDrive2.DriveName, module.GetHelpInfoName()), false, CopyContainers.CopyTargetContainer, true, true);
							}
							else
							{
								this._helpSystem.GenerateHelpInfo(module.ModuleName, module.ModuleGuid, updatableHelpInfo.UnresolvedUri, updatableHelpUri.Culture.Name, updatableHelpInfo.GetCultureVersion(updatableHelpUri.Culture), text2, module.GetHelpInfoName(), this._force);
							}
							base.WriteVerbose(StringUtil.Format(HelpDisplayStrings.SuccessfullyUpdatedHelpContent, new object[]
							{
								module.ModuleName,
								StringUtil.Format(HelpDisplayStrings.SavedHelpContent, Path.Combine(text2, helpContentName)),
								updatableHelpUri.Culture.Name,
								updatableHelpInfo.GetCultureVersion(updatableHelpUri.Culture)
							}));
							base.LogMessage(StringUtil.Format(HelpDisplayStrings.SaveHelpCompleted, text2));
						}
						catch (Exception e2)
						{
							CommandProcessorBase.CheckForSevereException(e2);
							base.ProcessException(module.ModuleName, updatableHelpUri.Culture.Name, e2);
						}
						finally
						{
							if (updatableHelpSystemDrive2 != null)
							{
								updatableHelpSystemDrive2.Dispose();
							}
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0400096F RID: 2415
		private bool alreadyCheckedOncePerDayPerModule;

		// Token: 0x04000970 RID: 2416
		private string[] _path;

		// Token: 0x04000971 RID: 2417
		private bool isLiteralPath;
	}
}
