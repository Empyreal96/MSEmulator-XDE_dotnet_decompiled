using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000353 RID: 851
	[Cmdlet("Test", "PSSessionConfigurationFile", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=217039")]
	[OutputType(new string[]
	{
		"bool"
	})]
	public class TestPSSessionConfigurationFileCommand : PSCmdlet
	{
		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06002A65 RID: 10853 RVA: 0x000EA705 File Offset: 0x000E8905
		// (set) Token: 0x06002A66 RID: 10854 RVA: 0x000EA70D File Offset: 0x000E890D
		[Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, ValueFromPipelineByPropertyName = true)]
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

		// Token: 0x06002A67 RID: 10855 RVA: 0x000EA718 File Offset: 0x000E8918
		protected override void ProcessRecord()
		{
			ProviderInfo providerInfo = null;
			Collection<string> collection;
			try
			{
				if (base.Context.EngineSessionState.IsProviderLoaded(base.Context.ProviderNames.FileSystem))
				{
					collection = base.SessionState.Path.GetResolvedProviderPathFromPSPath(this.path, out providerInfo);
				}
				else
				{
					collection = new Collection<string>();
					collection.Add(this.path);
				}
			}
			catch (ItemNotFoundException)
			{
				string message = StringUtil.Format(RemotingErrorIdStrings.PSSessionConfigurationFileNotFound, this.path);
				FileNotFoundException exception = new FileNotFoundException(message);
				ErrorRecord errorRecord = new ErrorRecord(exception, "PSSessionConfigurationFileNotFound", ErrorCategory.ResourceUnavailable, this.path);
				base.WriteError(errorRecord);
				return;
			}
			if (!providerInfo.NameEquals(base.Context.ProviderNames.FileSystem))
			{
				throw InterpreterError.NewInterpreterException(this.path, typeof(RuntimeException), null, "FileOpenError", ParserStrings.FileOpenError, new object[]
				{
					providerInfo.FullName
				});
			}
			if (collection == null || collection.Count < 1)
			{
				string message2 = StringUtil.Format(RemotingErrorIdStrings.PSSessionConfigurationFileNotFound, this.path);
				FileNotFoundException exception2 = new FileNotFoundException(message2);
				ErrorRecord errorRecord2 = new ErrorRecord(exception2, "PSSessionConfigurationFileNotFound", ErrorCategory.ResourceUnavailable, this.path);
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
				if (!extension.Equals(".pssc", StringComparison.OrdinalIgnoreCase))
				{
					string message3 = StringUtil.Format(RemotingErrorIdStrings.InvalidPSSessionConfigurationFilePath, text);
					InvalidOperationException exception3 = new InvalidOperationException(message3);
					ErrorRecord errorRecord3 = new ErrorRecord(exception3, "InvalidPSSessionConfigurationFilePath", ErrorCategory.InvalidArgument, this.path);
					base.ThrowTerminatingError(errorRecord3);
					return;
				}
				string text2;
				ExternalScriptInfo scriptInfoForFile = DISCUtils.GetScriptInfoForFile(base.Context, text, out text2);
				Hashtable hashtable = null;
				try
				{
					hashtable = DISCUtils.LoadConfigFile(base.Context, scriptInfoForFile);
				}
				catch (RuntimeException ex)
				{
					base.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.DISCErrorParsingConfigFile, text, ex.Message));
					base.WriteObject(false);
					return;
				}
				if (hashtable == null)
				{
					base.WriteObject(false);
					return;
				}
				DISCUtils.ExecutionPolicyType = typeof(ExecutionPolicy);
				base.WriteObject(DISCUtils.VerifyConfigTable(hashtable, this, text));
				return;
			}
		}

		// Token: 0x040014F3 RID: 5363
		private string path;
	}
}
