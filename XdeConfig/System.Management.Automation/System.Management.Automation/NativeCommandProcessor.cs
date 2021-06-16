using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation
{
	// Token: 0x02000094 RID: 148
	internal class NativeCommandProcessor : CommandProcessorBase
	{
		// Token: 0x0600073C RID: 1852 RVA: 0x00022E5C File Offset: 0x0002105C
		internal NativeCommandProcessor(ApplicationInfo applicationInfo, ExecutionContext context) : base(applicationInfo)
		{
			if (applicationInfo == null)
			{
				throw PSTraceSource.NewArgumentNullException("applicationInfo");
			}
			this.applicationInfo = applicationInfo;
			this._context = context;
			base.Command = new NativeCommand();
			base.Command.CommandInfo = applicationInfo;
			base.Command.Context = context;
			base.Command.commandRuntime = (this.commandRuntime = new MshCommandRuntime(context, applicationInfo, base.Command));
			base.CommandScope = context.EngineSessionState.CurrentScope;
			((NativeCommand)base.Command).MyCommandProcessor = this;
			this.inputWriter = new ProcessInputWriter(base.Command);
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x00022F10 File Offset: 0x00021110
		private NativeCommand nativeCommand
		{
			get
			{
				return base.Command as NativeCommand;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x00022F2C File Offset: 0x0002112C
		private string NativeCommandName
		{
			get
			{
				return this.applicationInfo.Name;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x00022F48 File Offset: 0x00021148
		private string Path
		{
			get
			{
				return this.applicationInfo.Path;
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00022F62 File Offset: 0x00021162
		internal ParameterBinderController NewParameterBinderController(InternalCommand command)
		{
			if (this.isMiniShell)
			{
				this.nativeParameterBinderController = new MinishellParameterBinderController(this.nativeCommand);
			}
			else
			{
				this.nativeParameterBinderController = new NativeCommandParameterBinderController(this.nativeCommand);
			}
			return this.nativeParameterBinderController;
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00022F96 File Offset: 0x00021196
		internal NativeCommandParameterBinderController NativeParameterBinderController
		{
			get
			{
				if (this.nativeParameterBinderController == null)
				{
					this.NewParameterBinderController(base.Command);
				}
				return this.nativeParameterBinderController;
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00022FB3 File Offset: 0x000211B3
		internal override void Prepare(IDictionary psDefaultParameterValues)
		{
			this.isPreparedCalled = true;
			this.isMiniShell = this.IsMiniShell();
			if (!this.isMiniShell)
			{
				this.NativeParameterBinderController.BindParameters(this.arguments);
			}
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00022FE2 File Offset: 0x000211E2
		internal override void ProcessRecord()
		{
			while (this.Read())
			{
				this.inputWriter.Add(base.Command.CurrentPipelineObject);
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00023004 File Offset: 0x00021204
		internal override void Complete()
		{
			bool flag;
			bool redirectError;
			bool redirectInput;
			this.CalculateIORedirection(out flag, out redirectError, out redirectInput);
			bool flag2 = base.Command.MyInvocation.PipelineLength == 1;
			ProcessStartInfo processStartInfo = this.GetProcessStartInfo(flag, redirectError, redirectInput, flag2);
			if (base.Command.Context.CurrentPipelineStopping)
			{
				throw new PipelineStoppedException();
			}
			Exception ex = null;
			Coordinates upperLeft = default(Coordinates);
			bool flag3 = false;
			try
			{
				if (!flag)
				{
					base.Command.Context.EngineHostInterface.NotifyBeginApplication();
					try
					{
						if (base.Command.Context.EngineHostInterface.UI.IsTranscribing)
						{
							flag3 = true;
							upperLeft = base.Command.Context.EngineHostInterface.UI.RawUI.CursorPosition;
							upperLeft.X = 0;
						}
					}
					catch (HostException)
					{
						flag3 = false;
					}
				}
				lock (this.sync)
				{
					if (this.stopped)
					{
						throw new PipelineStoppedException();
					}
					try
					{
						this.nativeProcess = new Process();
						this.nativeProcess.StartInfo = processStartInfo;
						this.nativeProcess.Start();
					}
					catch (Win32Exception)
					{
						string text = NativeCommandProcessor.FindExecutable(processStartInfo.FileName);
						bool flag5 = true;
						if (!string.IsNullOrEmpty(text))
						{
							if (NativeCommandProcessor.IsConsoleApplication(text))
							{
								ConsoleVisibility.AllocateHiddenConsole();
							}
							string arguments = processStartInfo.Arguments;
							string fileName = processStartInfo.FileName;
							processStartInfo.Arguments = "\"" + processStartInfo.FileName + "\" " + processStartInfo.Arguments;
							processStartInfo.FileName = text;
							try
							{
								this.nativeProcess.Start();
								flag5 = false;
							}
							catch (Win32Exception)
							{
								processStartInfo.Arguments = arguments;
								processStartInfo.FileName = fileName;
							}
						}
						if (flag5)
						{
							if (!flag2 || processStartInfo.UseShellExecute)
							{
								throw;
							}
							processStartInfo.UseShellExecute = true;
							processStartInfo.RedirectStandardInput = false;
							processStartInfo.RedirectStandardOutput = false;
							processStartInfo.RedirectStandardError = false;
							this.nativeProcess.Start();
						}
					}
				}
				bool flag6;
				if (base.Command.MyInvocation.PipelinePosition < base.Command.MyInvocation.PipelineLength)
				{
					flag6 = false;
				}
				else
				{
					flag6 = true;
					if (!processStartInfo.UseShellExecute)
					{
						flag6 = NativeCommandProcessor.IsWindowsApplication(this.nativeProcess.StartInfo.FileName);
					}
				}
				try
				{
					if (processStartInfo.RedirectStandardInput)
					{
						NativeCommandIOFormat inputFormat = NativeCommandIOFormat.Text;
						if (this.isMiniShell)
						{
							inputFormat = ((MinishellParameterBinderController)this.NativeParameterBinderController).InputFormat;
						}
						lock (this.sync)
						{
							if (!this.stopped)
							{
								this.inputWriter.Start(this.nativeProcess, inputFormat);
							}
						}
					}
					if (!flag6 && (processStartInfo.RedirectStandardOutput || processStartInfo.RedirectStandardError))
					{
						lock (this.sync)
						{
							if (!this.stopped)
							{
								this.outputReader = new ProcessOutputReader(this.nativeProcess, this.Path, flag, redirectError);
								this.outputReader.Start();
							}
						}
						if (this.outputReader != null)
						{
							this.ProcessOutputHelper();
						}
					}
				}
				catch (Exception)
				{
					this.StopProcessing();
					throw;
				}
				finally
				{
					if (!flag6)
					{
						this.nativeProcess.WaitForExit();
						this.inputWriter.Done();
						if (this.outputReader != null)
						{
							this.outputReader.Done();
						}
						if (base.Command.Context.EngineHostInterface.UI.IsTranscribing && flag3)
						{
							Coordinates cursorPosition = base.Command.Context.EngineHostInterface.UI.RawUI.CursorPosition;
							cursorPosition.X = base.Command.Context.EngineHostInterface.UI.RawUI.BufferSize.Width - 1;
							if (cursorPosition.Y < upperLeft.Y)
							{
								upperLeft.Y = 0;
							}
							BufferCell[,] bufferContents = base.Command.Context.EngineHostInterface.UI.RawUI.GetBufferContents(new Rectangle(upperLeft, cursorPosition));
							StringBuilder stringBuilder = new StringBuilder();
							StringBuilder stringBuilder2 = new StringBuilder();
							for (int i = 0; i < bufferContents.GetLength(0); i++)
							{
								if (i > 0)
								{
									stringBuilder2.Append(Environment.NewLine);
								}
								stringBuilder.Clear();
								for (int j = 0; j < bufferContents.GetLength(1); j++)
								{
									stringBuilder.Append(bufferContents[i, j].Character);
								}
								stringBuilder2.Append(stringBuilder.ToString().TrimEnd(new char[]
								{
									' ',
									'\t'
								}));
							}
							base.Command.Context.InternalHost.UI.TranscribeResult(stringBuilder2.ToString());
						}
						base.Command.Context.SetVariable(SpecialVariables.LastExitCodeVarPath, this.nativeProcess.ExitCode);
						if (this.nativeProcess.ExitCode != 0)
						{
							this.commandRuntime.PipelineProcessor.ExecutionFailed = true;
						}
					}
				}
			}
			catch (Win32Exception ex2)
			{
				ex = ex2;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				ex = ex3;
			}
			finally
			{
				if (!flag)
				{
					base.Command.Context.EngineHostInterface.NotifyEndApplication();
				}
				this.CleanUp();
			}
			if (ex != null)
			{
				string text2 = StringUtil.Format(ParserStrings.ProgramFailedToExecute, new object[]
				{
					this.NativeCommandName,
					ex.Message,
					base.Command.MyInvocation.PositionMessage
				});
				if (text2 == null)
				{
					text2 = StringUtil.Format("Program '{0}' failed to execute: {1}{2}", new object[]
					{
						this.NativeCommandName,
						ex.Message,
						base.Command.MyInvocation.PositionMessage
					});
				}
				ApplicationFailedException ex4 = new ApplicationFailedException(text2, ex);
				throw ex4;
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00023704 File Offset: 0x00021904
		private static void KillProcess(Process processToKill)
		{
			if (NativeCommandProcessor.IsServerSide)
			{
				Process[] processes = Process.GetProcesses();
				NativeCommandProcessor.ProcessWithParentId[] currentlyRunningProcs = NativeCommandProcessor.ProcessWithParentId.Construct(processes);
				NativeCommandProcessor.KillProcessAndChildProcesses(processToKill, currentlyRunningProcs);
				return;
			}
			try
			{
				processToKill.Kill();
			}
			catch (Win32Exception)
			{
				try
				{
					Process processById = Process.GetProcessById(processToKill.Id);
					processById.Kill();
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
			catch (Exception e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
			}
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00023788 File Offset: 0x00021988
		private static void KillProcessAndChildProcesses(Process processToKill, NativeCommandProcessor.ProcessWithParentId[] currentlyRunningProcs)
		{
			try
			{
				int id = processToKill.Id;
				NativeCommandProcessor.KillChildProcesses(id, currentlyRunningProcs);
				processToKill.Kill();
			}
			catch (Win32Exception)
			{
				try
				{
					Process processById = Process.GetProcessById(processToKill.Id);
					processById.Kill();
				}
				catch (Exception e)
				{
					CommandProcessorBase.CheckForSevereException(e);
				}
			}
			catch (Exception e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x000237FC File Offset: 0x000219FC
		private static void KillChildProcesses(int parentId, NativeCommandProcessor.ProcessWithParentId[] currentlyRunningProcs)
		{
			foreach (NativeCommandProcessor.ProcessWithParentId processWithParentId in currentlyRunningProcs)
			{
				if (processWithParentId.ParentId > 0 && processWithParentId.ParentId == parentId)
				{
					NativeCommandProcessor.KillProcessAndChildProcesses(processWithParentId.OriginalProcessInstance, currentlyRunningProcs);
				}
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00023847 File Offset: 0x00021A47
		private static bool IsConsoleApplication(string fileName)
		{
			return !NativeCommandProcessor.IsWindowsApplication(fileName);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00023854 File Offset: 0x00021A54
		[ArchitectureSensitive]
		private static bool IsWindowsApplication(string fileName)
		{
			NativeCommandProcessor.SHFILEINFO shfileinfo = default(NativeCommandProcessor.SHFILEINFO);
			IntPtr value = NativeCommandProcessor.SHGetFileInfo(fileName, 0U, ref shfileinfo, (uint)Marshal.SizeOf(shfileinfo), 8192U);
			int num = (int)value;
			return num != 0 && num != 17744 && num != 23117;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x000238A8 File Offset: 0x00021AA8
		internal void StopProcessing()
		{
			lock (this.sync)
			{
				if (this.stopped)
				{
					return;
				}
				this.stopped = true;
			}
			if (this.nativeProcess != null && !this._runStandAlone)
			{
				this.inputWriter.Stop();
				if (this.outputReader != null)
				{
					this.outputReader.Stop();
				}
				NativeCommandProcessor.KillProcess(this.nativeProcess);
			}
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0002392C File Offset: 0x00021B2C
		private void CleanUp()
		{
			try
			{
				if (this.nativeProcess != null)
				{
					this.nativeProcess.Dispose();
				}
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00023968 File Offset: 0x00021B68
		private void ProcessOutputHelper()
		{
			for (object obj = this.outputReader.Read(); obj != AutomationNull.Value; obj = this.outputReader.Read())
			{
				ProcessOutputObject processOutputObject = obj as ProcessOutputObject;
				if (processOutputObject.Stream == MinishellStream.Error)
				{
					ErrorRecord errorRecord = processOutputObject.Data as ErrorRecord;
					errorRecord.SetInvocationInfo(base.Command.MyInvocation);
					this.commandRuntime._WriteErrorSkipAllowCheck(errorRecord, null);
				}
				else if (processOutputObject.Stream == MinishellStream.Output)
				{
					this.commandRuntime._WriteObjectSkipAllowCheck(processOutputObject.Data);
				}
				else if (processOutputObject.Stream == MinishellStream.Debug)
				{
					string message = processOutputObject.Data as string;
					base.Command.PSHostInternal.UI.WriteDebugLine(message);
				}
				else if (processOutputObject.Stream == MinishellStream.Verbose)
				{
					string message2 = processOutputObject.Data as string;
					base.Command.PSHostInternal.UI.WriteVerboseLine(message2);
				}
				else if (processOutputObject.Stream == MinishellStream.Warning)
				{
					string message3 = processOutputObject.Data as string;
					base.Command.PSHostInternal.UI.WriteWarningLine(message3);
				}
				else if (processOutputObject.Stream == MinishellStream.Progress)
				{
					PSObject psobject = processOutputObject.Data as PSObject;
					if (psobject != null)
					{
						long sourceId = 0L;
						PSMemberInfo psmemberInfo = psobject.Properties["SourceId"];
						if (psmemberInfo != null)
						{
							sourceId = (long)psmemberInfo.Value;
						}
						psmemberInfo = psobject.Properties["Record"];
						ProgressRecord progressRecord = null;
						if (psmemberInfo != null)
						{
							progressRecord = (psmemberInfo.Value as ProgressRecord);
						}
						if (progressRecord != null)
						{
							base.Command.PSHostInternal.UI.WriteProgress(sourceId, progressRecord);
						}
					}
				}
				else if (processOutputObject.Stream == MinishellStream.Information)
				{
					InformationRecord informationRecord = processOutputObject.Data as InformationRecord;
					this.commandRuntime.WriteInformation(informationRecord);
				}
				if (base.Command.Context.CurrentPipelineStopping)
				{
					this.StopProcessing();
					return;
				}
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00023B64 File Offset: 0x00021D64
		private ProcessStartInfo GetProcessStartInfo(bool redirectOutput, bool redirectError, bool redirectInput, bool soloCommand)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = this.Path;
			if (this.ValidateExtension(this.Path))
			{
				processStartInfo.UseShellExecute = false;
				if (redirectInput)
				{
					processStartInfo.RedirectStandardInput = true;
				}
				if (redirectOutput)
				{
					processStartInfo.RedirectStandardOutput = true;
				}
				if (redirectError)
				{
					processStartInfo.RedirectStandardError = true;
				}
			}
			else
			{
				if (!soloCommand)
				{
					throw InterpreterError.NewInterpreterException(this.Path, typeof(RuntimeException), base.Command.InvocationExtent, "CantActivateDocumentInPipeline", ParserStrings.CantActivateDocumentInPipeline, new object[]
					{
						this.Path
					});
				}
				processStartInfo.UseShellExecute = true;
			}
			if (this.isMiniShell)
			{
				MinishellParameterBinderController minishellParameterBinderController = (MinishellParameterBinderController)this.NativeParameterBinderController;
				minishellParameterBinderController.BindParameters(this.arguments, redirectOutput, base.Command.Context.EngineHostInterface.Name);
				processStartInfo.CreateNoWindow = minishellParameterBinderController.NonInteractive;
			}
			processStartInfo.Arguments = this.NativeParameterBinderController.Arguments;
			ExecutionContext context = base.Command.Context;
			string providerPath = context.EngineSessionState.GetNamespaceCurrentLocation(context.ProviderNames.FileSystem).ProviderPath;
			processStartInfo.WorkingDirectory = WildcardPattern.Unescape(providerPath);
			return processStartInfo;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00023C8C File Offset: 0x00021E8C
		private bool IsDownstreamOutDefault(Pipe downstreamPipe)
		{
			CommandProcessorBase downstreamCmdlet = downstreamPipe.DownstreamCmdlet;
			return downstreamCmdlet != null && string.Equals(downstreamCmdlet.CommandInfo.Name, "Out-Default", StringComparison.OrdinalIgnoreCase) && !downstreamCmdlet.Command.MyInvocation.BoundParameters.ContainsKey("Transcript");
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00023CDC File Offset: 0x00021EDC
		private void CalculateIORedirection(out bool redirectOutput, out bool redirectError, out bool redirectInput)
		{
			redirectInput = true;
			redirectOutput = true;
			redirectError = true;
			if (base.Command.MyInvocation.PipelinePosition == base.Command.MyInvocation.PipelineLength && this.IsDownstreamOutDefault(this.commandRuntime.OutputPipe))
			{
				redirectOutput = false;
			}
			if (base.CommandRuntime.ErrorMergeTo != MshCommandRuntime.MergeDataStream.Output && this.IsDownstreamOutDefault(this.commandRuntime.ErrorOutputPipe))
			{
				redirectError = false;
			}
			if (!redirectError && redirectOutput && this.isMiniShell)
			{
				redirectError = true;
			}
			if (this.inputWriter.Count == 0 && !base.Command.MyInvocation.ExpectingInput)
			{
				redirectInput = false;
			}
			if (NativeCommandProcessor.IsServerSide)
			{
				redirectInput = true;
				redirectOutput = true;
				redirectError = true;
			}
			else if (NativeCommandProcessor.IsConsoleApplication(this.Path))
			{
				ConsoleVisibility.AllocateHiddenConsole();
				if (ConsoleVisibility.AlwaysCaptureApplicationIO)
				{
					redirectOutput = true;
					redirectError = true;
				}
			}
			if (!redirectInput && !redirectOutput)
			{
				this._runStandAlone = true;
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00023DC4 File Offset: 0x00021FC4
		private bool ValidateExtension(string path)
		{
			string extension = System.IO.Path.GetExtension(path);
			string text = (string)LanguagePrimitives.ConvertTo(base.Command.Context.GetVariableValue(SpecialVariables.PathExtVarPath), typeof(string), CultureInfo.InvariantCulture);
			string[] array;
			if (string.IsNullOrEmpty(text))
			{
				array = new string[]
				{
					".exe",
					".com",
					".bat",
					".cmd"
				};
			}
			else
			{
				array = text.Split(new char[]
				{
					';'
				});
			}
			foreach (string a in array)
			{
				if (string.Equals(a, extension, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000751 RID: 1873
		[DllImport("shell32.dll", EntryPoint = "FindExecutable")]
		private static extern IntPtr FindExecutableW(string fileName, string directoryPath, StringBuilder pathFound);

		// Token: 0x06000752 RID: 1874 RVA: 0x00023E88 File Offset: 0x00022088
		[ArchitectureSensitive]
		private static string FindExecutable(string filename)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			IntPtr value = (IntPtr)0;
			try
			{
				value = NativeCommandProcessor.FindExecutableW(filename, string.Empty, stringBuilder);
			}
			catch (IndexOutOfRangeException exception)
			{
				WindowsErrorReporting.FailFast(exception);
			}
			if ((long)value >= 32L)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06000753 RID: 1875
		[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref NativeCommandProcessor.SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

		// Token: 0x06000754 RID: 1876 RVA: 0x00023EE4 File Offset: 0x000220E4
		private bool IsMiniShell()
		{
			for (int i = 0; i < this.arguments.Count; i++)
			{
				CommandParameterInternal commandParameterInternal = this.arguments[i];
				if (!commandParameterInternal.ParameterNameSpecified && commandParameterInternal.ArgumentValue is ScriptBlock)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x00023F2C File Offset: 0x0002212C
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x00023F33 File Offset: 0x00022133
		internal static bool IsServerSide
		{
			get
			{
				return NativeCommandProcessor._isServerSide;
			}
			set
			{
				NativeCommandProcessor._isServerSide = value;
			}
		}

		// Token: 0x0400032B RID: 811
		private const int MaxExecutablePath = 1024;

		// Token: 0x0400032C RID: 812
		private const int SCS_32BIT_BINARY = 0;

		// Token: 0x0400032D RID: 813
		private const int SCS_DOS_BINARY = 1;

		// Token: 0x0400032E RID: 814
		private const int SCS_WOW_BINARY = 2;

		// Token: 0x0400032F RID: 815
		private const int SCS_PIF_BINARY = 3;

		// Token: 0x04000330 RID: 816
		private const int SCS_POSIX_BINARY = 4;

		// Token: 0x04000331 RID: 817
		private const int SCS_OS216_BINARY = 5;

		// Token: 0x04000332 RID: 818
		private const int SCS_64BIT_BINARY = 6;

		// Token: 0x04000333 RID: 819
		private const uint SHGFI_EXETYPE = 8192U;

		// Token: 0x04000334 RID: 820
		private ApplicationInfo applicationInfo;

		// Token: 0x04000335 RID: 821
		private bool isPreparedCalled;

		// Token: 0x04000336 RID: 822
		private NativeCommandParameterBinderController nativeParameterBinderController;

		// Token: 0x04000337 RID: 823
		private Process nativeProcess;

		// Token: 0x04000338 RID: 824
		private ProcessInputWriter inputWriter;

		// Token: 0x04000339 RID: 825
		private ProcessOutputReader outputReader;

		// Token: 0x0400033A RID: 826
		private bool _runStandAlone;

		// Token: 0x0400033B RID: 827
		private object sync = new object();

		// Token: 0x0400033C RID: 828
		private bool stopped;

		// Token: 0x0400033D RID: 829
		private bool isMiniShell;

		// Token: 0x0400033E RID: 830
		private static bool _isServerSide;

		// Token: 0x02000095 RID: 149
		internal struct ProcessWithParentId
		{
			// Token: 0x17000206 RID: 518
			// (get) Token: 0x06000757 RID: 1879 RVA: 0x00023F3B File Offset: 0x0002213B
			public int ParentId
			{
				get
				{
					if (-2147483648 == this.parentId)
					{
						this.ConstructParentId();
					}
					return this.parentId;
				}
			}

			// Token: 0x06000758 RID: 1880 RVA: 0x00023F56 File Offset: 0x00022156
			public ProcessWithParentId(Process originalProcess)
			{
				this.OriginalProcessInstance = originalProcess;
				this.parentId = int.MinValue;
			}

			// Token: 0x06000759 RID: 1881 RVA: 0x00023F6C File Offset: 0x0002216C
			public static NativeCommandProcessor.ProcessWithParentId[] Construct(Process[] originalProcCollection)
			{
				NativeCommandProcessor.ProcessWithParentId[] array = new NativeCommandProcessor.ProcessWithParentId[originalProcCollection.Length];
				for (int i = 0; i < originalProcCollection.Length; i++)
				{
					array[i] = new NativeCommandProcessor.ProcessWithParentId(originalProcCollection[i]);
				}
				return array;
			}

			// Token: 0x0600075A RID: 1882 RVA: 0x00023FA8 File Offset: 0x000221A8
			private void ConstructParentId()
			{
				try
				{
					this.parentId = -1;
					Process parentProcess = PsUtils.GetParentProcess(this.OriginalProcessInstance);
					if (parentProcess != null)
					{
						this.parentId = parentProcess.Id;
					}
				}
				catch (Win32Exception)
				{
				}
				catch (InvalidOperationException)
				{
				}
				catch (CimException)
				{
				}
			}

			// Token: 0x0400033F RID: 831
			public Process OriginalProcessInstance;

			// Token: 0x04000340 RID: 832
			private int parentId;
		}

		// Token: 0x02000096 RID: 150
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct SHFILEINFO
		{
			// Token: 0x04000341 RID: 833
			public IntPtr hIcon;

			// Token: 0x04000342 RID: 834
			public int iIcon;

			// Token: 0x04000343 RID: 835
			public uint dwAttributes;

			// Token: 0x04000344 RID: 836
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;

			// Token: 0x04000345 RID: 837
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		}
	}
}
