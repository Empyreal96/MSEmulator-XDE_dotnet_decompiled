using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000C4 RID: 196
	[OutputType(new Type[]
	{
		typeof(PSModuleInfo)
	})]
	[Cmdlet("Test", "ModuleManifest", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=141557")]
	public sealed class TestModuleManifestCommand : ModuleCmdletBase
	{
		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0003FEA6 File Offset: 0x0003E0A6
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x0003FEAE File Offset: 0x0003E0AE
		[Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, ValueFromPipelineByPropertyName = true)]
		public string Path
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

		// Token: 0x06000ABB RID: 2747 RVA: 0x0003FEB8 File Offset: 0x0003E0B8
		protected override void ProcessRecord()
		{
			ProviderInfo providerInfo = null;
			Collection<string> collection;
			try
			{
				if (base.Context.EngineSessionState.IsProviderLoaded(base.Context.ProviderNames.FileSystem))
				{
					collection = base.SessionState.Path.GetResolvedProviderPathFromPSPath(this._path, out providerInfo);
				}
				else
				{
					collection = new Collection<string>();
					collection.Add(this._path);
				}
			}
			catch (ItemNotFoundException)
			{
				string message = StringUtil.Format(Modules.ModuleNotFound, this._path);
				FileNotFoundException exception = new FileNotFoundException(message);
				ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_ModuleNotFound", ErrorCategory.ResourceUnavailable, this._path);
				base.WriteError(errorRecord);
				return;
			}
			if (!providerInfo.NameEquals(base.Context.ProviderNames.FileSystem))
			{
				throw InterpreterError.NewInterpreterException(this._path, typeof(RuntimeException), null, "FileOpenError", ParserStrings.FileOpenError, new object[]
				{
					providerInfo.FullName
				});
			}
			if (collection == null || collection.Count < 1)
			{
				string message2 = StringUtil.Format(Modules.ModuleNotFound, this._path);
				FileNotFoundException exception2 = new FileNotFoundException(message2);
				ErrorRecord errorRecord2 = new ErrorRecord(exception2, "Modules_ModuleNotFound", ErrorCategory.ResourceUnavailable, this._path);
				base.WriteError(errorRecord2);
			}
			else
			{
				if (collection.Count > 1)
				{
					throw InterpreterError.NewInterpreterException(collection, typeof(RuntimeException), null, "AmbiguousPath", ParserStrings.AmbiguousPath, new object[0]);
				}
				string text = collection[0];
				string extension = System.IO.Path.GetExtension(text);
				if (extension.Equals(".psd1", StringComparison.OrdinalIgnoreCase))
				{
					string text2;
					ExternalScriptInfo scriptInfoForFile = base.GetScriptInfoForFile(text, out text2, false);
					string moduleBeingProcessed = base.Context.ModuleBeingProcessed;
					PSModuleInfo psmoduleInfo;
					try
					{
						psmoduleInfo = base.LoadModuleManifest(scriptInfoForFile, ModuleCmdletBase.ManifestProcessingFlags.WriteErrors | ModuleCmdletBase.ManifestProcessingFlags.WriteWarnings, null, null, null, null);
					}
					finally
					{
						base.Context.ModuleBeingProcessed = moduleBeingProcessed;
					}
					DirectoryInfo directoryInfo = null;
					try
					{
						directoryInfo = ClrFacade.GetParent(text);
					}
					catch (IOException)
					{
					}
					catch (UnauthorizedAccessException)
					{
					}
					catch (ArgumentException)
					{
					}
					Version version;
					if (directoryInfo != null && Version.TryParse(directoryInfo.Name, out version))
					{
						if (!version.Equals(psmoduleInfo.Version))
						{
							string message3 = StringUtil.Format(Modules.InvalidModuleManifestVersion, new object[]
							{
								text,
								psmoduleInfo.Version.ToString(),
								directoryInfo.FullName
							});
							InvalidOperationException exception3 = new InvalidOperationException(message3);
							ErrorRecord errorRecord3 = new ErrorRecord(exception3, "Modules_InvalidModuleManifestVersion", ErrorCategory.InvalidArgument, this._path);
							base.ThrowTerminatingError(errorRecord3);
						}
						base.WriteVerbose(Modules.ModuleVersionEqualsToVersionFolder);
					}
					if (psmoduleInfo != null)
					{
						base.WriteObject(psmoduleInfo);
						return;
					}
				}
				else
				{
					string message4 = StringUtil.Format(Modules.InvalidModuleManifestPath, text);
					InvalidOperationException exception4 = new InvalidOperationException(message4);
					ErrorRecord errorRecord4 = new ErrorRecord(exception4, "Modules_InvalidModuleManifestPath", ErrorCategory.InvalidArgument, this._path);
					base.ThrowTerminatingError(errorRecord4);
				}
				return;
			}
		}

		// Token: 0x040004BF RID: 1215
		private string _path;
	}
}
