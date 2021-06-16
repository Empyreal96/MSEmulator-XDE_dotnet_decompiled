using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Security;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000859 RID: 2137
	[Cmdlet("Export", "Console", SupportsShouldProcess = true, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113298")]
	public sealed class ExportConsoleCommand : ConsoleCmdletsBase
	{
		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x06005239 RID: 21049 RVA: 0x001B6FFE File Offset: 0x001B51FE
		// (set) Token: 0x0600523A RID: 21050 RVA: 0x001B7006 File Offset: 0x001B5206
		[Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		[Alias(new string[]
		{
			"PSPath"
		})]
		public string Path
		{
			get
			{
				return this.fileName;
			}
			set
			{
				this.fileName = value;
			}
		}

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x0600523B RID: 21051 RVA: 0x001B700F File Offset: 0x001B520F
		// (set) Token: 0x0600523C RID: 21052 RVA: 0x001B701C File Offset: 0x001B521C
		[Parameter]
		public SwitchParameter Force
		{
			get
			{
				return this.force;
			}
			set
			{
				this.force = value;
			}
		}

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x0600523D RID: 21053 RVA: 0x001B702A File Offset: 0x001B522A
		// (set) Token: 0x0600523E RID: 21054 RVA: 0x001B7037 File Offset: 0x001B5237
		[Alias(new string[]
		{
			"NoOverwrite"
		})]
		[Parameter]
		public SwitchParameter NoClobber
		{
			get
			{
				return this.noclobber;
			}
			set
			{
				this.noclobber = value;
			}
		}

		// Token: 0x0600523F RID: 21055 RVA: 0x001B7048 File Offset: 0x001B5248
		protected override void ProcessRecord()
		{
			string text = this.GetFileName();
			if (string.IsNullOrEmpty(text))
			{
				text = this.PromptUserForFile();
			}
			if (string.IsNullOrEmpty(text))
			{
				PSArgumentException innerException = PSTraceSource.NewArgumentException("file", ConsoleInfoErrorStrings.FileNameNotResolved, new object[0]);
				base.ThrowError(text, "FileNameNotResolved", innerException, ErrorCategory.InvalidArgument);
			}
			if (WildcardPattern.ContainsWildcardCharacters(text))
			{
				base.ThrowError(text, "WildCardNotSupported", PSTraceSource.NewInvalidOperationException(ConsoleInfoErrorStrings.ConsoleFileWildCardsNotSupported, new object[]
				{
					text
				}), ErrorCategory.InvalidOperation);
			}
			string text2 = this.ResolveProviderAndPath(text);
			if (string.IsNullOrEmpty(text2))
			{
				return;
			}
			if (!text2.EndsWith(".psc1", StringComparison.OrdinalIgnoreCase))
			{
				text2 += ".psc1";
			}
			if (!base.ShouldProcess(this.Path))
			{
				return;
			}
			if (File.Exists(text2))
			{
				if (this.NoClobber)
				{
					string message = StringUtil.Format(ConsoleInfoErrorStrings.FileExistsNoClobber, text2, "NoClobber");
					Exception exception = new UnauthorizedAccessException(message);
					ErrorRecord errorRecord = new ErrorRecord(exception, "NoClobber", ErrorCategory.ResourceExists, text2);
					base.ThrowTerminatingError(errorRecord);
				}
				FileAttributes attributes = File.GetAttributes(text2);
				if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					if (this.Force)
					{
						this.RemoveFileThrowIfError(text2);
					}
					else
					{
						base.ThrowError(text, "ConsoleFileReadOnly", PSTraceSource.NewArgumentException(text, ConsoleInfoErrorStrings.ConsoleFileReadOnly, new object[]
						{
							text2
						}), ErrorCategory.InvalidArgument);
					}
				}
			}
			try
			{
				if (base.Runspace != null)
				{
					base.Runspace.SaveAsConsoleFile(text2);
				}
				else
				{
					if (base.InitialSessionState == null)
					{
						throw PSTraceSource.NewInvalidOperationException(ConsoleInfoErrorStrings.CmdletNotAvailable, new object[0]);
					}
					base.InitialSessionState.SaveAsConsoleFile(text2);
				}
			}
			catch (PSArgumentException innerException2)
			{
				base.ThrowError(text2, "PathNotAbsolute", innerException2, ErrorCategory.InvalidArgument);
			}
			catch (PSArgumentNullException innerException3)
			{
				base.ThrowError(text2, "PathNull", innerException3, ErrorCategory.InvalidArgument);
			}
			catch (ArgumentException innerException4)
			{
				base.ThrowError(text2, "InvalidCharacetersInPath", innerException4, ErrorCategory.InvalidArgument);
			}
			Exception ex = null;
			try
			{
				base.Context.EngineSessionState.SetConsoleVariable();
			}
			catch (ArgumentNullException ex2)
			{
				ex = ex2;
			}
			catch (ArgumentOutOfRangeException ex3)
			{
				ex = ex3;
			}
			catch (ArgumentException ex4)
			{
				ex = ex4;
			}
			catch (SessionStateUnauthorizedAccessException ex5)
			{
				ex = ex5;
			}
			catch (SessionStateOverflowException ex6)
			{
				ex = ex6;
			}
			catch (ProviderNotFoundException ex7)
			{
				ex = ex7;
			}
			catch (System.Management.Automation.DriveNotFoundException ex8)
			{
				ex = ex8;
			}
			catch (NotSupportedException ex9)
			{
				ex = ex9;
			}
			catch (ProviderInvocationException ex10)
			{
				ex = ex10;
			}
			if (ex != null)
			{
				throw PSTraceSource.NewInvalidOperationException(ex, ConsoleInfoErrorStrings.ConsoleVariableCannotBeSet, new object[]
				{
					text2
				});
			}
		}

		// Token: 0x06005240 RID: 21056 RVA: 0x001B7320 File Offset: 0x001B5520
		private void RemoveFileThrowIfError(string destination)
		{
			FileInfo fileInfo = new FileInfo(destination);
			if (fileInfo != null)
			{
				Exception ex = null;
				try
				{
					fileInfo.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.Hidden);
					fileInfo.Delete();
				}
				catch (FileNotFoundException ex2)
				{
					ex = ex2;
				}
				catch (DirectoryNotFoundException ex3)
				{
					ex = ex3;
				}
				catch (UnauthorizedAccessException ex4)
				{
					ex = ex4;
				}
				catch (SecurityException ex5)
				{
					ex = ex5;
				}
				catch (ArgumentNullException ex6)
				{
					ex = ex6;
				}
				catch (ArgumentException ex7)
				{
					ex = ex7;
				}
				catch (PathTooLongException ex8)
				{
					ex = ex8;
				}
				catch (NotSupportedException ex9)
				{
					ex = ex9;
				}
				catch (IOException ex10)
				{
					ex = ex10;
				}
				if (ex != null)
				{
					throw PSTraceSource.NewInvalidOperationException(ex, ConsoleInfoErrorStrings.ExportConsoleCannotDeleteFile, new object[]
					{
						fileInfo
					});
				}
			}
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x001B741C File Offset: 0x001B561C
		private string ResolveProviderAndPath(string path)
		{
			CmdletProviderContext currentCommandContext = new CmdletProviderContext(this);
			PathInfo pathInfo = this.ResolvePath(path, true, currentCommandContext);
			if (pathInfo == null)
			{
				return null;
			}
			if (pathInfo.Provider.ImplementingType == typeof(FileSystemProvider))
			{
				return pathInfo.Path;
			}
			throw PSTraceSource.NewInvalidOperationException(ConsoleInfoErrorStrings.ProviderNotSupported, new object[]
			{
				pathInfo.Provider.Name
			});
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x001B7484 File Offset: 0x001B5684
		private PathInfo ResolvePath(string pathToResolve, bool allowNonexistingPaths, CmdletProviderContext currentCommandContext)
		{
			Collection<PathInfo> collection = new Collection<PathInfo>();
			try
			{
				Collection<PathInfo> resolvedPSPathFromPSPath = base.SessionState.Path.GetResolvedPSPathFromPSPath(pathToResolve, currentCommandContext);
				foreach (PathInfo item in resolvedPSPathFromPSPath)
				{
					collection.Add(item);
				}
			}
			catch (PSNotSupportedException ex)
			{
				base.WriteError(new ErrorRecord(ex.ErrorRecord, ex));
			}
			catch (System.Management.Automation.DriveNotFoundException ex2)
			{
				base.WriteError(new ErrorRecord(ex2.ErrorRecord, ex2));
			}
			catch (ProviderNotFoundException ex3)
			{
				base.WriteError(new ErrorRecord(ex3.ErrorRecord, ex3));
			}
			catch (ItemNotFoundException ex4)
			{
				if (allowNonexistingPaths)
				{
					ProviderInfo provider = null;
					PSDriveInfo drive = null;
					string unresolvedProviderPathFromPSPath = base.SessionState.Path.GetUnresolvedProviderPathFromPSPath(pathToResolve, currentCommandContext, out provider, out drive);
					PathInfo item2 = new PathInfo(drive, provider, unresolvedProviderPathFromPSPath, base.SessionState);
					collection.Add(item2);
				}
				else
				{
					base.WriteError(new ErrorRecord(ex4.ErrorRecord, ex4));
				}
			}
			if (collection.Count == 1)
			{
				return collection[0];
			}
			if (collection.Count > 1)
			{
				Exception exception = PSTraceSource.NewNotSupportedException();
				base.WriteError(new ErrorRecord(exception, "NotSupported", ErrorCategory.NotImplemented, collection));
				return null;
			}
			return null;
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x001B75F8 File Offset: 0x001B57F8
		private string GetFileName()
		{
			if (!string.IsNullOrEmpty(this.fileName))
			{
				return this.fileName;
			}
			PSVariable psvariable = base.Context.SessionState.PSVariable.Get("ConsoleFileName");
			if (psvariable == null)
			{
				return string.Empty;
			}
			string text = psvariable.Value as string;
			if (text == null)
			{
				PSObject psobject = psvariable.Value as PSObject;
				if (psobject != null && psobject.BaseObject is string)
				{
					text = (psobject.BaseObject as string);
				}
			}
			if (text != null)
			{
				return text;
			}
			throw PSTraceSource.NewArgumentException("fileName", ConsoleInfoErrorStrings.ConsoleCannotbeConvertedToString, new object[0]);
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x001B7690 File Offset: 0x001B5890
		private string PromptUserForFile()
		{
			if (!base.ShouldContinue(ConsoleInfoErrorStrings.PromptForExportConsole, null))
			{
				return string.Empty;
			}
			string caption = StringUtil.Format(ConsoleInfoErrorStrings.FileNameCaptionForExportConsole, "export-console");
			string fileNamePromptMessage = ConsoleInfoErrorStrings.FileNamePromptMessage;
			Collection<FieldDescription> collection = new Collection<FieldDescription>();
			collection.Add(new FieldDescription("Name"));
			Dictionary<string, PSObject> dictionary = base.PSHostInternal.UI.Prompt(caption, fileNamePromptMessage, collection);
			if (dictionary != null && dictionary["Name"] != null)
			{
				return dictionary["Name"].BaseObject as string;
			}
			return string.Empty;
		}

		// Token: 0x04002A38 RID: 10808
		private string fileName;

		// Token: 0x04002A39 RID: 10809
		private bool force;

		// Token: 0x04002A3A RID: 10810
		private bool noclobber;
	}
}
