using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.PowerShell.Commands;
using Microsoft.Win32;

namespace System.Management.Automation.Host
{
	// Token: 0x0200020D RID: 525
	public abstract class PSHostUserInterface
	{
		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x0600186F RID: 6255
		public abstract PSHostRawUserInterface RawUI { get; }

		// Token: 0x06001870 RID: 6256
		public abstract string ReadLine();

		// Token: 0x06001871 RID: 6257
		public abstract SecureString ReadLineAsSecureString();

		// Token: 0x06001872 RID: 6258
		public abstract void Write(string value);

		// Token: 0x06001873 RID: 6259
		public abstract void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value);

		// Token: 0x06001874 RID: 6260 RVA: 0x000951FB File Offset: 0x000933FB
		public virtual void WriteLine()
		{
			this.WriteLine("");
		}

		// Token: 0x06001875 RID: 6261
		public abstract void WriteLine(string value);

		// Token: 0x06001876 RID: 6262 RVA: 0x00095208 File Offset: 0x00093408
		public virtual void WriteLine(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
		{
			if (value != null && value.Length != 0)
			{
				this.Write(foregroundColor, backgroundColor, value);
			}
			this.Write("\n");
		}

		// Token: 0x06001877 RID: 6263
		public abstract void WriteErrorLine(string value);

		// Token: 0x06001878 RID: 6264
		public abstract void WriteDebugLine(string message);

		// Token: 0x06001879 RID: 6265
		public abstract void WriteProgress(long sourceId, ProgressRecord record);

		// Token: 0x0600187A RID: 6266
		public abstract void WriteVerboseLine(string message);

		// Token: 0x0600187B RID: 6267
		public abstract void WriteWarningLine(string message);

		// Token: 0x0600187C RID: 6268 RVA: 0x00095229 File Offset: 0x00093429
		public virtual void WriteInformation(InformationRecord record)
		{
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x0600187D RID: 6269 RVA: 0x0009522C File Offset: 0x0009342C
		private TranscriptionData TranscriptionData
		{
			get
			{
				LocalRunspace localRunspace = Runspace.DefaultRunspace as LocalRunspace;
				if (localRunspace != null)
				{
					this.volatileTranscriptionData = localRunspace.TranscriptionData;
					if (this.volatileTranscriptionData != null)
					{
						return this.volatileTranscriptionData;
					}
				}
				if (this.volatileTranscriptionData != null)
				{
					return this.volatileTranscriptionData;
				}
				return new TranscriptionData();
			}
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x00095278 File Offset: 0x00093478
		internal void TranscribeCommand(string commandText, InvocationInfo invocation)
		{
			if (this.ShouldIgnoreCommand(commandText, invocation))
			{
				return;
			}
			if (this.IsTranscribing)
			{
				foreach (TranscriptionOption transcriptionOption in this.TranscriptionData.Transcripts.Prepend(this.TranscriptionData.SystemTranscript))
				{
					if (transcriptionOption != null)
					{
						lock (transcriptionOption.OutputToLog)
						{
							if (transcriptionOption.OutputToLog.Count == 0)
							{
								if (transcriptionOption.IncludeInvocationHeader)
								{
									transcriptionOption.OutputToLog.Add("**********************");
									transcriptionOption.OutputToLog.Add(string.Format(CultureInfo.InvariantCulture, InternalHostUserInterfaceStrings.CommandStartTime, new object[]
									{
										DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)
									}));
									transcriptionOption.OutputToLog.Add("**********************");
								}
								transcriptionOption.OutputToLog.Add(this.TranscriptionData.PromptText + commandText);
							}
							else
							{
								transcriptionOption.OutputToLog.Add(">> " + commandText);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x000953CC File Offset: 0x000935CC
		private bool ShouldIgnoreCommand(string logElement, InvocationInfo invocation)
		{
			string b = logElement;
			if (invocation != null)
			{
				b = invocation.InvocationName;
				CmdletInfo cmdletInfo = invocation.MyCommand as CmdletInfo;
				if (cmdletInfo != null && cmdletInfo.ImplementingType == typeof(OutDefaultCommand))
				{
					return true;
				}
				if (invocation.CommandOrigin == CommandOrigin.Internal)
				{
					this.IgnoreCommand(logElement, invocation);
					return true;
				}
			}
			string[] array = new string[]
			{
				"TabExpansion2",
				"prompt",
				"TabExpansion",
				"PSConsoleHostReadline"
			};
			foreach (string a in array)
			{
				if (string.Equals(a, b, StringComparison.OrdinalIgnoreCase))
				{
					this.IgnoreCommand(logElement, invocation);
					this.TranscriptionData.IsHelperCommand = true;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00095494 File Offset: 0x00093694
		internal void IgnoreCommand(string commandText, InvocationInfo invocation)
		{
			this.TranscribeCommandComplete(null);
			if (this.TranscriptionData.CommandBeingIgnored == null)
			{
				this.TranscriptionData.CommandBeingIgnored = commandText;
				this.TranscriptionData.IsHelperCommand = false;
				if (invocation != null && invocation.MyCommand != null)
				{
					this.TranscriptionData.CommandBeingIgnored = invocation.MyCommand.Name;
				}
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001881 RID: 6273 RVA: 0x000954EE File Offset: 0x000936EE
		// (set) Token: 0x06001882 RID: 6274 RVA: 0x000954F6 File Offset: 0x000936F6
		internal bool TranscribeOnly { get; set; }

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001883 RID: 6275 RVA: 0x000954FF File Offset: 0x000936FF
		internal bool IsTranscribing
		{
			get
			{
				this.CheckSystemTranscript();
				return this.TranscriptionData.Transcripts.Count > 0 || this.TranscriptionData.SystemTranscript != null;
			}
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x00095530 File Offset: 0x00093730
		private void CheckSystemTranscript()
		{
			lock (this.TranscriptionData)
			{
				if (this.TranscriptionData.SystemTranscript == null)
				{
					this.TranscriptionData.SystemTranscript = PSHostUserInterface.GetSystemTranscriptOption(this.TranscriptionData.SystemTranscript);
					if (this.TranscriptionData.SystemTranscript != null)
					{
						this.LogTranscriptHeader(null, this.TranscriptionData.SystemTranscript);
					}
				}
			}
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x000955B4 File Offset: 0x000937B4
		internal void StartTranscribing(string path, PSSenderInfo senderInfo, bool includeInvocationHeader)
		{
			TranscriptionOption transcriptionOption = new TranscriptionOption();
			transcriptionOption.Path = path;
			transcriptionOption.IncludeInvocationHeader = includeInvocationHeader;
			this.TranscriptionData.Transcripts.Add(transcriptionOption);
			this.LogTranscriptHeader(senderInfo, transcriptionOption);
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x000955F0 File Offset: 0x000937F0
		private void LogTranscriptHeader(PSSenderInfo senderInfo, TranscriptionOption transcript)
		{
			string text = Environment.UserDomainName + "\\" + Environment.UserName;
			string text2 = text;
			if (senderInfo != null)
			{
				text = senderInfo.UserInfo.Identity.Name;
			}
			StringBuilder stringBuilder = new StringBuilder();
			Hashtable psversionTable = PSVersionInfo.GetPSVersionTable();
			foreach (object obj in psversionTable.Keys)
			{
				string text3 = (string)obj;
				object obj2 = psversionTable[text3];
				string str;
				if (obj2 is IEnumerable)
				{
					str = string.Join(", ", (object[])obj2);
				}
				else
				{
					str = obj2.ToString();
				}
				stringBuilder.AppendLine(text3 + ": " + str);
			}
			string transcriptPrologue = InternalHostUserInterfaceStrings.TranscriptPrologue;
			string item = string.Format(CultureInfo.InvariantCulture, transcriptPrologue, new object[]
			{
				DateTime.Now,
				text,
				text2,
				Environment.MachineName,
				Environment.OSVersion.VersionString,
				string.Join(" ", Environment.GetCommandLineArgs()),
				Process.GetCurrentProcess().Id,
				stringBuilder.ToString().TrimEnd(new char[0])
			});
			lock (transcript.OutputToLog)
			{
				transcript.OutputToLog.Add(item);
			}
			this.TranscribeCommandComplete(null);
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00095794 File Offset: 0x00093994
		internal string StopTranscribing()
		{
			if (this.TranscriptionData.Transcripts.Count == 0)
			{
				throw new PSInvalidOperationException(InternalHostUserInterfaceStrings.HostNotTranscribing);
			}
			TranscriptionOption transcriptionOption = this.TranscriptionData.Transcripts[this.TranscriptionData.Transcripts.Count - 1];
			this.LogTranscriptFooter(transcriptionOption);
			transcriptionOption.Dispose();
			this.TranscriptionData.Transcripts.Remove(transcriptionOption);
			return transcriptionOption.Path;
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x00095808 File Offset: 0x00093A08
		private void LogTranscriptFooter(TranscriptionOption stoppedTranscript)
		{
			try
			{
				string item = string.Format(CultureInfo.InvariantCulture, InternalHostUserInterfaceStrings.TranscriptEpilogue, new object[]
				{
					DateTime.Now
				});
				lock (stoppedTranscript.OutputToLog)
				{
					stoppedTranscript.OutputToLog.Add(item);
				}
				this.TranscribeCommandComplete(null);
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x00095894 File Offset: 0x00093A94
		internal void StopAllTranscribing()
		{
			this.TranscribeCommandComplete(null);
			while (this.TranscriptionData.Transcripts.Count > 0)
			{
				this.StopTranscribing();
			}
			lock (this.TranscriptionData)
			{
				if (this.TranscriptionData.SystemTranscript != null)
				{
					this.LogTranscriptFooter(this.TranscriptionData.SystemTranscript);
					this.TranscriptionData.SystemTranscript.Dispose();
					this.TranscriptionData.SystemTranscript = null;
					lock (PSHostUserInterface.systemTranscriptLock)
					{
						PSHostUserInterface.systemTranscript = null;
					}
				}
			}
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0009595C File Offset: 0x00093B5C
		internal void TranscribeResult(Runspace sourceRunspace, string resultText)
		{
			if (this.IsTranscribing)
			{
				Runspace runspace = null;
				if (sourceRunspace != null)
				{
					runspace = Runspace.DefaultRunspace;
					Runspace.DefaultRunspace = sourceRunspace;
				}
				try
				{
					if (this.TranscriptionData.CommandBeingIgnored != null)
					{
						if (string.Equals("prompt", this.TranscriptionData.CommandBeingIgnored, StringComparison.OrdinalIgnoreCase))
						{
							this.TranscriptionData.PromptText = resultText;
						}
					}
					else
					{
						resultText = resultText.TrimEnd(new char[0]);
						foreach (TranscriptionOption transcriptionOption in this.TranscriptionData.Transcripts.Prepend(this.TranscriptionData.SystemTranscript))
						{
							if (transcriptionOption != null)
							{
								lock (transcriptionOption.OutputToLog)
								{
									transcriptionOption.OutputToLog.Add(resultText);
								}
							}
						}
					}
				}
				finally
				{
					if (runspace != null)
					{
						Runspace.DefaultRunspace = runspace;
					}
				}
			}
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x00095A6C File Offset: 0x00093C6C
		internal void TranscribeResult(string resultText)
		{
			this.TranscribeResult(null, resultText);
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00095A78 File Offset: 0x00093C78
		internal void TranscribeCommandComplete(InvocationInfo invocation)
		{
			this.FlushPendingOutput();
			if (invocation != null)
			{
				string a = this.TranscriptionData.CommandBeingIgnored;
				if (this.TranscriptionData.IsHelperCommand)
				{
					a = "Out-Default";
				}
				if (this.TranscriptionData.CommandBeingIgnored != null && invocation != null && string.Equals(a, invocation.MyCommand.Name, StringComparison.OrdinalIgnoreCase))
				{
					this.TranscriptionData.CommandBeingIgnored = null;
					this.TranscriptionData.IsHelperCommand = false;
				}
			}
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00095AE9 File Offset: 0x00093CE9
		internal void TranscribePipelineComplete()
		{
			this.FlushPendingOutput();
			this.TranscriptionData.CommandBeingIgnored = null;
			this.TranscriptionData.IsHelperCommand = false;
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x00095BA0 File Offset: 0x00093DA0
		private void FlushPendingOutput()
		{
			using (IEnumerator<TranscriptionOption> enumerator = this.TranscriptionData.Transcripts.Prepend(this.TranscriptionData.SystemTranscript).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TranscriptionOption transcript = enumerator.Current;
					if (transcript != null)
					{
						lock (transcript.OutputToLog)
						{
							if (transcript.OutputToLog.Count == 0)
							{
								continue;
							}
							lock (transcript.OutputBeingLogged)
							{
								bool flag3 = transcript.OutputBeingLogged.Count > 0;
								transcript.OutputBeingLogged.AddRange(transcript.OutputToLog);
								transcript.OutputToLog.Clear();
								if (flag3)
								{
									continue;
								}
							}
						}
						Task.Run(delegate()
						{
							int num = new Random().Next(10) + 1;
							bool flag4 = false;
							while (!flag4)
							{
								try
								{
									string directoryName = Path.GetDirectoryName(transcript.Path);
									if (!Directory.Exists(directoryName))
									{
										Directory.CreateDirectory(directoryName);
									}
									transcript.FlushContentToDisk();
									flag4 = true;
								}
								catch (IOException)
								{
									Thread.Sleep(num);
								}
								catch (UnauthorizedAccessException)
								{
									Thread.Sleep(num);
								}
								if (num < 1000)
								{
									num *= 2;
								}
							}
						});
					}
				}
			}
		}

		// Token: 0x0600188F RID: 6287
		public abstract Dictionary<string, PSObject> Prompt(string caption, string message, Collection<FieldDescription> descriptions);

		// Token: 0x06001890 RID: 6288
		public abstract PSCredential PromptForCredential(string caption, string message, string userName, string targetName);

		// Token: 0x06001891 RID: 6289
		public abstract PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options);

		// Token: 0x06001892 RID: 6290
		public abstract int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, int defaultChoice);

		// Token: 0x06001893 RID: 6291 RVA: 0x00095CFC File Offset: 0x00093EFC
		protected PSHostUserInterface()
		{
			this.CheckSystemTranscript();
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x00095D0C File Offset: 0x00093F0C
		internal void TranscribeError(ExecutionContext context, InvocationInfo invocation, PSObject errorWrap)
		{
			context.InternalHost.UI.TranscribeCommandComplete(invocation);
			InitialSessionState initialSessionState = InitialSessionState.CreateDefault2();
			Collection<PSObject> collection = PowerShell.Create(initialSessionState).AddCommand("Out-String").Invoke(new List<PSObject>
			{
				errorWrap
			});
			this.TranscribeResult(collection[0].ToString());
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x00095D68 File Offset: 0x00093F68
		internal static TranscriptionOption GetSystemTranscriptOption(TranscriptionOption currentTranscript)
		{
			Dictionary<string, object> groupPolicySetting = Utils.GetGroupPolicySetting("Transcription", new RegistryKey[]
			{
				Registry.LocalMachine,
				Registry.CurrentUser
			});
			if (groupPolicySetting != null)
			{
				lock (PSHostUserInterface.systemTranscriptLock)
				{
					if (PSHostUserInterface.systemTranscript == null)
					{
						PSHostUserInterface.systemTranscript = PSHostUserInterface.GetTranscriptOptionFromSettings(groupPolicySetting, currentTranscript);
					}
				}
			}
			return PSHostUserInterface.systemTranscript;
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x00095DE0 File Offset: 0x00093FE0
		private static TranscriptionOption GetTranscriptOptionFromSettings(Dictionary<string, object> settings, TranscriptionOption currentTranscript)
		{
			TranscriptionOption transcriptionOption = null;
			object obj = null;
			if (settings.TryGetValue("EnableTranscripting", out obj) && string.Equals(obj.ToString(), "1", StringComparison.OrdinalIgnoreCase))
			{
				if (currentTranscript != null)
				{
					return currentTranscript;
				}
				transcriptionOption = new TranscriptionOption();
				object obj2 = null;
				if (settings.TryGetValue("OutputDirectory", out obj2))
				{
					string baseDirectory = obj2 as string;
					transcriptionOption.Path = PSHostUserInterface.GetTranscriptPath(baseDirectory, true);
				}
				else
				{
					transcriptionOption.Path = PSHostUserInterface.GetTranscriptPath();
				}
				object obj3 = null;
				if (settings.TryGetValue("EnableInvocationHeader", out obj3) && string.Equals("1", obj3.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					transcriptionOption.IncludeInvocationHeader = true;
				}
			}
			return transcriptionOption;
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00095E80 File Offset: 0x00094080
		internal static string GetTranscriptPath()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			return PSHostUserInterface.GetTranscriptPath(folderPath, false);
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00095E9C File Offset: 0x0009409C
		internal static string GetTranscriptPath(string baseDirectory, bool includeDate)
		{
			if (string.IsNullOrEmpty(baseDirectory))
			{
				baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
			else if (!Path.IsPathRooted(baseDirectory))
			{
				baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), baseDirectory);
			}
			if (includeDate)
			{
				baseDirectory = Path.Combine(baseDirectory, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
			}
			byte[] array = new byte[6];
			RandomNumberGenerator.Create().GetBytes(array);
			string path = string.Format(CultureInfo.InvariantCulture, "PowerShell_transcript.{0}.{1}.{2:yyyyMMddHHmmss}.txt", new object[]
			{
				Environment.MachineName,
				Convert.ToBase64String(array).Replace('/', '_'),
				DateTime.Now
			});
			return Path.Combine(baseDirectory, path);
		}

		// Token: 0x04000A33 RID: 2611
		private TranscriptionData volatileTranscriptionData;

		// Token: 0x04000A34 RID: 2612
		internal static TranscriptionOption systemTranscript = null;

		// Token: 0x04000A35 RID: 2613
		private static object systemTranscriptLock = new object();
	}
}
